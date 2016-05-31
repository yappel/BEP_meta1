// <copyright file="OrientationParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using System.Collections.Generic;

    using IRescue.Core.Utils;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;
    using IRescue.UserLocalisation.Sensors;

    /// <summary>
    /// Particle filter that determines the x y z Tait–Bryan angles of the user at a given timestamp.
    /// </summary>
    internal class OrientationParticleFilter : AbstractParticleFilter, IOrientationReceiver
    {
        /// <summary>
        /// List containing all the <see cref="IOrientationSource"/>s
        /// </summary>
        private List<IOrientationSource> orientationSources;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrientationParticleFilter"/> class.
        /// </summary>
        /// <param name="noiseGenerator">See noiseGenerator argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="resampleNoiseSize">See resampleNoiseSize argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="resampler">See resampler argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="particleGenerator">See particleGenerator argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="particleAmount">See particleAmount argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="smoother">See smoother argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        public OrientationParticleFilter(INoiseGenerator noiseGenerator, float resampleNoiseSize, IResampler resampler, IParticleGenerator particleGenerator, int particleAmount, ISmoother smoother)
            : base(
                resampler,
                noiseGenerator,
                new CircularParticleController(particleGenerator, particleAmount),
                new CircularParticleController(particleGenerator, particleAmount),
                new CircularParticleController(particleGenerator, particleAmount),
                resampleNoiseSize,
                smoother,
                AngleMath.Average)
        {
            this.orientationSources = new List<IOrientationSource>();
        }

        /// <summary>
        /// Add a orientation source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        public void AddOrientationSource(IOrientationSource source)
        {
            this.orientationSources.Add(source);
        }

        /// <inheritdoc/>
        protected override void RetrieveMeasurements()
        {
            this.Measurements.Clear();
            foreach (IOrientationSource source in this.orientationSources)
            {
                this.Measurements.AddRange(source.GetOrientationClosestTo(this.CurrentTimeStamp, this.CurrentTimeStamp - this.PreviousTimeStamp));
            }
        }
    }
}