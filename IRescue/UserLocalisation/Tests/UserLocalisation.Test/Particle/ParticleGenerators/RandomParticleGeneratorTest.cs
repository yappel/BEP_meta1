// <copyright file="RandomGeneratorTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Linq;
    using Algos.ParticleGenerators;

    using MathNet.Numerics.Distributions;
    using MathNet.Numerics.Random;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// Tests for the random particle generator class
    /// </summary>
    public class RandomParticleGeneratorTest
    {

        private Mock<IContinuousDistribution> rngsource;

        private double sample = 1f;

        private RandomParticleGenerator rng;


        [SetUp]
        public void SetUp()
        {
            this.rngsource = new Mock<IContinuousDistribution>();
            this.rngsource.SetupGet(foo => foo.Maximum).Returns(1);
            this.rngsource.SetupGet(foo => foo.Minimum).Returns(0);
            this.rngsource.Setup(foo => foo.Sample()).Returns(this.sample);

            this.rng = new RandomParticleGenerator(this.rngsource.Object);
        }

        /// <summary>
        /// Test if the max and min values are actually the max and min values generated.
        /// </summary>
        [Test]
        public void TestSpread()
        {
            float min = -10;
            float max = 10;
            this.rngsource.Setup(foo => foo.Sample()).Returns(0);
            float[] list = this.rng.Generate(1, min, max);
            Assert.AreEqual(min, list[0]);
            this.rngsource.Setup(foo => foo.Sample()).Returns(1);
            list = this.rng.Generate(1, min, max);
            Assert.AreEqual(max, list[0]);
        }

        /// <summary>
        /// Test if the length of the output is correct.
        /// </summary>
        [Test]
        public void TestOutputLength()
        {
            int particleamount = 30;
            float[] list = this.rng.Generate(particleamount, 0, 10);
            Assert.AreEqual(particleamount, list.Length);
        }

        /// <summary>
        /// Test if exception is thrown when the rng is not generating numbers between 0 and 1.
        /// </summary>
        [Test]
        public void TestWrongRNGSource()
        {
            Assert.Throws<ArgumentException>(() => new RandomParticleGenerator(new ContinuousUniform(-1, 1)));
        }
    }
}
