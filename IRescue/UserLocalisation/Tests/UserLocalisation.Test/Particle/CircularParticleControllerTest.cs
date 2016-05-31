// <copyright file="CircularParticleControllerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace UserLocalisation.Test.Particle
{
    using System;
    using System.Linq;

    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// Test for circular particle controller.
    /// </summary>
    public class CircularParticleControllerTest
    {
        private CircularParticleController controller;

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
            this.controller = new CircularParticleController(this.particleGenerator.Object, this.particleAmount);
            this.minValue = this.controller.MinValue;
            this.maxValue = this.controller.MaxValue;
        }

        /// <summary>
        /// Test adding _values.
        /// </summary>
        [Test]
        public void TestAddingValues()
        {
            float[] begin = Enumerable.Repeat(0f, this.particleAmount).ToArray();
            Assert.AreEqual(begin, this.controller.Values);

            float[] expected = Enumerable.Repeat(90f, this.particleAmount).ToArray();
            this.controller.AddToValues(expected);
            Assert.AreEqual(expected, this.controller.Values);

            this.controller.AddToValues(expected);
            expected = Enumerable.Repeat(180f, this.particleAmount).ToArray();
            Assert.AreEqual(expected, this.controller.Values);

            this.controller.AddToValues(expected);
            expected = Enumerable.Repeat(0f, this.particleAmount).ToArray();
            Assert.AreEqual(expected, this.controller.Values);

            this.controller.AddToValues(Enumerable.Repeat(-90f, this.particleAmount).ToArray());
            expected = Enumerable.Repeat(270f, this.particleAmount).ToArray();
            Assert.AreEqual(expected, this.controller.Values);

            this.controller.AddToValues(Enumerable.Repeat(-720f, this.particleAmount).ToArray());
            expected = Enumerable.Repeat(270f, this.particleAmount).ToArray();
            Assert.AreEqual(expected, this.controller.Values);

            this.controller.AddToValues(Enumerable.Repeat(89.5f, this.particleAmount).ToArray());
            expected = Enumerable.Repeat(359.5f, this.particleAmount).ToArray();
            Assert.AreEqual(expected, this.controller.Values);
        }

        /// <summary>
        /// Test if exception is thrown when wrong input array length is given;
        /// </summary>
        [Test]
        public void TestAddingValuesWrongLength()
        {
            float[] expected = Enumerable.Repeat(1f, this.particleAmount - 1).ToArray();
            Assert.Throws<ArgumentException>(() => this.controller.AddToValues(expected));
            expected = Enumerable.Repeat(1f, this.particleAmount + 1).ToArray();
            Assert.Throws<ArgumentException>(() => this.controller.AddToValues(expected));
        }

        /// <summary>
        /// Test getting the distance to the _values.
        /// </summary>
        [Test]
        public void TestDistanceToValues()
        {
            float x = 359;
            float[] newval = new float[this.particleAmount];
            for (int i = 0; i < this.particleAmount; i++)
            {
                newval[i] = this.minValue + 1;
            }

            float[] expected = new float[newval.Length];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = -2;
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
            float[] values = { 359f, 0, 1, 1 };
            float[] weights = { 1f, 0f, 0.5f, 0.5f };
            Mock<IParticleGenerator> pargen = new Mock<IParticleGenerator>();
            pargen.Setup(foo => foo.Generate(It.IsAny<int>(), this.minValue, this.maxValue)).Returns(values);
            CircularParticleController cont = new CircularParticleController(pargen.Object, values.Length) { Weights = weights };

            Assert.AreEqual(0, cont.WeightedAverage());
        }

        /// <summary>
        /// Test getting the weighted average.
        /// </summary>
        [Test]
        public void TestWeightedAverage2()
        {
            float[] values = { 359, 181 };
            float[] weights = { 0.5f, 0.5f };
            Mock<IParticleGenerator> pargen = new Mock<IParticleGenerator>();
            pargen.Setup(foo => foo.Generate(It.IsAny<int>(), this.minValue, this.maxValue)).Returns(values);
            CircularParticleController cont = new CircularParticleController(pargen.Object, values.Length) { Weights = weights };

            Assert.AreEqual(270, cont.WeightedAverage());
        }

        /// <summary>
        /// Test getting the weighted average.
        /// </summary>
        [Test]
        public void TestWeightedAverage3()
        {
            float[] values = { 271f, 0, 89f, 89f };
            float[] weights = { 1f, 0f, 0.5f, 0.5f };
            Mock<IParticleGenerator> pargen = new Mock<IParticleGenerator>();
            pargen.Setup(foo => foo.Generate(It.IsAny<int>(), this.minValue, this.maxValue)).Returns(values);
            CircularParticleController cont = new CircularParticleController(pargen.Object, values.Length) { Weights = weights };

            Assert.AreEqual(0, cont.WeightedAverage());
        }

        /// <summary>
        /// Test getting the weighted average when none can be calculated (e.g. average between the angle 180 and 0 ).
        /// </summary>
        [Test]
        public void TestWeightedAverageNaN()
        {
            float[] values = { 270f, 90f };
            float[] weights = { 0.5f, 0.5f };
            Mock<IParticleGenerator> pargen = new Mock<IParticleGenerator>();
            pargen.Setup(foo => foo.Generate(It.IsAny<int>(), this.minValue, this.maxValue)).Returns(values);
            CircularParticleController cont = new CircularParticleController(pargen.Object, values.Length) { Weights = weights };

            Assert.AreEqual(float.NaN, cont.WeightedAverage());
        }

        /// <summary>
        /// Test getting the weighted average when sum of weights is zero.
        /// </summary>
        [Test]
        public void TestWeightedAverageZeroWeights()
        {
            float[] values = { 270f, 90f };
            float[] weights = { 0f, 0f };
            Mock<IParticleGenerator> pargen = new Mock<IParticleGenerator>();
            pargen.Setup(foo => foo.Generate(It.IsAny<int>(), this.minValue, this.maxValue)).Returns(values);
            CircularParticleController cont = new CircularParticleController(pargen.Object, values.Length) { Weights = weights };

            Assert.AreEqual(float.NaN, cont.WeightedAverage());
        }
    }
}