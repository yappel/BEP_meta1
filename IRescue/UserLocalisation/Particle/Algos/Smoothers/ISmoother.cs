// <copyright file="ISmoother.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos.Smoothers
{
    using System;
    using System.Collections.Generic;

    using IRescue.Core.DataTypes;

    /// <summary>
    /// Interface for classes that can smooth results of the filter
    /// </summary>
    public interface ISmoother
    {
        /// <summary>
        /// Calculates the smoothed result.
        /// </summary>
        /// <param name="rawResult">The unsmoothed result.</param>
        /// <param name="timeStamp">The timestamp of the (un)smoothed result.</param>
        /// <returns>The smoothed result</returns>
        Vector3 GetSmoothedResult(Vector3 rawResult, long timeStamp, Func<float[], float> averageFunction);

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        ISmoother Clone();
    }
}