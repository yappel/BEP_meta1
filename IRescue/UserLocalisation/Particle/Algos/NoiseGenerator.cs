// <copyright file="NoiseGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace IRescue.UserLocalisation.Particle.Algos
{
    /// <summary>
    /// TODO
    /// </summary>
    public class NoiseGenerator
    {

        public static void Uniform(Matrix<float> particles, double range)
        {
            Random rng_uniform = new System.Random();
            particles.SetSubMatrix(0, 0, particles.Map(c => (float)((rng_uniform.NextDouble() * 2 - 1) * range + c)));
        }
    }
}
