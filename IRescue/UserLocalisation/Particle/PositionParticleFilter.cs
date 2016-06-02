// <copyright file="PositionParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using System.Collections.Generic;
    using System.Linq;

    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;
    using IRescue.UserLocalisation.PosePrediction;
    using IRescue.UserLocalisation.Sensors;

    /// <summary>
    /// Particle filter that calculates the cartesian x y z coordinates of the user.
    /// </summary>
    internal class PositionParticleFilter : AbstractParticleFilter, IPositionReceiver, IDisplacementReceiver
    {
        /// <summary>
        /// List containing all tho <see cref="IDisplacementSource"/> sources.
        /// </summary>
        private readonly List<IDisplacementSource> displacementSources;

        /// <summary>
        /// The algorithm used for the predict step in the x dimension.
        /// </summary>
        private readonly IExtrapolate iex;

        /// <summary>
        /// The algorithm used for the predict step in the x dimension.
        /// </summary>
        private readonly IExtrapolate iey;

        /// <summary>
        /// The algorithm used for the predict step in the x dimension.
        /// </summary>
        private readonly IExtrapolate iez;

        /// <summary>
        /// List containing all tho <see cref="IPositionSource"/> sources.
        /// </summary>
        private readonly List<IPositionSource> positionSources;

        /// <summary>
        /// The result of the previous calculation.
        /// </summary>
        private Vector3 previousResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionParticleFilter"/> class.
        /// </summary>
        /// <param name="noiseGenerator">See noiseGenerator argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="resampleNoiseSize">See resampleNoiseSize argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="resampler">See resampler argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="particleGenerator">See particleGenerator argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="particleAmount">See particleAmount argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="fieldSize">The dimensions of the area where the user can be.</param>
        /// <param name="smoother">See smoother argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        public PositionParticleFilter(
            INoiseGenerator noiseGenerator,
            float resampleNoiseSize,
            IResampler resampler,
            IParticleGenerator particleGenerator,
            int particleAmount,
            FieldSize fieldSize,
            ISmoother smoother)
            : base(
                resampler,
                noiseGenerator,
                new LinearParticleController(particleGenerator, particleAmount, fieldSize.Xmin, fieldSize.Xmax),
                new LinearParticleController(particleGenerator, particleAmount, fieldSize.Ymin, fieldSize.Ymax),
                new LinearParticleController(particleGenerator, particleAmount, fieldSize.Zmin, fieldSize.Zmax),
                resampleNoiseSize,
                smoother,
                Enumerable.Average)
        {
            this.displacementSources = new List<IDisplacementSource>();
            this.positionSources = new List<IPositionSource>();
            this.iex = new LinearRegression();
            this.iey = new LinearRegression();
            this.iez = new LinearRegression();
        }

        /// <summary>
        /// Add a displacement source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        public void AddDisplacementSource(IDisplacementSource source)
        {
            this.displacementSources.Add(source);
        }

        /// <summary>
        /// Add a position source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        public void AddPositionSource(IPositionSource source)
        {
            this.positionSources.Add(source);
        }

        /// <inheritdoc/>
        protected override void Predict()
        {
            if (this.PreviousTimeStamp < 0)
            {
                return;
            }

            this.Predict(this.ParticleControllerX, this.iex);
            this.Predict(this.ParticleControllerY, this.iey);
            this.Predict(this.ParticleControllerZ, this.iez);
        }

        /// <inheritdoc/>
        protected override Vector3 ProcessResults()
        {
            Vector3 result = base.ProcessResults();
            this.previousResult = result;
            this.iex.AddData(this.CurrentTimeStamp, result.X);
            this.iey.AddData(this.CurrentTimeStamp, result.Y);
            this.iez.AddData(this.CurrentTimeStamp, result.Z);
            return result;
        }

        /// <inheritdoc/>
        protected override void RetrieveMeasurements()
        {
            this.Measurements.Clear();
            this.CheckPositionSources();
            if (this.previousResult != null)
            {
                this.CheckDislocationSources();
            }
        }

        /// <summary>
        /// Check if there are dislocation measurements.
        /// If there are convert them to position measurements and add them to the list of measurements.
        /// </summary>
        private void CheckDislocationSources()
        {
            for (int i = 0; i < this.displacementSources.Count; i++)
            {
                Measurement<Vector3> meas = this.displacementSources[i].GetDisplacement(this.PreviousTimeStamp, this.CurrentTimeStamp);
                meas.Data.Add(this.previousResult, meas.Data);
                this.Measurements.Add(meas);
            }
        }

        /// <summary>
        /// Check if there are position measurements and add them to the list if there are.
        /// </summary>
        private void CheckPositionSources()
        {
            for (int i = 0; i < this.positionSources.Count; i++)
            {
                this.Measurements.AddRange(this.positionSources[i].GetPositionsClosestTo(this.CurrentTimeStamp, this.CurrentTimeStamp - this.PreviousTimeStamp));
            }
        }

        /// <summary>
        /// Predicts the value for the current timestamp and move the particles.
        /// </summary>
        /// <param name="cont">The dimension to predict in.</param>
        /// <param name="ie">The prediction algorithm to use.</param>
        private void Predict(AbstractParticleController cont, IExtrapolate ie)
        {
            float prediction = (float)ie.PredictChange(this.PreviousTimeStamp, this.CurrentTimeStamp);
            cont.AddToValues(prediction);
        }
    }
}