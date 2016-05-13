// <copyright file="ParticleFilterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Particle.Algos;
using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
using IRescue.UserLocalisation.Sensors;
using IRescue.UserLocalisation.Sensors.Marker;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using Moq;

namespace IRescue.UserLocalisation.Particle
{
    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class ParticleFilterTest
    {
        private ParticleFilter filter;
        [SetUp]
        public void init()
        {
            filter = new ParticleFilter(new double[6] { 2, 2, 2, 360, 360, 360 }, 30, 0.005);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>
            {
                new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 1, 0)
            };
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
        }

        [Test]
        public void testNormalizeWeights()
        {
            Matrix<float> weights = new DenseMatrix(2, 2, new float[] { 1, 1, 1, 1 });
            this.filter.normalizeWeightsAll(weights);
            Assert.AreEqual(0.5, weights[0, 0]);
            Assert.AreEqual(0.5, weights[0, 1]);
            Assert.AreEqual(0.5, weights[1, 0]);
            Assert.AreEqual(0.5, weights[1, 1]);

        }

        [Test]
        public void testParticleWeighting()
        {
            int lpcount = 1;
            Matrix<float> localparts = new DenseMatrix(lpcount, 3, new float[] { 1, 1, 1 });
            Matrix<float> localweigh = new DenseMatrix(lpcount, 3, new float[] { 1, 1, 1 });
            Matrix<float> localmeas = new DenseMatrix(lpcount, 4, new float[] { 1, 1, 1, 0.1f });
            filter.AddWeights(0.01, localparts, localmeas, localweigh);
            Assert.AreEqual(0.0797f, localweigh[0, 0], 0.0001);
            //normcdf(1.01,1,0.1)-normcdf(0.99,1,0.1) = 0.0797
        }

        [Test]
        public void testMovingParticles()
        {

        }

        /// <summary>
        /// Test if the weighted average gets calculated correctly.
        /// </summary>
        [Test]
        public void testWeightedAverage()
        {
            Matrix<float> ptcls = new DenseMatrix(2, 2, new float[] { 1, 4, 2, 4 });
            Matrix<float> wgts = new DenseMatrix(2, 2, new float[] { 2 / 3f, 1 / 3f, 0.5f, 0.5f });
            float[] res = filter.WeightedAverage(ptcls, wgts);
            float[] expected = new float[] { 2, 3 };
            Assert.AreEqual(expected, res);
        }





