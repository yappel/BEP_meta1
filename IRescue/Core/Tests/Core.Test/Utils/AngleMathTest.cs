// <copyright file="AngleMathTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace Core.Test
{
    using System;

    using IRescue.Core.Utils;

    using NUnit.Framework;

    /// <summary>
    /// Test for the angle math.
    /// </summary>
    public class AngleMathTest
    {
        /// <summary>
        /// Test getting average angle.
        /// </summary>
        [Test]
        public void TestAverage()
        {
            Assert.AreEqual(5f, AngleMath.Average(new[] { 350f, 20f }), 0.0001);
        }

        /// <summary>
        /// Test getting smallest difference between to angles.
        /// </summary>
        [Test]
        public void TestSmallestAngle()
        {
            Assert.AreEqual(30, AngleMath.SmallesAngle(350f, 20f));
        }

        /// <summary>
        /// Test summing angle.s
        /// </summary>
        [Test]
        public void TestSum()
        {
            Assert.AreEqual(10, AngleMath.Sum(new[] { 350f, 20f }));
        }

        /// <summary>
        /// Test getting weighted average.
        /// </summary>
        [Test]
        public void TestWeightedAverage()
        {
            Assert.AreEqual(0, AngleMath.WeightedAverage(new[] { 90f, 315 }, new float[] { 1f, (float)Math.Sqrt(2) }), 0.001);
        }

        /// <summary>
        /// Test getting weighted average.
        /// </summary>
        [Test]
        public void TestWeightedAverageWrongLength()
        {
            Assert.Throws<ArgumentException>(() => AngleMath.WeightedAverage(new[] { 1f }, new[] { 2f, 2f }));
        }
    }
}
