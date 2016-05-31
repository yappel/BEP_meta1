// <copyright file="RandomParticleGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle.Algos.ParticleGenerators
{
    using System;
    using System.Linq;

    using MathNet.Numerics.Distributions;

    /// <summary>
    /// A Particles generator that generates Particles using a random number generator
    /// </summary>
    public class RandomParticleGenerator : IParticleGenerator
    {
        /// <summary>
        /// The random number generator that generates a number between 0.0 and 1.0.
        /// </summary>
        private IContinuousDistribution rng;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomParticleGenerator"/> class.
        /// </summary>
        /// <param name="rng">The random number generator that generates a number between 0.0 and 1.0.</param>
        public RandomParticleGenerator(IContinuousDistribution rng)
        {
            if ((Math.Abs(rng.Minimum) > float.Epsilon) || (Math.Abs(rng.Maximum - 1) > float.Epsilon))
            {
                throw new ArgumentException("The random number generator does not generate numbers with a range of 0 to 1");
            }

            this.rng = rng;
        }

        /// <inheritdoc/>
        public float[] Generate(int amount, float min, float max)
        {
            return Enumerable.Repeat(0, amount).Select(i => this.RandNum(min, max)).ToArray();
        }

        private float RandNum(float min, float max)
        {
            return (float)((this.rng.Sample() * (max - min)) + min);
        }
    }
}