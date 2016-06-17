// <copyright file="MovingAverageSmootherTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace UserLocalisation.Test.Particle.Smoothers
{
    using System.Linq;

    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;

    using NUnit.Framework;

    /// <summary>
    /// Test moving average smoother.
    /// </summary>
    public class MovingAverageSmootherTest
    {
        /// <summary>
        /// Test moving average smoother.
        /// </summary>
        [Test]
        public void TestAverage()
        {
            MovingAverageSmoother smoother = new MovingAverageSmoother(100);
            Vector3 expected = new Vector3(1, 1, 1);
            Vector3 actual = smoother.GetSmoothedResult(expected, 1, Enumerable.Average);
            Assert.AreEqual(expected, actual);
            expected = new Vector3(2, 2, 2);
            actual = smoother.GetSmoothedResult(new Vector3(3, 3, 3), 50, Enumerable.Average);
            Assert.AreEqual(expected, actual);
            expected = new Vector3(4, 4, 4);
            actual = smoother.GetSmoothedResult(new Vector3(5, 5, 5), 120, Enumerable.Average);
            Assert.AreEqual(expected, actual);
        }
    }
}