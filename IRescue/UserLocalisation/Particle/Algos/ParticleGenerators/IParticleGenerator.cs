// <copyright file="IParticleGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle.Algos.ParticleGenerators
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for classes able to generate particles values.
    /// </summary>
    public interface IParticleGenerator
    {
        /// <summary>
        /// Generates a new set of Particles.
        /// </summary>
        /// <param name="amount"> The amount of particles values to generate for every dimension</param>
        /// <param name="min">The minimum value that the generated values can have.</param>
        /// <param name="max">The maximum value that the generated values can have.</param>
        /// <returns>List with generated particle _values</returns>
        float[] Generate(int amount, float min, float max);
    }
}
