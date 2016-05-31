// <copyright file="RandomNoiseGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle.Algos.NoiseGenerators
{
    using System;
    using System.Linq;

    using MathNet.Numerics.Distributions;

    /// <summary>
    /// Generates and adds noise to the _values of particles bases on a random number generator.
    /// </summary>
    public class RandomNoiseGenerator : INoiseGenerator
    {
        /// <summary>
        /// The random number generator to generate the noise with.
        /// </summary>
        private IContinuousDistribution rng;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNoiseGenerator"/> class.
        /// </summary>
        /// <param name="rng">The random number generator that generates numbers between 0 and 1</param>
        public RandomNoiseGenerator(IContinuousDistribution rng)
        {
            if ((Math.Abs(rng.Minimum) > float.Epsilon) || (Math.Abs(rng.Maximum - 1) > float.Epsilon))
            {
                throw new ArgumentException("The random number generator does not generate numbers with a range of 0 to 1");
            }

            this.rng = rng;
        }

        /// <inheritdoc/>
        public void GenerateNoise(float min, float max, AbstractParticleController particles)
        {
            float[] noisearray = Enumerable.Repeat(0, particles.Count).Select(i => (float)(min + (this.rng.Sample() * (max - min)))).ToArray();
            particles.AddToValues(noisearray);
        }

        /// <inheritdoc/>
        public void GenerateNoise(float percentage, AbstractParticleController particles)
        {
            float noisesize = percentage * (particles.MaxValue - particles.MinValue);
            this.GenerateNoise(-noisesize, noisesize, particles);
        }
    }
}