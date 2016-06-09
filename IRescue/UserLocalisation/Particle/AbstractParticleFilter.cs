// <copyright file="AbstractParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IRescue.Core.DataTypes;
    using IRescue.Core.Distributions;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;

    /// <summary>
    /// Parent class for filters calculating a 3 dimensional result given 3 dimensional measurements.
    /// </summary>
    public abstract class AbstractParticleFilter
    {
        /// <summary>
        /// The margin used to calculate the probability that a certain value is the real value using CDF functions.
        /// </summary>
        private const double Cdfmargin = 0.005;

        /// <summary>
        /// Function that calculates the average value given a list of _values.
        /// </summary>
        private readonly Func<float[], float> averageCalculator;

        /// <summary>
        /// The algorithm used to generate noise and add the to the particles.
        /// </summary>
        private readonly INoiseGenerator noiseGenerator;

        /// <summary>
        /// A percentage of the range of possible _values to add as noise to the particles after the resampling step.
        /// </summary>
        private readonly float resampleNoiseSize;

        /// <summary>
        /// The algorithm used to resample the particles.
        /// </summary>
        private readonly IResampler resampler;

        /// <summary>
        /// The algorithm used to smooth the results of the filter.
        /// </summary>
        private readonly ISmoother smoother;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractParticleFilter"/> class.
        /// </summary>
        /// <param name="resampler"><see cref="resampler"/></param>
        /// <param name="noiseGenerator"><see cref="noiseGenerator"/></param>
        /// <param name="particleControllerX"><see cref="ParticleControllerX"/></param>
        /// <param name="particleControllerY">The particles for the y dimension</param>
        /// <param name="particleControllerZ">The particles for the z dimension</param>
        /// <param name="resampleNoiseSize"><see cref="resampleNoiseSize"/></param>
        /// <param name="smoother"><see cref="smoother"/></param>
        /// <param name="averageCalculator"><see cref="averageCalculator"/></param>
        protected AbstractParticleFilter(
            IResampler resampler,
            INoiseGenerator noiseGenerator,
            AbstractParticleController particleControllerX,
            AbstractParticleController particleControllerY,
            AbstractParticleController particleControllerZ,
            float resampleNoiseSize,
            ISmoother smoother,
            Func<float[], float> averageCalculator)
        {
            this.resampler = resampler;
            this.noiseGenerator = noiseGenerator;
            this.ParticleControllerX = particleControllerX;
            this.ParticleControllerY = particleControllerY;
            this.ParticleControllerZ = particleControllerZ;
            this.resampleNoiseSize = resampleNoiseSize;
            this.smoother = smoother;
            this.averageCalculator = averageCalculator;
            this.CurrentTimeStamp = -1;
            this.Measurements = new List<Measurement<Vector3>>();
        }

        /// <summary>
        /// Gets or sets the timestamp of the current or, if no calculation is in progress at the moment, the last calculation.
        /// </summary>
        protected long CurrentTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the measurements for the current calculation cycle.
        /// </summary>
        protected List<Measurement<Vector3>> Measurements { get; set; }

        /// <summary>
        /// Gets the particles for the x dimension.
        /// </summary>
        protected AbstractParticleController ParticleControllerX { get; }

        /// <summary>
        /// Gets the particles for the y dimension
        /// </summary>
        protected AbstractParticleController ParticleControllerY { get; }

        /// <summary>
        /// Gets the particles for the z dimension
        /// </summary>
        protected AbstractParticleController ParticleControllerZ { get; }

        /// <summary>
        /// Gets or sets the timestamp of previous or, if no calculation is in progress at the moment, the timestamp before the last calculation.
        /// </summary>
        protected long PreviousTimeStamp { get; set; }

        /// <summary>
        /// Calculates a vector3 on time stamp based on retrieved measurements.
        /// </summary>
        /// <param name="timeStamp">The time stamp to perform the calculation for.</param>
        /// <returns>The calculated vector3</returns>
        public Vector3 Calculate(long timeStamp)
        {
            this.PreviousTimeStamp = this.CurrentTimeStamp;
            this.CurrentTimeStamp = timeStamp;
            this.RetrieveMeasurements();

            this.Resample();
            this.Predict();
            this.Update();

            return this.ProcessResults();
        }

        /// <summary>
        /// Returns the average standard deviation of the distributions described by the particles.
        ///  </summary>
        /// <returns>The average standard deviation.</returns>
        public float GetConfidence()
        {
            return new[] { this.ParticleControllerX.Stddev, this.ParticleControllerY.Stddev, this.ParticleControllerZ.Stddev }.Average();
        }

        /// <summary>
        /// Predict where the next value will be in this timestamp and move the particles accordingly.
        /// </summary>
        protected virtual void Predict()
        {
        }

        /// <summary>
        /// Calculate the result of the calculation and perform the necessary post calculation actions.
        /// </summary>
        /// <returns>The result of the calculation.</returns>
        protected virtual Vector3 ProcessResults()
        {
            float resultX = this.ProcessResult(this.ParticleControllerX);
            float resultY = this.ProcessResult(this.ParticleControllerY);
            float resultZ = this.ProcessResult(this.ParticleControllerZ);
            Vector3 res = this.smoother.GetSmoothedResult(new Vector3(resultX, resultY, resultZ), this.CurrentTimeStamp, this.averageCalculator);

            return res;
        }

        /// <summary>
        /// Get all measurements of the sources and add them to the measurement list.
        /// </summary>
        protected abstract void RetrieveMeasurements();

        /// <summary>
        /// Calculate and set the weights of the particles based on a measurement.
        /// </summary>
        /// <param name="parCon">The particles to calculate and set the weights for</param>
        /// <param name="diffs">List containing the difference between the measurement and a particle value.</param>
        /// <param name="dist">The propability distribution of the measurement.</param>
        private void CalculateWeights(AbstractParticleController parCon, IList<float> diffs, IDistribution dist)
        {
            for (int index = 0; index < diffs.Count; index++)
            {
                float diff = diffs[index];
                float p = (float)(dist.CDF(0, diff + Cdfmargin) - dist.CDF(0, diff - Cdfmargin));
                parCon.SetWeightAt(index, parCon.GetWeightAt(index) * p);
            }
        }

        /// <summary>
        /// Calculates the weighted aveage of the particles.
        /// </summary>
        /// <param name="con">The particles to calculate the weighted average of.</param>
        /// <returns>The weighted average value of the particles.</returns>
        private float GetWeightedAverage(AbstractParticleController con)
        {
            con.NormalizeWeights();
            return con.WeightedAverage();
        }

        /// <summary>
        /// Determine the result of a dimension and perform the necessary post calculation actions.
        /// </summary>
        /// <param name="cont">The particles of the dimension.</param>
        /// <returns>The value of the result of dimension.</returns>
        private float ProcessResult(AbstractParticleController cont)
        {
            if (Math.Abs(cont.Weights.Sum()) < float.Epsilon)
            {
                this.noiseGenerator.GenerateNoise(0.01f, cont);
                cont.SetWeights(1f / cont.Count);
            }

            float result = this.GetWeightedAverage(cont);

            return result;
        }

        /// <summary>
        /// Resample all the particles so that the ones with high weights survive and
        ///  the ones with low weights are removed.
        /// </summary>
        private void Resample()
        {
            if (this.PreviousTimeStamp < 0)
            {
                return;
            }

            this.Resample(this.ParticleControllerX);
            this.Resample(this.ParticleControllerY);
            this.Resample(this.ParticleControllerZ);
        }

        private void Resample(AbstractParticleController particleController)
        {
            this.resampler.Resample(particleController);
            this.noiseGenerator.GenerateNoise(this.resampleNoiseSize, particleController);
        }

        /// <summary>
        /// Calculate and set the weights of the particles based on the measurements.
        /// </summary>
        private void Update()
        {
            this.ParticleControllerX.SetWeights(1f);
            this.ParticleControllerY.SetWeights(1f);
            this.ParticleControllerZ.SetWeights(1f);

            for (int index = 0; index < this.Measurements.Count; index++)
            {
                Measurement<Vector3> measurement = this.Measurements[index];
                IDistribution dist = measurement.DistributionType;
                float[] diffx = this.ParticleControllerX.DistanceToValue(measurement.Data.X);
                float[] diffy = this.ParticleControllerY.DistanceToValue(measurement.Data.Y);
                float[] diffz = this.ParticleControllerZ.DistanceToValue(measurement.Data.Z);
                this.CalculateWeights(this.ParticleControllerX, diffx, dist);
                this.CalculateWeights(this.ParticleControllerY, diffy, dist);
                this.CalculateWeights(this.ParticleControllerZ, diffz, dist);
            }
        }
    }
}