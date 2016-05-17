// <copyright file="RandomNoiseGeneratorTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Linq;
    using Algos.NoiseGenerators;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;

    using NUnit.Framework;

    /// <summary>
    /// Test for random noise generator class
    /// </summary>
    public class RandomNoiseGeneratorTest
    {
        /// <summary>
        /// The particles
        /// </summary>
        private Matrix<float> particles;

        /// <summary>
        /// The object to test
        /// </summary>
        private RandomNoiseGenerator rng;

        /// <summary>
        /// Setup method
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.particles = new SparseMatrix(200, 1);
            this.rng = new RandomNoiseGenerator(new MathNet.Numerics.Random.SystemRandomSource());
        }

        /// <summary>
        /// Test if the particle values changed
        /// </summary>
        [Test]
        public void TestIfNoiseIsAdded()
        {
            this.rng.GenerateNoise(0, 1, this.particles);
            Assert.IsTrue(this.particles.Column(0).ToArray().Max() > 0);
        }

        /// <summary>
        /// Test if the correct amount of noise is added.
        /// </summary>
        [Test]
        public void TestNoiseRangeCorrect()
        {
            this.particles.Clear();
            float max = 1;
            float min = -1;
            this.rng.GenerateNoise(min, max, this.particles);
            Assert.AreEqual(max, this.particles.Column(0).ToArray().Max(), 0.1 * max);
            Assert.AreEqual(min, this.particles.Column(0).ToArray().Min(), 0.1 * Math.Abs(min));
        }
    }
}
