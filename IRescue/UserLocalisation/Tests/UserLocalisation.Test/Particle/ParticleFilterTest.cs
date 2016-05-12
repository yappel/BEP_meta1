// <copyright file="ParticleFilterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Particle.Algos;
using IRescue.UserLocalisation.Sensors;
using MathNet.Numerics;
using Moq;

namespace IRescue.UserLocalisation.Particle
{
    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class ParticleFilterTest
    {

        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestParticleFilterRun()
        {
            ParticleFilter filter = new ParticleFilter(new double[] { 500, 200, 500, 360, 360, 360 }, 30, 0.5);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>();
            returnlist.Add(new Measurement<Vector3>(new Vector3(250, 180, 250), 100, 0));
            possourcemock.Setup(foo => foo.GetPosition(It.IsAny<long>()))
                .Returns(new Measurement<Vector3>(new Vector3(250, 180, 250), 100, 0));
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filter.AddPositionSource(possourcemock.Object);

            for (int i = 0; i < 1000; i++)
            {
                Pose pose = filter.CalculatePose(i);
                System.Diagnostics.Debug.WriteLine(pose.Position.X);
            }

        }

        [Test]
        public void ParticleFilterUnits()
        {
            ParticleFilter filter = new ParticleFilter(new double[] { 500, 200, 500, 360, 360, 360 }, 30, 0.5);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>();
            returnlist.Add(new Measurement<Vector3>(new Vector3(250, 180, 250), 100, 0));
            possourcemock.Setup(foo => foo.GetPosition(It.IsAny<long>()))
                .Returns(new Measurement<Vector3>(new Vector3(250, 180, 250), 100, 0));
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filter.AddPositionSource(possourcemock.Object);


            Assert.AreEqual(30, filter.particles.RowCount);
            Resample.Multinomial(filter.particles, filter.weights);
            Assert.AreEqual(30, filter.particles.RowCount);
            Assert.AreEqual(6, filter.particles.ColumnCount);
            filter.AddMeasurements(1);
            Assert.AreEqual(30, filter.particles.RowCount);
            Feeder.AddWeights(20, filter.particles.SubMatrix(0, filter.particles.RowCount, 0, 3), filter.measurementspos,
                filter.weights);
            Assert.AreEqual(30, filter.particles.RowCount);
            Assert.AreEqual(6, filter.particles.ColumnCount);
            filter.normalizeWeightsAll(filter.weights);
            Assert.AreEqual(30, filter.particles.RowCount);
            Assert.AreEqual(6, filter.particles.ColumnCount);
        }

        [Test]
        public void ResampleMultinomalTest()
        {
            var res = InitParticles.RandomUniform(300, 6, new double[] { 500, 200, 500, 360, 360, 360 });
            Assert.AreEqual(300 * 6, res.Length);
        }


        public void NoiseTest()
        {
            Tuple<float, float>[] list = new Tuple<float, float>[30];
            for (int i = 0; i < 30; i++)
            {
                list[i] = new Tuple<float, float>(200, 1 / 30f);
            }
            float min = 500;
            float max = 0;
            foreach (Tuple<float, float> tuple in list)
            {
                if (tuple.Item1 < min)
                {
                    min = tuple.Item1;
                }
                if (tuple.Item1 > max)
                {
                    max = tuple.Item1;
                }
            }
            Assert.AreEqual(0, max - min);
            min = 500;
            max = 0;
            foreach (Tuple<float, float> tuple in list)
            {
                if (tuple.Item1 < min)
                {
                    min = tuple.Item1;
                }
                if (tuple.Item1 > max)
                {
                    max = tuple.Item1;
                }
            }
            foreach (var tuple in list)
            {
                System.Diagnostics.Debug.WriteLine(tuple.Item1);
            }
            Assert.AreNotEqual(0, max - min);
        }

        //[Test]
        public void writefile()
        {
            ParticleFilter filter = new ParticleFilter(new double[] { 500, 200, 500, 360, 360, 360 }, 30, 0.5);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>();
            returnlist.Add(new Measurement<Vector3>(new Vector3(400, 180, 250), 50, 0));
            possourcemock.Setup(foo => foo.GetPosition(It.IsAny<long>()))
                .Returns(new Measurement<Vector3>(new Vector3(250, 180, 250), 100, 0));
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filter.AddPositionSource(possourcemock.Object);

            var fullpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\DATA.txt");
            Pose pose = filter.CalculatePose(1);
            System.IO.File.WriteAllText(fullpath, pose.Position.X + " " + pose.Position.Z + " " + pose.Orientation.Y + System.Environment.NewLine);

            for (int j = 0; j < 60; j++)
            {
                fullpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\DATA.txt");
                pose = filter.CalculatePose(1);
                System.IO.File.AppendAllText(fullpath, pose.Position.X + " " + pose.Position.Z + " " + pose.Orientation.Y + System.Environment.NewLine);

            }
        }
    }
}


