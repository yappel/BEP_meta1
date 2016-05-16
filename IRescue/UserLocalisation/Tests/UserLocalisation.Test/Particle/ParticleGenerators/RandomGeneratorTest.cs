// <copyright file="RandomGeneratorTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Linq;
    using Algos.ParticleGenerators;
    using MathNet.Numerics.Random;
    using NUnit.Framework;

    /// <summary>
    /// Tests for the random particle generator class
    /// </summary>
    public class RandomGeneratorTest
    {
        /// <summary>
        /// Test if the range of the generated Particles is at least 90% of the maximum range.
        /// </summary>
        [Test]
        public void TestSpread()
        {
            RandomParticleGenerator rng = new RandomParticleGenerator(new SystemRandomSource());
            double maxrange = 100;
            float[] list = rng.Generate(300, 1, new double[] { 0 }, new double[] { maxrange });
            float min = list.Concat(new[] { float.MaxValue }).Min();
            float max = list.Concat(new[] { float.MinValue }).Max();
            Assert.AreEqual(maxrange, max - min, 0.1 * maxrange);
        }

        /// <summary>
        /// Test if  the generated Particles are equally spread.
        /// </summary>
        [Test]
        public void TestDistribution()
        {
            RandomParticleGenerator rng = new RandomParticleGenerator(new SystemRandomSource());
            double maxrange = 100;
            int ptclamt = 300;
            float[] list = rng.Generate(ptclamt, 1, new double[] { 0 }, new double[] { maxrange });
            Array.Sort(list);
            float[] diffs = new float[list.Length - 1];
            for (int i = 0; i < list.Length - 1; i++)
            {
                diffs[i] = list[i + 1] - list[i];
            }

            float averagediff = diffs.Average();
            float expected = (float)(maxrange / (ptclamt + 1));
            Assert.AreEqual(expected, averagediff, expected);
        }

        /// <summary>
        /// Test if the length of the output is correct.
        /// </summary>
        [Test]
        public void TestOutputLength()
        {
            RandomParticleGenerator rng = new RandomParticleGenerator(new SystemRandomSource());
            int particleamount = 30;
            int dimension = 6;
            int expected = particleamount * dimension;
            float[] list = rng.Generate(
                particleamount,
                dimension,
                new double[] { 0, 0, 0, 0, 0, 0 },
                new double[] { 1, 1, 1, 1, 1, 1 });
            Assert.AreEqual(expected, list.Length);
        }

        /// <summary>
        /// Test if exception is thrown when wrong input length is given
        /// </summary>
        [Test]
        public void TestWrongArrayLength1()
        {
            RandomParticleGenerator rng = new RandomParticleGenerator(new SystemRandomSource());
            Assert.Catch<ArgumentException>(() => rng.Generate(1, 2, new double[] { 0 }, new double[] { 0, 0 }));
            Assert.Catch<ArgumentException>(() => rng.Generate(1, 2, new double[] { 0, 0 }, new double[] { 0 }));
        }
    }
}
