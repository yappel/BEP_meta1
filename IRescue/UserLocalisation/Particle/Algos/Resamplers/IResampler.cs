// <copyright file="IResampler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos.Resamplers
{
    using MathNet.Numerics.LinearAlgebra;

    /// <summary>
    /// Interface for all particle resample classes.
    /// </summary>
    public interface IResampler
    {
        /// <summary>
        /// Resamples the particles so the ones with low weights do not survive.
        /// </summary>
        /// <param name="particles">The particles to resample</param>
        /// <param name="weights">The weights of the particles</param>
        void Resample(Matrix<float> particles, Matrix<float> weights);
    }
}
