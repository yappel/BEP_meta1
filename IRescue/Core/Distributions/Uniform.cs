// <copyright file="Uniform.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Distributions
{
    using MathNet.Numerics.Distributions;

    /// <summary>
    ///     The uniform distribution.
    /// </summary>
    public class Uniform : IDistribution
    {
        /// <summary>
        ///     The length of the range of possible values in the distribution.
        /// </summary>
        private readonly double rangelength;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Uniform" /> class.
        /// </summary>
        /// <param name="rangelength">The length of the range of possible values in the distribution.</param>
        public Uniform(double rangelength)
        {
            this.rangelength = rangelength;
        }

        /// <summary>
        ///     Computes the cumulative distribution (CDF) of the distribution at x given a certain mean, i.e. P(X ≤ x | μ = mean).
        /// </summary>
        /// <param name="mean">The mean (μ) of the distribution.</param>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <returns>The cumulative distribution at location x.</returns>
        public double CDF(double mean, double x)
        {
            return ContinuousUniform.CDF(mean - (0.5 * this.rangelength), mean + (0.5 * this.rangelength), x);
        }
    }
}