        ///----------------------------------------------------------------Messy basement below-----------------------------------------------

        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestParticleFilterRun()
        {
            ParticleFilter filter = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>();
            returnlist.Add(new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 1, 0));
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filter.AddPositionSource(possourcemock.Object);
            Pose pose = null;
            for (int i = 0; i < 1000; i++)
            {
                pose = filter.CalculatePose(i);
                System.Diagnostics.Debug.WriteLine(pose.Position.X);
            }
            Assert.AreEqual(2.5, pose.Position.X, 0.5);
        }

        [Test]
        public void FullRunWithRawMarkerData()
        {
            const double epsilon = 1;
            MarkerSensor msens = new MarkerSensor(1, TestContext.CurrentContext.TestDirectory + "\\MarkerMapRealistic.xml");
            ParticleFilter filter = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005);

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(TestContext.CurrentContext.TestDirectory + "\\P1OR180.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                filter.AddOrientationSource(msens);
                filter.AddPositionSource(msens);
                Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
                Pose pose = null;
                string line;
                for (int i = 0; i < 10; i++)
                {
                    line = streamReader.ReadLine();
                    string[] strdata = line.Split(' ');
                    dic.Add(0,
                        new Pose(new Vector3(Convert.ToSingle(strdata[3]), Convert.ToSingle(strdata[4]), Convert.ToSingle(strdata[5])),
                            new Vector3(Convert.ToSingle(strdata[0]), Convert.ToSingle(strdata[1]), Convert.ToSingle(strdata[2]))));
                    msens.UpdateLocations(dic);
                    pose = filter.CalculatePose(i);
                    dic.Clear();
                    System.Console.WriteLine(msens.GetLastPosition().Data.X);
                    Assert.AreEqual(1, msens.GetLastPosition().Data.X, 1);
                }
                Assert.AreEqual(1, pose.Position.X, epsilon);
                Assert.AreEqual(1, pose.Position.Y, epsilon);
                Assert.AreEqual(1.8, pose.Position.Z, epsilon);
                Assert.AreEqual(0, pose.Orientation.X, 5);
                Assert.AreEqual(90, pose.Orientation.Y, 5);
                Assert.AreEqual(0, pose.Orientation.Z, 5);
            }
        }

        [Test]
        public void ParticleFilterUnits()
        {
            ParticleFilter filter = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>();
            returnlist.Add(new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 1, 0));
            possourcemock.Setup(foo => foo.GetPosition(It.IsAny<long>()))
                .Returns(new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 1, 0));
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filter.AddPositionSource(possourcemock.Object);


            Assert.AreEqual(30, filter.particles.RowCount);
            Resample.Multinomial(filter.particles, filter.weights);
            Assert.AreEqual(30, filter.particles.RowCount);
            Assert.AreEqual(6, filter.particles.ColumnCount);
            filter.RetrieveMeasurements(1);
            Assert.AreEqual(30, filter.particles.RowCount);
            filter.AddWeights(20, filter.particles.SubMatrix(0, filter.particles.RowCount, 0, 3), filter.measurementspos,
                filter.weights);
            Assert.AreEqual(30, filter.particles.RowCount);
            Assert.AreEqual(6, filter.particles.ColumnCount);
            filter.normalizeWeightsAll(filter.weights);
            Assert.AreEqual(30, filter.particles.RowCount);
            Assert.AreEqual(6, filter.particles.ColumnCount);
        }

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

        [Test]
        public void sladkfj()
        {
            ParticleFilter filterr = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>
            {
                new Measurement<Vector3>(new Vector3(4.0f, 1.8f, 2.5f), 1, 0)
            };
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            var fullpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\DATA.txt");
            System.IO.File.WriteAllText(fullpath, "0 0 0" + System.Environment.NewLine);
            for (int i = 0; i < 30; i++)
            {
                var weig = filterr.weights[i, 0] + filterr.weights[i, 2];
                System.IO.File.AppendAllText(fullpath,
                    filterr.particles[i, 0] + " " + filterr.particles[i, 2] + " " + weig + Environment.NewLine);
            }
            Pose pose = filterr.CalculatePose(0);
            System.IO.File.AppendAllText(fullpath, pose.Position.X + " " + pose.Position.Z + " " + pose.Orientation.Y + System.Environment.NewLine);
            for (int i = 0; i < 30; i++)
            {
                var weig = filterr.weights[i, 0] + filterr.weights[i, 2];
                System.IO.File.AppendAllText(fullpath,
                    filterr.particles[i, 0] + " " + filterr.particles[i, 2] + " " + weig + Environment.NewLine);
            }
        }

        [Test]
        public void writefile()
        {
            ParticleFilter filterr = new ParticleFilter(new double[] { 5, 2, 5, 360, 360, 360 }, 30, 0.005);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>
            {
                new Measurement<Vector3>(new Vector3(4.0f, 1.8f, 2.5f), 1, 0)
            };
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filterr.AddPositionSource(possourcemock.Object);

            var fullpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\DATA.txt");
            Pose pose = filterr.CalculatePose(1);
            System.IO.File.WriteAllText(fullpath, pose.Position.X + " " + pose.Position.Z + " " + pose.Orientation.Y + System.Environment.NewLine);
            for (int i = 0; i < 30; i++)
            {
                var weig = filterr.weights[i, 0] + filterr.weights[i, 2];
                System.IO.File.AppendAllText(fullpath,
                    filterr.particles[i, 0] + " " + filterr.particles[i, 2] + " " + weig + Environment.NewLine);
            }

            for (int j = 0; j < 60; j++)
            {
                fullpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\DATA.txt");
                pose = filterr.CalculatePose(j);
                System.IO.File.AppendAllText(fullpath, pose.Position.X + " " + pose.Position.Z + " " + pose.Orientation.Y + System.Environment.NewLine);
                for (int i = 0; i < 30; i++)
                {
                    var weig = filterr.weights[i, 0] + filterr.weights[i, 2];
                    System.IO.File.AppendAllText(fullpath,
                        filterr.particles[i, 0] + " " + filterr.particles[i, 2] + " " + weig + Environment.NewLine);
                }
            }
        }
    }
}


