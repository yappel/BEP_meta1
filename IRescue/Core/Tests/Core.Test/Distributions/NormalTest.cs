// <copyright file="NormalTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Test.Distributions
{
    using Core.Distributions;
    using NUnit.Framework;

    /// <summary>
    /// Testing for the normal distribution.
    /// </summary>
    public class NormalTest
    {
        /// <summary>
        /// Test if the normal distribution is a normal distribution.
        /// </summary>
        [Test]
        public void TestMethod()
        {
            float stddev = 0.1f;
            Normal normal = new Normal(stddev);
            double mean = 0;
            Assert.AreEqual(0.95, normal.CDF(mean, mean + (2 * stddev)) - normal.CDF(mean, mean - (2 * stddev)), 0.01);
        }
    }
}
