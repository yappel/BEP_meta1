// <copyright file="LinearParticleControllerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace UserLocalisation.Test.Particle
{
    using System.Linq;

    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class LinearParticleControllerTest
    {
        private LinearParticleController controller;

        private float maxValue;

        private float minValue;

        private int particleAmount;

        private Mock<IParticleGenerator> particleGenerator;

        /// <summary>
        /// Setup of tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.particleGenerator = new Mock<IParticleGenerator>();
            this.particleGenerator.Setup(foo => foo.Generate(It.IsAny<int>(), It.IsAny<float>(), It.IsAny<float>())).Returns<int, float, float>((par, min, max) => Enumerable.Repeat(min, par).ToArray());

            this.particleAmount = 5;
            this.minValue = 0;
            this.maxValue = 1;
            this.controller = new LinearParticleController(this.particleGenerator.Object, this.particleAmount, this.minValue, this.maxValue);
        }



        /// <summary>
        /// Test getting the distance to the values.
        /// </summary>
        [Test]
        public void TestDistanceToValues()
        {
            float x = 1;
            float[] newval = new float[this.particleAmount];
            for (int i = 0; i < this.particleAmount; i++)
            {
                newval[i] = ((1f / (i + 1)) * (this.maxValue - this.minValue)) + this.minValue;
            }

            float[] expected = new float[newval.Length];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = x - newval[i];
            }

            this.controller.Values = newval;
            Assert.AreEqual(expected, this.controller.DistanceToValue(x));
        }



        /// <summary>
        /// Test getting the weighted average.
        /// </summary>
        [Test]
        public void TestWeightedAverage()
        {
            float[] values = { -1f, 0, 1, 1 };
            float[] weights = { 0.5f, 0f, 0.25f, 0.25f };
            this.particleGenerator.Setup(foo => foo.Generate(It.IsAny<int>(), -1, 1)).Returns(values);
            LinearParticleController cont = new LinearParticleController(this.particleGenerator.Object, 4, -1, 1);
            cont.Weights = weights;

            Assert.AreEqual(0, this.controller.WeightedAverage());
        }

        /// <summary>
        /// Test getting the weighted average.
        /// </summary>
        [Test]
        public void TestWeightedAverage2()
        {
            float[] values = { 359, 1 };
            float[] weights = { 0.5f, 0.5f };
            this.particleGenerator.Setup(foo => foo.Generate(It.IsAny<int>(), 0, 359)).Returns(values);
            LinearParticleController cont = new LinearParticleController(this.particleGenerator.Object, 2, 0, 359);
            cont.Weights = weights;

            Assert.AreEqual(180, cont.WeightedAverage());
        }

        /// <summary>
        /// Test getting the weighted average.
        /// </summary>
        [Test]
        public void TestWeightedAverageZeroWeights()
        {
            float[] values = { 359, 1 };
            float[] weights = { 0f, 0f };
            this.particleGenerator.Setup(foo => foo.Generate(It.IsAny<int>(), 0, 359)).Returns(values);
            LinearParticleController cont = new LinearParticleController(this.particleGenerator.Object, 2, 0, 359);
            cont.Weights = weights;

            Assert.AreEqual(float.NaN, cont.WeightedAverage());
        }
    }
}