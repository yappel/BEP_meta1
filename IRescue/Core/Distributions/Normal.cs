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
        ///     Initializes a new instance of the <see cref="Normal" /> class.
        /// </summary>
        /// <param name="stddev">The standard deviation of the distribution</param>
        public Normal(double stddev)
        {
            this.Stddev = stddev;
        }

        /// <summary>
        ///     Gets or sets the standard deviation.
        /// </summary>
        public double Stddev { get; set; }

        /// <summary>
        ///     Computes the cumulative distribution (CDF) of the distribution at x given a certain mean, i.e. P(X ≤ x | μ = mean).
        /// </summary>
        /// <param name="mean">The mean (μ) of the distribution.</param>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <returns>The cumulative distribution at location x.</returns>
        public double CDF(double mean, double x)
        {
            return MathNet.Numerics.Distributions.Normal.CDF(mean, this.Stddev, x);
        }
    }
}