// <copyright file="MultinomialResamplerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Linq;
    using Algos.Resamplers;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;
    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class MultinomialResamplerTest
    {
        /// <summary>
        /// The particles.
        /// </summary>
        private Matrix<float> particles;

        /// <summary>
        /// The weights.
        /// </summary>
        private Matrix<float> weights;

        /// <summary>
        /// A test subject
        /// </summary>
        private MultinomialResampler mr;

        /// <summary>
        /// Setup method
        /// </summary>
        [SetUp]
        public void Setup()
        {
            int rowcount = 200;
            this.weights = new DenseMatrix(rowcount, 1);
            this.mr = new MultinomialResampler();
            float[] parts = new float[rowcount];
            for (int i = 0; i < rowcount; i++)
            {
                parts[i] = i;
                this.weights[i, 0] = 1 / (float)rowcount;
            }

            this.particles = new DenseMatrix(rowcount, 1, parts);
        }

        /// <summary>
        /// Test if particles with zero weight are not chosen.
        /// </summary>
        [Test]
        public void TestZerosNotChosen()
        {
            this.weights.Clear();
            this.weights[1, 0] = 1;
            this.mr.Resample(this.particles, this.weights);
            Assert.AreEqual(this.particles.RowCount, this.particles.Column(0).ToArray().Sum());
        }

        /// <summary>
        /// Test if particles with zero weight are not chosen.
        /// </summary>
        [Test]
        public void TestRightSpread()
        {
            this.mr.Resample(this.particles, this.weights);
            var diffs = new float[this.particles.RowCount - 1];
            var particlearray = this.particles.Column(0).ToArray();
            Array.Sort(particlearray);
            for (int i = 0; i < this.particles.RowCount - 1; i++)
            {
                diffs[i] = particlearray[i + 1] - particlearray[i];
            }

            Assert.AreEqual(1, diffs.Average(), 1);
        }
    }
}
