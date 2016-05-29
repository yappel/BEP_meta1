﻿// <copyright file="OrientationParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Sensors;
    using System.Collections.Generic;


    /// <summary>
    /// TODO
    /// </summary>
    internal class OrientationParticleFilter : AbstractParticleFilter, IOrientationReceiver
    {
        private List<IOrientationSource> orientationSources;

        public OrientationParticleFilter(INoiseGenerator noiseGenerator, float resampleNoiseSize, IResampler resampler, IParticleGenerator particleGenerator, int particleAmount)
            : base(
                  resampler,
                  noiseGenerator,
                  new CircularParticleController(particleGenerator, particleAmount),
                  new CircularParticleController(particleGenerator, particleAmount),
                  new CircularParticleController(particleGenerator, particleAmount),
                  resampleNoiseSize)
        {
            this.orientationSources = new List<IOrientationSource>();
        }

        protected override void RetrieveMeasurements()
        {
            this.measurements.Clear();
            foreach (IOrientationSource source in this.orientationSources)
            {
                this.measurements.AddRange(source.GetOrientationClosestTo(this.currentTimeStamp, this.currentTimeStamp - this.previousTimeStamp));
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
