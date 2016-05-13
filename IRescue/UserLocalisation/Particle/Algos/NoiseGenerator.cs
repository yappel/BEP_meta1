// <copyright file="NoiseGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos
{
    using System;
    using MathNet.Numerics.LinearAlgebra;

    /// <summary>
    /// Generates and adds noise to the values of particles.
    /// </summary>
    public class NoiseGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoiseGenerator"/> class.
        /// </summary>
        /// <param name="particles">The particle to add noise to.</param>
        /// <param name="range">Indicates how much noise can be added to a value. 
        /// The amount of noise on a particle is between -<see cref="range"/> and <see cref="range"/></param>
        public static void Uniform(Matrix<float> particles, double range)
        {
            Random rng_uniform = new System.Random();
            particles.SetSubMatrix(0, 0, particles.Map(c => (float)((((rng_uniform.NextDouble() * 2) - 1) * range) + c)));
        }
    }
}
