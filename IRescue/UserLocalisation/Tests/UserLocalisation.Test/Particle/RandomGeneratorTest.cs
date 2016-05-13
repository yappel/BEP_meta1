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
            RandomGenerator rng = new RandomGenerator(new SystemRandomSource());
            double maxrange = 100;
            float[] list = rng.Generate(300, 1, new double[] { maxrange });
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
            RandomGenerator rng = new RandomGenerator(new SystemRandomSource());
            double maxrange = 100;
            int ptclamt = 300;
            float[] list = rng.Generate(ptclamt, 1, new double[] { maxrange });
            Array.Sort(list);
            float[] diffs = new float[list.Length - 1];
            for (int i = 0; i < list.Length - 1; i++)
            {
                diffs[i] = list[i + 1] - list[i];
            }

            Array.Sort(diffs);
            Assert.AreEqual(maxrange / (ptclamt + 1), diffs[0] - diffs[ptclamt - 2], maxrange / (ptclamt + 1) * 10);
            Assert.AreEqual(maxrange / (ptclamt + 1), diffs[(ptclamt - 2) / 2], maxrange / (ptclamt + 1));
        }
    }
}
