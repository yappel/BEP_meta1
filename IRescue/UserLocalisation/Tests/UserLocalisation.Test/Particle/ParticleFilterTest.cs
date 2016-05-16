// <copyright file="ParticleFilterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
    using System.Collections.Generic;
    using System.Linq;
    using Algos.NoiseGenerators;
    using Algos.ParticleGenerators;
    using Algos.Resamplers;
    using Core.DataTypes;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;
    using Moq;
    using NUnit.Framework;
    using PosePrediction;
    using Sensors;

    /// <summary>
    /// Test for the particles
    /// </summary>
    public class ParticleFilterTest
    {
        /// <summary>
        /// Filter to use in tests.
        /// </summary>
        private ParticleFilter filter;

        /// <summary>
        /// Particle generator mock
        /// </summary>
        private Mock<IParticleGenerator> ptclgen;

        /// <summary>
        /// Noise generator mock
        /// </summary>
        private Mock<INoiseGenerator> noisegen;

        /// <summary>
        /// Pose predictor mock
        /// </summary>
        private Mock<IPosePredictor> posepredictor;

        /// <summary>
        /// Resample mock
        /// </summary>
        private Mock<IResampler> resampler;

        /// <summary>
        /// Size of the field
        /// </summary>
        private FieldSize fieldsize;

        /// <summary>
        /// Particle list
        /// </summary>
        private float[] particles;

        /// <summary>
        /// Initialization method
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.ptclgen = new Mock<IParticleGenerator>();
            this.noisegen = new Mock<INoiseGenerator>();
            this.posepredictor = new Mock<IPosePredictor>();
            this.resampler = new Mock<IResampler>();
            this.fieldsize = new FieldSize() { Xmax = 2, Xmin = 0, Ymax = 2, Ymin = 0, Zmax = 2, Zmin = 0 };
            var particleamount = 30;

            float[] particles = new float[particleamount * 6];
            for (int i = 0; i < particleamount * 6; i++)
            {
                particles[i] = 1;
            }

            this.particles = particles;

            this.ptclgen.Setup(foo => foo.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double[]>(), It.IsAny<double[]>())).Returns(particles);
            this.posepredictor.SetReturnsDefault(new float[] { 0, 0, 0, 0, 0, 0 });

            this.filter = new ParticleFilter(this.fieldsize, particleamount, 0.005f, 0.0f, this.ptclgen.Object, this.posepredictor.Object, this.noisegen.Object, this.resampler.Object);

            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>
            {
                new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 0.1f, 0)
            };
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            Mock<IOrientationSource> orisourcemock = new Mock<IOrientationSource>();
            List<Measurement<Vector3>> returnlist2 = new List<Measurement<Vector3>>
            {
                new Measurement<Vector3>(new Vector3(40f, 40f, 40f), 0.1f, 0)
            };
            orisourcemock.Setup(foo => foo.GetOrientations(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist2);
            this.filter.AddPositionSource(possourcemock.Object);
            this.filter.AddOrientationSource(orisourcemock.Object);
        }

        /// <summary>
        /// Test if normalizing the Weights in a matrix works correctly
        /// </summary>
        [Test]
        public void TestNormalizeWeights()
        {
            Matrix<float> weights = new DenseMatrix(2, 2, new float[] { 1, 1, 1, 1 });
            this.filter.NormalizeWeightsAll(weights);
            Assert.AreEqual(0.5, weights[0, 0]);
            Assert.AreEqual(0.5, weights[0, 1]);
            Assert.AreEqual(0.5, weights[1, 0]);
            Assert.AreEqual(0.5, weights[1, 1]);
        }

        /// <summary>
        /// Test if Particles get the right weight
        /// </summary>
        [Test]
        public void TestParticleWeighting()
        {
            int lpcount = 1;
            Matrix<float> localparts = new DenseMatrix(lpcount, 3, new float[] { 1, 1, 1 });
            Matrix<float> localweigh = new DenseMatrix(lpcount, 3, new float[] { 1, 1, 1 });
            Matrix<float> localmeas = new DenseMatrix(lpcount, 4, new float[] { 1, 1, 1, 0.1f });
            this.filter.AddWeights(0.01, localparts, 0, 3, localmeas, localweigh);
            Assert.AreEqual(0.0797f, localweigh[0, 0], 0.0001);
            ////normcdf(1.01,1,0.1)-normcdf(0.99,1,0.1) = 0.0797
        }

        /// <summary>
        /// Test if the weighted average gets calculated correctly.
        /// </summary>
        [Test]
        public void TestWeightedAverage()
        {
            Matrix<float> ptcls = new DenseMatrix(2, 2, new float[] { 1, 4, 2, 4 });
            Matrix<float> wgts = new DenseMatrix(2, 2, new float[] { 2 / 3f, 1 / 3f, 0.5f, 0.5f });
            float[] res = this.filter.WeightedAverage(ptcls, wgts);
            float[] expected = new float[] { 2, 3 };
            Assert.AreEqual(expected, res);
        }

        /// <summary>
        /// Test if the filter doesn't crash.
        /// </summary>
        [Test]
        public void TestParticleFilterRun()
        {
            this.filter.Particles[0, 0] = this.fieldsize.Xmax + 1000;
            this.filter.Particles[1, 0] = this.fieldsize.Xmin - 1000;
            this.filter.CalculatePose(0);
            float[] parts = this.filter.Particles.Column(0).ToArray();
            Assert.IsTrue(parts.Max() <= this.fieldsize.Xmax);
            Assert.IsTrue(parts.Min() >= this.fieldsize.Xmin);
        }

        /// <summary>
        /// Test if the filter doesn't crash when there are no sources.
        /// </summary>
        [Test]
        public void TestParticleFilterRunWithoutSources()
        {
            ParticleFilter filterr = new ParticleFilter(this.fieldsize, 30, 0.001, 0.01f, this.ptclgen.Object, this.posepredictor.Object, this.noisegen.Object, this.resampler.Object);
            this.ptclgen.Setup(foo => foo.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double[]>(), It.IsAny<double[]>())).Returns(this.particles);
            this.posepredictor.SetReturnsDefault(new float[] { 0, 0, 0, 0, 0, 0 });
            for (int i = 0; i < 1000; i++)
            {
                filterr.CalculatePose(i);
            }

            Assert.Pass();
        }

        /// <summary>
        /// Test if the amount of Particles is correct through the different steps.
        /// </summary>
        [Test]
        public void ParticleFilterUnits()
        {
            Assert.AreEqual(30, this.filter.Particles.RowCount);
            Assert.AreEqual(6, this.filter.Particles.ColumnCount);
            this.filter.RetrieveMeasurements(1);
            Assert.AreEqual(30, this.filter.Particles.RowCount);
            this.filter.AddWeights(
                20, this.filter.Particles, 0, 3, this.filter.Measurementspos, this.filter.Weights);
            Assert.AreEqual(30, this.filter.Particles.RowCount);
            Assert.AreEqual(6, this.filter.Particles.ColumnCount);
            this.filter.NormalizeWeightsAll(this.filter.Weights);
            Assert.AreEqual(30, this.filter.Particles.RowCount);
            Assert.AreEqual(6, this.filter.Particles.ColumnCount);
        }
    }
}
