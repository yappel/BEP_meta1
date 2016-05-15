// <copyright file="ParticleFilterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
    using System.Collections.Generic;
    using Algos;
    using Algos.ParticleGenerators;
    using Core.DataTypes;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;
    using MathNet.Numerics.Random;
    using Moq;
    using NUnit.Framework;
    using Sensors;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class ParticleFilterTest
    {
        /// <summary>
        /// Filter to use in tests.
        /// </summary>
        private ParticleFilter filter;

        /// <summary>
        /// Initialization method
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.filter = new ParticleFilter(new double[6] { 2, 2, 2, 360, 360, 360 }, 30, 0.005, 0.1, new RandomGenerator(new SystemRandomSource()));
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>
            {
                new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 1, 0)
            };
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
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
        /// Test the average accuracy of the filter is within an acceptable range.
        /// </summary>
        [Test]
        public void TestParticleFilterRun()
        {
            ParticleFilter filter = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005, 0.1, new RandomGenerator(new SystemRandomSource()));
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>();
            returnlist.Add(new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 1, 0));
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filter.AddPositionSource(possourcemock.Object);
            Pose pose = null;
            float x = 0;
            for (int i = 0; i < 1000; i++)
            {
                pose = filter.CalculatePose(i);
                System.Diagnostics.Debug.WriteLine(pose.Position.X);
                if (i % 50 == 0)
                {
                    x += pose.Position.X;
                }
            }

            Assert.AreEqual(2.5, x / 20, 0.5);
        }

        /// <summary>
        /// Test if the amount of Particles is correct through the different steps.
        /// </summary>
        [Test]
        public void ParticleFilterUnits()
        {
            ParticleFilter filter = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005, 0.1, new RandomGenerator(new SystemRandomSource()));
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>();
            returnlist.Add(new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 1, 0));
            possourcemock.Setup(foo => foo.GetPosition(It.IsAny<long>()))
                .Returns(new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 1, 0));
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filter.AddPositionSource(possourcemock.Object);

            Assert.AreEqual(30, filter.Particles.RowCount);
            Resample.Multinomial(filter.Particles, filter.Weights);
            Assert.AreEqual(30, filter.Particles.RowCount);
            Assert.AreEqual(6, filter.Particles.ColumnCount);
            filter.RetrieveMeasurements(1);
            Assert.AreEqual(30, filter.Particles.RowCount);
            filter.AddWeights(
                20, filter.Particles, 0, 3, filter.Measurementspos, filter.Weights);
            Assert.AreEqual(30, filter.Particles.RowCount);
            Assert.AreEqual(6, filter.Particles.ColumnCount);
            filter.NormalizeWeightsAll(filter.Weights);
            Assert.AreEqual(30, filter.Particles.RowCount);
            Assert.AreEqual(6, filter.Particles.ColumnCount);
        }

        /// <summary>
        /// Test if adding noise actually increases the range of values of the Particles.
        /// </summary>
        [Test]
        public void NoiseTest()
        {
            float[] list = new float[30];
            for (int i = 0; i < 30; i++)
            {
                list[i] = 1 / 30f;
            }

            float min = 500;
            float max = 0;
            foreach (float f in list)
            {
                if (f < min)
                {
                    min = f;
                }

                if (f > max)
                {
                    max = f;
                }
            }

            Assert.AreEqual(0, max - min);
            Matrix<float> locparts = new DenseMatrix(1, 30, list);
            NoiseGenerator.Uniform(locparts, 1);
            min = 500;
            max = 0;
            foreach (float f in locparts.ToArray())
            {
                if (f < min)
                {
                    min = f;
                }

                if (f > max)
                {
                    max = f;
                }
            }

            Assert.AreNotEqual(0, max - min);
        }
    }
}
