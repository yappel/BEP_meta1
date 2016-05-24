// <copyright file="UniformTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Test.Distributions
{
    using Core.Distributions;
    using NUnit.Framework;

    /// <summary>
    /// Testing for the normal distribution.
    /// </summary>
    public class UniformTest
    {
        /// <summary>
        /// Test if the normal distribution is a normal distribution.
        /// </summary>
        [Test]
        public void TestMethod()
        {
            double length = 1;
            Uniform uniform = new Uniform(length);
            double mean = 0;
            Assert.AreEqual(0.1, uniform.CDF(mean, mean + (0.05 * length)) - uniform.CDF(mean, mean - (0.05 * length)), 0.01);
        }
    }
}