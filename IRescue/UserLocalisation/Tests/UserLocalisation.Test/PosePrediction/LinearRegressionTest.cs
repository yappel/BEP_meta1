// <copyright file="LinearRegressionTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace UserLocalisation.Test.Particle.PosePrediction
{
    using IRescue.UserLocalisation.PosePrediction;
    using NUnit.Framework;

    /// <summary>
    ///     Test for the particles
    /// </summary>
    public class LinearRegressionTest
    {
        private LinearRegression linearRegression;

        /// <summary>
        /// Setup for the tests
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.linearRegression = new LinearRegression();
            this.linearRegression.AddData(0, 10);
            this.linearRegression.AddData(1, 20);
        }

        /// <summary>
        /// Test if a predict is correct
        /// </summary>
        [Test]
        public void TestPredictZero()
        {
            Assert.AreEqual(0, this.linearRegression.PredictValueAt(3));
        }

        /// <summary>
        /// Test if a predict is correct
        /// </summary>
        [Test]
        public void TestPredict()
        {
            this.linearRegression.AddData(2, 30);
            Assert.AreEqual(40, this.linearRegression.PredictValueAt(3), 0.01f);
        }

        /// <summary>
        /// Test if a predict is correct
        /// </summary>
        [Test]
        public void TestPredict2()
        {
            this.linearRegression.AddData(2, 25);
            Assert.AreEqual(33.33, this.linearRegression.PredictValueAt(3), 0.01);
        }

        /// <summary>
        /// Test if a predict change is correct
        /// </summary>
        [Test]
        public void TestPredictChange()
        {
            this.linearRegression.AddData(2, 25);
            Assert.AreEqual(30, this.linearRegression.PredictChange(0, 4), 0.001);
        }
    }
}