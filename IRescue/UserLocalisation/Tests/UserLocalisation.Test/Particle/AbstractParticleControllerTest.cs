using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserLocalisation.Test.Particle
{
    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;

    using Moq;

    using NUnit.Framework;

    class AbstractParticleControllerTest
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
        /// Test adding values.
        /// </summary>
        [Test]
        public void TestAddingValues()
        {
            float[] begin = Enumerable.Repeat(0f, this.particleAmount).ToArray();
            Assert.AreEqual(begin, this.controller.Values);
            float[] expected = Enumerable.Repeat(0.5f, this.particleAmount).ToArray();
            this.controller.AddToValues(expected);
            Assert.AreEqual(expected, this.controller.Values);
            this.controller.AddToValues(expected);
            expected = Enumerable.Repeat(1f, this.particleAmount).ToArray();
            Assert.AreEqual(expected, this.controller.Values);
            this.controller.AddToValues(expected);
            expected = Enumerable.Repeat(1f, this.particleAmount).ToArray();
            Assert.AreEqual(expected, this.controller.Values);
            this.controller.AddToValues(Enumerable.Repeat(-1.5f, this.particleAmount).ToArray());
            expected = Enumerable.Repeat(0f, this.particleAmount).ToArray();
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
        /// Test the if count is equal to particleamount.
        /// </summary>
        [Test]
        public void TestCount()
        {
            Assert.AreEqual(this.particleAmount, this.controller.Count);
        }

        /// <summary>
        /// Test getting value.
        /// </summary>
        [Test]
        public void TestGettingValue()
        {
            float[] init = this.particleGenerator.Object.Generate(this.particleAmount, this.minValue, this.maxValue);
            for (int i = 0; i < init.Length; i++)
            {
                Assert.AreEqual(init[i], this.controller.GetValueAt(i));
            }
        }

        /// <summary>
        /// Test adding invalid values to set value.
        /// </summary>
        [Test]
        public void TestSettingInvalidValue()
        {
            float toset = this.maxValue + 1;
            Assert.Throws<ArgumentOutOfRangeException>(() => this.controller.SetValueAt(0, toset));
            Assert.Throws<ArgumentOutOfRangeException>(() => this.controller.SetValueAt(this.particleAmount + 1, this.maxValue));
        }

        /// <summary>
        /// Test setting a value.
        /// </summary>
        [Test]
        public void TestSettingValue()
        {
            float toset = this.maxValue / 2;
            int index = 0;
            Assert.AreNotEqual(toset, this.controller.GetValueAt(index));
            this.controller.SetValueAt(index, toset);
            Assert.AreEqual(toset, this.controller.GetValueAt(index));
        }

        /// <summary>
        /// Test setting values.
        /// </summary>
        [Test]
        public void TestSettingValues()
        {
            float[] init = this.particleGenerator.Object.Generate(this.particleAmount, this.minValue, this.maxValue);
            init = init.Select(f => f + 0.1f).ToArray();
            this.controller.Values = init;
            Assert.AreEqual(init, this.controller.Values);
        }

        /// <summary>
        /// Test adding wrong length of array of values.
        /// </summary>
        [Test]
        public void TestSettingValuesWrongLength()
        {
            Assert.Throws<ArgumentException>(() => this.controller.Values = Enumerable.Repeat(this.minValue, this.particleAmount - 1).ToArray());
        }

        /// <summary>
        /// Test when adding values outside minmax range.
        /// </summary>
        [Test]
        public void TestSettingValuesWrongValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => this.controller.Values = Enumerable.Repeat(this.maxValue + 1, this.particleAmount).ToArray());
        }

        /// <summary>
        /// Test if the inital values are correct.
        /// </summary>
        [Test]
        public void TestValues()
        {
            Assert.AreEqual(this.particleGenerator.Object.Generate(this.particleAmount, this.minValue, this.maxValue), this.controller.Values);
        }

        [Test]
        public void TestGettingWeightAtIndex()
        {
            Assert.AreEqual(1f / this.particleAmount, this.controller.GetWeightAt(0));
            float[] newweights = new float[this.controller.Count];
            for (int i = 0; i < this.controller.Count; i++)
            {
                newweights[i] = i;
            }

            this.controller.Weights = newweights;
            Assert.AreEqual(this.particleAmount - 1, this.controller.GetWeightAt(this.particleAmount - 1));
        }

        [Test]
        public void TestSettingWeightAtIndex()
        {
            Assert.AreEqual(1f / this.particleAmount, this.controller.GetWeightAt(0));

            this.controller.SetWeightAt(0, this.particleAmount);
            Assert.AreEqual(this.particleAmount, this.controller.GetWeightAt(0));
        }

        [Test]
        public void TestMultiplyWeights()
        {
            Assert.AreEqual(1f / this.particleAmount, this.controller.GetWeightAt(0));
            this.controller.MultiplyWeightAt(0, 2);
            Assert.AreEqual(2f / this.particleAmount, this.controller.GetWeightAt(0));
            this.controller.MultiplyWeightAt(0, 0);
            Assert.AreEqual(0f, this.controller.GetWeightAt(0));
        }

        [Test]
        public void TestNormalizingWeights()
        {
            this.controller.Weights = Enumerable.Repeat(10f, this.particleAmount).ToArray();
            this.controller.NormalizeWeights();
            Assert.AreEqual(1, this.controller.Weights.Sum(), float.Epsilon);
        }

        [Test]
        public void TestNormalizingWeightsNotPossible()
        {
            this.controller.Weights = Enumerable.Repeat(0f, this.particleAmount).ToArray();
            Assert.Throws<DivideByZeroException>(() => this.controller.NormalizeWeights());
        }
    }
}
