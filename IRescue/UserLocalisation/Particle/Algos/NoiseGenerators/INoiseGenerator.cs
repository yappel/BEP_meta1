// <copyright file="INoiseGenerator.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos.NoiseGenerators
{
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
        void GenerateNoise(float min, float max, AbstractParticleController particles);

        /// <summary>
        /// Generates noise and adds it to the particles.
        /// </summary>
        /// <param name="percentage">The amount of noise to add to the particles.
        ///  The maximum added noise is this number times the rangelength of possible values.</param>
        /// <param name="particles">The particles to add the noise to.</param>
        void GenerateNoise(float percentage, AbstractParticleController particles);
    }
}
