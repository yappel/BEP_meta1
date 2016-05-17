// <copyright file="RandomNoiseGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos.NoiseGenerators
{
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
        private RandomSource rng;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNoiseGenerator"/> class.
        /// </summary>
        /// <param name="rng">The random number generator to generate the noise with</param>
        public RandomNoiseGenerator(RandomSource rng)
        {
            this.rng = rng;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoiseGenerator"/> class.
        /// </summary>
        /// <param name="min">THe minimum amount of noise added to a particle</param>
        /// <param name="max">The maximum amount of noise added to a particle</param>
        /// <param name="particles">The particle to add noise to.</param>
        public void GenerateNoise(float min, float max, Matrix<float> particles)
        {
            particles.SetSubMatrix(0, 0, particles.Map(p => (float)(p + min + (this.rng.NextDouble() * (max - min)))));
        }
    }
}
