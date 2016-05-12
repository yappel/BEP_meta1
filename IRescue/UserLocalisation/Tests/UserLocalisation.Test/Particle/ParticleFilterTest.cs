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
            ParticleFilter filter = new ParticleFilter(500, 200, 360, 30, 0.5);
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
            ParticleFilter filter = new ParticleFilter(500, 200, 360, 30, 0.5);
            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>>();
            returnlist.Add(new Measurement<Vector3>(new Vector3(250, 180, 250), 100, 0));
            possourcemock.Setup(foo => foo.GetPosition(It.IsAny<long>()))
                .Returns(new Measurement<Vector3>(new Vector3(250, 180, 250), 100, 0));
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>()))
               .Returns(returnlist);
            filter.AddPositionSource(possourcemock.Object);


            Assert.AreEqual(30, filter.listx.Length);
            Resample.MultinomialAll(filter.listx, filter.listy, filter.listz, filter.listp, filter.listyy, filter.listr);
            Assert.AreEqual(30, filter.listx.Length);
            filter.AddMeasurements(1);
            Assert.AreEqual(30, filter.listx.Length);
            Feeder.AddWeights(20, filter.listx, filter.measx);
            Feeder.AddWeights(20, filter.listy, filter.measy);
            Feeder.AddWeights(20, filter.listz, filter.measz);
            Feeder.AddWeights(20, filter.listp, filter.measp);
            Feeder.AddWeights(20, filter.listyy, filter.measyy);
            Feeder.AddWeights(20, filter.listr, filter.measr);
            Assert.AreEqual(30, filter.listx.Length);
            filter.normalizeWeightsAll(filter.listx, filter.listy, filter.listz, filter.listp, filter.listyy, filter.listr);
            Assert.AreEqual(30, filter.listx.Length);
        }

        [Test]
        public void ResampleMultinomalTest()
        {
            Tuple<float, float>[] list = new Tuple<float, float>[30];
            InitParticles.Random(30, 500, list);
            Assert.AreEqual(30, list.Length);
            Resample.MultinomialAll(list);
            Assert.AreEqual(30, list.Length);
        }

        [Test]
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
            NoiseGenerator.Uniform(100, list);
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
            ParticleFilter filter = new ParticleFilter(500, 200, 360, 30, 0.5);
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
            for (int i = 0; i < filter.listx.Length; i++)
            {
                System.IO.File.AppendAllText(fullpath, filter.listx[i].Item1 + " " + filter.listz[i].Item1 + " " + 0.5 * (filter.listx[i].Item2 + filter.listz[i].Item2) + System.Environment.NewLine);
            }

            for (int j = 0; j < 60; j++)
            {
                fullpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\DATA.txt");
                pose = filter.CalculatePose(1);
                System.IO.File.AppendAllText(fullpath, pose.Position.X + " " + pose.Position.Z + " " + pose.Orientation.Y + System.Environment.NewLine);
                for (int i = 0; i < filter.listx.Length; i++)
                {
                    System.IO.File.AppendAllText(fullpath, filter.listx[i].Item1 + " " + filter.listz[i].Item1 + " " + 0.5 * (filter.listx[i].Item2 + filter.listz[i].Item2) + System.Environment.NewLine);
                }

            }
        }
    }
}


