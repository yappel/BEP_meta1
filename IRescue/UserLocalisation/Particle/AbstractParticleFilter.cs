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
    using IRescue.UserLocalisation.PosePrediction;

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
        /// The algorithm used to generate noise and add the to the particles.
        /// </summary>
        private readonly INoiseGenerator noiseGenerator;

        /// <summary>
        /// The particles for the x dimension.
        /// </summary>
        private readonly AbstractParticleController particleControllerX;

        /// <summary>
        /// The particles for the y dimension
        /// </summary>
        private readonly AbstractParticleController particleControllerY;

        /// <summary>
        /// The particles for the z dimension
        /// </summary>
        private readonly AbstractParticleController particleControllerZ;

        /// <summary>
        /// The algorithm used to resample the particles.
        /// </summary>
        private readonly IResampler resampler;

        /// <summary>
        /// Function that calculates the average value given a list of values.
        /// </summary>
        private readonly Func<float[], float> averageCalculator;

        private readonly IExtrapolate iex = new FlexibleExtrapolate();

        private readonly IExtrapolate iey = new FlexibleExtrapolate();

        private readonly IExtrapolate iez = new FlexibleExtrapolate();

        /// <summary>
        /// A percentage of the range of possible values to add as noise to the particles after the resampling step.
        /// </summary>
        private readonly float resampleNoiseSize;

        /// <summary>
        /// The algorithm used to smooth the results of the filter.
        /// </summary>
        private readonly ISmoother smoother;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractParticleFilter"/> class.
        /// </summary>
        /// <param name="resampler"><see cref="resampler"/></param>
        /// <param name="noiseGenerator"><see cref="noiseGenerator"/></param>
        /// <param name="particleControllerX"><see cref="particleControllerX"/></param>
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
            this.particleControllerX = particleControllerX;
            this.particleControllerY = particleControllerY;
            this.particleControllerZ = particleControllerZ;
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

            // Console.WriteLine($"Calculating at timestamp {timeStamp}");
            this.RetrieveMeasurements();

            // foreach (Measurement<Vector3> measurement in this.measurements)
            // {
            // Console.WriteLine($"Retrieved an measurement: {measurement.Data.X} | {measurement.Data.Y} | {measurement.Data.Z}");
            // }
            this.Resample();

            // Console.WriteLine($"Average particle values: {this.averageCalculator(this.particleControllerX.Values)} | {this.averageCalculator(this.particleControllerY.Values)} | {this.averageCalculator(this.particleControllerZ.Values)}");
            // this.Predict();
            this.Update();

            // Console.WriteLine($"Weighted average values: {this.particleControllerX.WeightedAverage()} | {this.particleControllerY.WeightedAverage()} | {this.particleControllerZ.WeightedAverage()}");
            return this.ProcessResults();
        }

        /// <summary>
        /// Calculate the result of the calculation and perform the necessary post calculation actions.
        /// </summary>
        /// <returns>The result of the calculation.</returns>
        protected virtual Vector3 ProcessResults()
        {
            float resultX = this.ProcessResult(this.particleControllerX, this.iex);
            float resultY = this.ProcessResult(this.particleControllerY, this.iey);
            float resultZ = this.ProcessResult(this.particleControllerZ, this.iez);
            Vector3 res = this.smoother.GetSmoothedResult(new Vector3(resultX, resultY, resultZ), this.CurrentTimeStamp, this.averageCalculator);

            // Console.WriteLine($"Results : {res.X} | {res.Y} | {res.Z}");
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
        /// Predict where the next value will be in this timestamp and move the particles accordingly.
        /// </summary>
        private void Predict()
        {
            if (this.PreviousTimeStamp < 0)
            {
                return;
            }

            this.Predict(this.particleControllerX, this.iex);
            this.Predict(this.particleControllerY, this.iey);
            this.Predict(this.particleControllerZ, this.iez);
        }

        private void Predict(AbstractParticleController cont, IExtrapolate ie)
        {
            float prediction = (float)ie.PredictChange(this.PreviousTimeStamp, this.CurrentTimeStamp);
            cont.AddToValues(prediction);
        }

        /// <summary>
        /// Determine the result of a dimension and perform the necessary post calculation actions.
        /// </summary>
        /// <param name="cont">The particles of the dimension.</param>
        /// <param name="ie">The extrapolation algorithm where the results will be added to.</param>
        /// <returns>The value of the result of dimension.</returns>
        private float ProcessResult(AbstractParticleController cont, IExtrapolate ie)
        {
            if (Math.Abs(cont.Weights.Sum()) < float.Epsilon)
            {
                this.noiseGenerator.GenerateNoise(0.01f, cont);
                cont.SetWeights(1f / cont.Count);
            }

            float result = this.GetWeightedAverage(cont);
            ie.AddData(this.CurrentTimeStamp, result);
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

            this.Resample(this.particleControllerX);
            this.Resample(this.particleControllerY);
            this.Resample(this.particleControllerZ);
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
            this.particleControllerX.SetWeights(1f);
            this.particleControllerY.SetWeights(1f);
            this.particleControllerZ.SetWeights(1f);
            if (this.Measurements == null)
            {
                return;
            }

            for (int index = 0; index < this.Measurements.Count; index++)
            {
                Measurement<Vector3> measurement = this.Measurements[index];
                IDistribution dist = measurement.DistributionType;
                float[] diffx = this.particleControllerX.DistanceToValue(measurement.Data.X);
                float[] diffy = this.particleControllerY.DistanceToValue(measurement.Data.Y);
                float[] diffz = this.particleControllerZ.DistanceToValue(measurement.Data.Z);
                this.CalculateWeights(this.particleControllerX, diffx, dist);
                this.CalculateWeights(this.particleControllerY, diffy, dist);
                this.CalculateWeights(this.particleControllerZ, diffz, dist);
            }
        }
    }
}