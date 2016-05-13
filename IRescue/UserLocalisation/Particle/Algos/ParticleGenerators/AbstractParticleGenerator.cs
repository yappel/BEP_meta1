// <copyright file="AbstractParticleGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle.Algos.ParticleGenerators
{
    /// <summary>
    /// Base class for particle generators.
    /// </summary>
    public abstract class AbstractParticleGenerator
    {
        /// <summary>
        /// Generates a new set of particles.
        /// </summary>
        /// <param name="amount"> The amount of particles to generate for every dimension</param>
        /// <param name="dimensions">The amount of dimensions to generate particles for</param>
        /// <param name="maxima">The maximum amount the particles can have in each dimension</param>
        /// <returns></returns>
        public abstract float[] Generate(int amount, int dimensions, double[] maxima);
    }
}
