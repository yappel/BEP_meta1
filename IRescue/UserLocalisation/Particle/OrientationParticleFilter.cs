// <copyright file="OrientationParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Sensors;
    using System.Collections.Generic;

    using IRescue.Core.Utils;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;

    /// <summary>
    /// TODO
    /// </summary>
    internal class OrientationParticleFilter : AbstractParticleFilter, IOrientationReceiver
    {
        private List<IOrientationSource> orientationSources;

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

        protected override void RetrieveMeasurements()
        {
            this.Measurements.Clear();
            foreach (IOrientationSource source in this.orientationSources)
            {
                this.Measurements.AddRange(source.GetOrientationClosestTo(this.CurrentTimeStamp, this.CurrentTimeStamp - this.PreviousTimeStamp));
            }
        }

        /// <summary>
        /// Add a orientation source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        public void AddOrientationSource(IOrientationSource source)
        {
            this.orientationSources.Add(source);
        }
    }
}
