// <copyright file="Normal.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Distributions
{
    /// <summary>
    ///     The normal distribution.
    /// </summary>
    public class Normal : IDistribution
    {
        /// <summary>
        ///     The standard deviation.
        /// </summary>
        private readonly double stddev;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Normal" /> class.
        /// </summary>
        /// <param name="stddev">The standard deviation of the distribution</param>
        public Normal(double stddev)
        {
            this.stddev = stddev;
        }

        /// <summary>
        ///     Computes the cumulative distribution (CDF) of the distribution at x given a certain mean, i.e. P(X ≤ x | μ = mean).
        /// </summary>
        /// <param name="mean">The mean (μ) of the distribution.</param>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <returns>The cumulative distribution at location x.</returns>
        public double CDF(double mean, double x)
        {
            return MathNet.Numerics.Distributions.Normal.CDF(mean, this.stddev, x);
        }
    }
}