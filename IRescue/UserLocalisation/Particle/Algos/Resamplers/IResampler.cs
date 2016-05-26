// <copyright file="IResampler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos.Resamplers
{
    using System.Collections.Generic;

    using MathNet.Numerics.LinearAlgebra;

    /// <summary>
    /// Interface for all particle resample classes.
    /// </summary>
    public interface IResampler
    {
        /// <summary>
        /// Resamples the particles so the ones with low weights do not survive.
        /// </summary>
        /// <param name="particles">The class that controls the particles</param>
        void Resample(AbstractParticleController particles);
    }
}
