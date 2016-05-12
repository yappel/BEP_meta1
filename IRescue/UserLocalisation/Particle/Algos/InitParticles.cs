// <copyright file="InitParticles.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using MathNet.Numerics;

namespace IRescue.UserLocalisation.Particle.Algos
{
    /// <summary>
    /// TODO
    /// </summary>
    public class InitParticles
    {

        public static float[] RandomUniform(int amount, int dimensions, double[] maxima)
        {
            System.Random rng = new System.Random();
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
