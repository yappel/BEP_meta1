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


    /// <summary>
    /// TODO
    /// </summary>
    public class OrientationParticleFilter : AbstractParticleFilter, IOrientationReceiver
    {
        private List<IOrientationSource> orientationSources;

        public OrientationParticleFilter(INoiseGenerator noiseGenerator, float resampleNoiseSize, IResampler resampler, IParticleGenerator particleGenerator, int particleAmount)
            : base(
                  noiseGenerator,
                  new CircularParticleController(resampler, particleGenerator, particleAmount, 0, 360),
                  new CircularParticleController(resampler, particleGenerator, particleAmount, 0, 360),
                  new CircularParticleController(resampler, particleGenerator, particleAmount, 0, 360),
                  resampleNoiseSize)
        {
            this.orientationSources = new List<IOrientationSource>();
        }

        protected override void RetrieveMeasurements()
        {
            foreach (IOrientationSource source in this.orientationSources)
            {
                ////TODO interface changes
                this.measurements.Add(source.GetLastOrientation());
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
