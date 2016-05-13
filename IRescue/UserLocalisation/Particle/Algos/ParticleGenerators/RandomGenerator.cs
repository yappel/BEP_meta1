// <copyright file="InitParticles.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.Random;

namespace IRescue.UserLocalisation.Particle.Algos.ParticleGenerators
{
    /// <summary>
    /// TODO
    /// </summary>
    public class RandomGenerator : AbstractParticleGenerator
    {

        private RandomSource rng;
        public RandomGenerator(RandomSource rng)
        {
            this.rng = rng;
        }
        public override float[] Generate(int amount, int dimensions, double[] maxima)
        {
            float[] result = new float[amount * dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < amount; j++)
                {
                    result[i * amount + j] = (float)(rng.NextDouble() * maxima[i]);
                }
            }
            return result;
        }
    }
}
