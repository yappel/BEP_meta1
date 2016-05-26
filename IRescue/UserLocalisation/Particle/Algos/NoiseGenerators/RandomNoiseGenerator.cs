// <copyright file="RandomNoiseGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos.NoiseGenerators
{
    using System.Linq;

    using MathNet.Numerics.Distributions;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.Random;

    /// <summary>
    /// Generates and adds noise to the values of particles bases on a random number generator.
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
        /// <param name="rng">The random number generator to generate the noise with</param>
        public RandomNoiseGenerator(IContinuousDistribution rng)
        {
            this.rng = rng;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNoiseGenerator"/> class.
        /// </summary>
        /// <param name="min">THe minimum amount of noise added to a particle</param>
        /// <param name="max">The maximum amount of noise added to a particle</param>
        /// <param name="particles">The particle to add noise to.</param>
        public void GenerateNoise(float min, float max, AbstractParticleController particles)
        {
            float[] noisearray = Enumerable.Repeat(0, particles.Count).Select(i => (float)(min + (this.rng.Sample() * (max - min)))).ToArray();
            particles.AddToValues(noisearray);
        }

        public void GenerateNoise(float percentage, AbstractParticleController particles)
        {
            float noisesize = percentage * (particles.maxValue - particles.minValue);
            this.GenerateNoise(-noisesize, noisesize, particles);
        }
    }
}
