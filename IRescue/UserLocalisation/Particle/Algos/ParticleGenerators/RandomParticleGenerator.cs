﻿// <copyright file="RandomParticleGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos.ParticleGenerators
{
    using MathNet.Numerics.Random;

    /// <summary>
    /// A Particles generator that generates Particles using a random number generator
    /// </summary>
    public class RandomParticleGenerator : IParticleGenerator
    {
        /// <summary>
        /// The random number generator to generate the Particles with
        /// </summary>
        private RandomSource rng;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomParticleGenerator"/> class.
        /// </summary>
        /// <param name="rng">The random number generator to generate the Particles with</param>
        public RandomParticleGenerator(RandomSource rng)
        {
            this.rng = rng;
        }

        /// <summary>
        /// Generates a new set of Particles.
        /// </summary>
        /// <param name="amount"> The amount of Particles to generate for every dimension</param>
        /// <param name="dimensions">The amount of dimensions to generate Particles for</param>
        /// <returns>A list of particle values</returns>
        public float[] Generate(int amount, int dimensions)
        {
            float[] result = new float[amount * dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < amount; j++)
                {
                    result[(i * amount) + j] = (float)this.rng.NextDouble();
                }
            }

            return result;
        }
    }
}
