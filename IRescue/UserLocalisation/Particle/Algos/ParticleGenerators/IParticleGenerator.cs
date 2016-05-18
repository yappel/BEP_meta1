// <copyright file="IParticleGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle.Algos.ParticleGenerators
{
    /// <summary>
    /// Interface for classes able to generate particles.
    /// </summary>
    public interface IParticleGenerator
    {
        /// <summary>
        /// Generates a new set of Particles.
        /// </summary>
        /// <param name="amount"> The amount of Particles to generate for every dimension</param>
        /// <param name="dimensions">The amount of dimensions to generate Particles for</param>
        /// <param name="minima">The minimum value the Particles can have in each dimension</param>
        /// <param name="maxima">The maximum value the Particles can have in each dimension</param>
        /// <returns>List with generated particle values</returns>
        float[] Generate(int amount, int dimensions);
    }
}
