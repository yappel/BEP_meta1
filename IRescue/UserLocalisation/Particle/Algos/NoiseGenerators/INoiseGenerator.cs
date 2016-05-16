﻿// <copyright file="INoiseGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos.NoiseGenerators
{
    using MathNet.Numerics.LinearAlgebra;

    /// <summary>
    /// Generates noise within a range.
    /// </summary>
    public interface INoiseGenerator
    {
        /// <summary>
        /// Generates noise and adds it to the particles.
        /// </summary>
        /// <param name="min">The minimum amount of noise that can be added to a particle.</param>
        /// <param name="max">The maximum amount of noise that can be added to a particle.</param>
        /// <param name="particles">The particles to add the noise to.</param>
        void GenerateNoise(float min, float max, Matrix<float> particles);
    }
}