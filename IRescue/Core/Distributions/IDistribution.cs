// <copyright file="IDistribution.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Distributions
{
    /// <summary>
    /// Interface for different type of probability distributions.
    /// </summary>
    public interface IDistribution
    {
        /// <summary>
        ///     Computes the cumulative distribution (CDF) of the distribution at x given a certain mean, i.e. P(X ≤ x | μ = mean).
        /// </summary>
        /// <param name="mean">The mean (μ) of the distribution.</param>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <returns>The cumulative distribution at location x.</returns>
        double CDF(double mean, double x);
    }
}