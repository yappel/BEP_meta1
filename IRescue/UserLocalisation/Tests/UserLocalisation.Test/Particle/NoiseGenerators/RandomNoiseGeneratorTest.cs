// <copyright file="RandomNoiseGeneratorTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace UserLocalisation.Test.Particle.NoiseGenerators
{
    using System;
    using System.Linq;

    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;

    using MathNet.Numerics.Distributions;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// Test for random noise generator class
    /// </summary>
    public class RandomNoiseGeneratorTest
    {
        /// <summary>
        /// The particles
        /// </summary>
        private AbstractParticleController particles;

        private Mock<IContinuousDistribution> rngsource;

        /// <summary>
        /// The object to test
        /// </summary>
        private RandomNoiseGenerator rng;

        private float addedNoise = 0.5f;

        private int particleCount = 5;

        private Mock<IParticleGenerator> particleGenerator;

        /// <summary>
        /// Setup method
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.rngsource = new Mock<IContinuousDistribution>();
            this.rngsource.SetupGet(foo => foo.Maximum).Returns(1);
            this.rngsource.SetupGet(foo => foo.Minimum).Returns(0);
            this.rngsource.Setup(foo => foo.Sample()).Returns(this.addedNoise);

            this.particleGenerator = new Mock<IParticleGenerator>();
            this.particleGenerator.Setup(foo => foo.Generate(It.IsAny<int>(), It.IsAny<float>(), It.IsAny<float>()))
                .Returns<int, float, float>((par, min, max) => Enumerable.Repeat(min, par).ToArray());

            this.particles = new LinearParticleController(this.particleGenerator.Object, this.particleCount, 0, 1);

            this.rng = new RandomNoiseGenerator(this.rngsource.Object);
        }

        /// <summary>
        /// Test if the particle values changed
        /// </summary>
        [Test]
        public void TestIfNoiseIsAdded()
        {
            float percentage = 0.1f;
            this.rngsource.Setup(foo => foo.Sample()).Returns(1);
            this.rng.GenerateNoise(percentage, this.particles);
            float[] expected = Enumerable.Repeat(percentage, this.particleCount).ToArray();
            Assert.AreEqual(expected, this.particles.Values);
        }

        /// <summary>
        /// Test if the particle values changed
        /// </summary>
        [Test]
        public void TestIfNoiseIsAdded2()
        {
            float min = 0;
            float max = 1;
            this.rng.GenerateNoise(min, max, this.particles);
            float[] expected = Enumerable.Repeat(0.5f, this.particleCount).ToArray();
            Assert.AreEqual(expected, this.particles.Values);
        }

        /// <summary>
        /// Test if the particle values changed
        /// </summary>
        [Test]
        public void TestIfNoiseIsAdded3()
        {
            float min = -1;
            float max = 1;
            this.rng.GenerateNoise(min, max, this.particles);
            float[] expected = Enumerable.Repeat(0f, this.particleCount).ToArray();
            Assert.AreEqual(expected, this.particles.Values);
        }

        /// <summary>
        /// Test if the particle values changed
        /// </summary>
        [Test]
        public void TestIfNoiseIsAdded4()
        {
            float min = -1;
            float max = 0;
            this.particles.Values = Enumerable.Repeat(1f, this.particles.Count).ToArray();
            this.rng.GenerateNoise(min, max, this.particles);
            float[] expected = Enumerable.Repeat(0.5f, this.particleCount).ToArray();
            Assert.AreEqual(expected, this.particles.Values);
        }

        /// <summary>
        /// Test if the correct amount of noise is added.
        /// </summary>
        [Test]
        public void TestNoiseRangeCorrect()
        {
        }

        /// <summary>
        /// Test if exception is thrown when the rng is not generating numbers between 0 and 1.
        /// </summary>
        [Test]
        public void TestWrongRNGSource()
        {
            Assert.Throws<ArgumentException>(() => new RandomNoiseGenerator(new ContinuousUniform(-1, 1)));
        }
    }
}
