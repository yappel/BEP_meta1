﻿// <copyright file="ParticleFilterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace UserLocalisation.Test.Particle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using IRescue.Core.DataTypes;
    using IRescue.Core.Utils;
    using IRescue.UserLocalisation.Feedback;
    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;
    using IRescue.UserLocalisation.Sensors;

    using MathNet.Numerics.Distributions;

    using Moq;

    using NUnit.Framework;

    using Normal = IRescue.Core.Distributions.Normal;

    /// <summary>
    ///     Test for the particles
    /// </summary>
    public class ParticleFilterTest
    {
        private bool writetofile;

        /// <summary>
        /// Test if data generated with the Meta1 is resulting in the right results.
        /// </summary>
        [Test]
        public void TestRealMarkerSensorData()
        {
            List<Measurement<Vector3>> oridata = new List<Measurement<Vector3>>();
            List<Measurement<Vector3>> posdata = new List<Measurement<Vector3>>();
            using (StreamReader sr = new StreamReader(TestContext.CurrentContext.TestDirectory + @"\RealMarkerOriData_Expected_9_10_3.dat"))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] unparsed = line.Split(',');
                    oridata.Add(new Measurement<Vector3>(
                        VectorMath.Normalize(new Vector3(float.Parse(unparsed[0]), float.Parse(unparsed[1]), float.Parse(unparsed[2]))),
                        0,
                        new Normal(0.1)));
                    line = sr.ReadLine();
                }
            }

            using (StreamReader sr = new StreamReader(TestContext.CurrentContext.TestDirectory + @"\RealMarkerPosData_Expected_060_150_1.dat"))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] unparsed = line.Split(',');
                    posdata.Add(new Measurement<Vector3>(
                        new Vector3(float.Parse(unparsed[0]), float.Parse(unparsed[1]), float.Parse(unparsed[2])),
                        0,
                        new Normal(0.01)));
                    line = sr.ReadLine();
                }
            }

            FieldSize fieldsize = new FieldSize { Xmin = 0, Xmax = 4, Ymax = 2, Ymin = 0, Zmax = 4, Zmin = 0 };
            IParticleGenerator particleGenerator = new RandomParticleGenerator(new ContinuousUniform());
            IResampler resampler = new MultinomialResampler();
            INoiseGenerator noiseGenerator = new RandomNoiseGenerator(new ContinuousUniform());
            ISmoother smoother = new MovingAverageSmoother(200);
            ParticleFilter filter = new ParticleFilter(250, 0.1f, fieldsize, particleGenerator, resampler, noiseGenerator, smoother);

            int poscount = 0;
            Mock<IPositionSource> possource = new Mock<IPositionSource>();
            possource.Setup(foo => foo.GetPositionsClosestTo(It.IsAny<long>(), It.IsAny<long>())).
                Returns(() => new List<Measurement<Vector3>> { posdata[poscount] }).
                Callback(() => poscount++);
            filter.AddPositionSource(possource.Object);

            int oricount = 0;
            Mock<IOrientationSource> orisource = new Mock<IOrientationSource>();
            orisource.Setup(foo => foo.GetOrientationClosestTo(It.IsAny<long>(), It.IsAny<long>())).
                Returns(() => new List<Measurement<Vector3>> { oridata[oricount] }).
                Callback(() => oricount++);
            filter.AddOrientationSource(orisource.Object);

            this.writetofile = false;
            StringBuilder res = new StringBuilder();
            List<Pose> results = new List<Pose>();
            long i = 0;
            while ((poscount < posdata.Count / 10) && (oricount < oridata.Count / 10))
            {
                Pose pose = filter.CalculatePose(i);
                if (this.writetofile)
                {
                    res.AppendFormat($"{pose.Position.X},{pose.Position.Y},{pose.Position.Z},{pose.Orientation.X},{pose.Orientation.Y},{pose.Orientation.Z}" + Environment.NewLine);
                }

                i += 33;
                results.Add(pose);
            }

            if (this.writetofile)
            {
                File.WriteAllText(TestContext.CurrentContext.TestDirectory + @"\RealMarkerResults.dat", res.ToString());
            }

            int startindex = 30;
            Pose[] resultarray = new Pose[results.Count - startindex];
            results.CopyTo(startindex, resultarray, 0, resultarray.Length);

            Assert.AreEqual(posdata.GetRange(startindex, resultarray.Length).Select(m => m.Data.X).Average(), resultarray.Select(p => p.Position.X).Average(), 0.1);
            Assert.AreEqual(posdata.GetRange(startindex, resultarray.Length).Select(m => m.Data.Y).Average(), resultarray.Select(p => p.Position.Y).Average(), 0.1);
            Assert.AreEqual(posdata.GetRange(startindex, resultarray.Length).Select(m => m.Data.Z).Average(), resultarray.Select(p => p.Position.Z).Average(), 0.1);
            Assert.AreEqual(oridata.GetRange(startindex, resultarray.Length).Select(m => m.Data.X).Average(), resultarray.Select(p => p.Orientation.X).Average(), 0.1);
            Assert.AreEqual(oridata.GetRange(startindex, resultarray.Length).Select(m => m.Data.Y).Average(), resultarray.Select(p => p.Orientation.Y).Average(), 0.1);
            Assert.AreEqual(oridata.GetRange(startindex, resultarray.Length).Select(m => m.Data.Z).Average(), resultarray.Select(p => p.Orientation.Z).Average(), 0.1);
        }

        /// <summary>
        /// Test if adding displacement sources works.
        /// </summary>
        [Test]
        public void TestAddingDisplacementSource()
        {
            FieldSize fieldsize = new FieldSize { Xmin = 0, Xmax = 4, Ymax = 2, Ymin = 0, Zmax = 4, Zmin = 0 };
            IParticleGenerator particleGenerator = new RandomParticleGenerator(new ContinuousUniform());
            IResampler resampler = new MultinomialResampler();
            INoiseGenerator noiseGenerator = new RandomNoiseGenerator(new ContinuousUniform());
            ISmoother smoother = new MovingAverageSmoother(200);
            ParticleFilter filter = new ParticleFilter(250, 0.1f, fieldsize, particleGenerator, resampler, noiseGenerator, smoother);

            Mock<IDisplacementSource> sourcemock = new Mock<IDisplacementSource>();
            sourcemock.SetReturnsDefault<Measurement<Vector3>>(new Measurement<Vector3>(new Vector3(), 0, new Normal(0.1)));
            sourcemock.Setup(foo => foo.GetDisplacement(It.IsAny<int>(), It.IsAny<int>())).Returns(() => new Measurement<Vector3>(new Vector3(), 0, new Normal(0.1)));
            filter.AddDisplacementSource(sourcemock.Object);

            filter.CalculatePose(1);
            filter.CalculatePose(2);
            sourcemock.Verify(foo => foo.GetDisplacement(1, 2));
        }

        /// <summary>
        /// Test that filter can run without sources.
        /// </summary>
        [Test]
        public void TestNoCrashWhenNoData()
        {
            FieldSize fieldsize = new FieldSize { Xmin = 0, Xmax = 4, Ymax = 2, Ymin = 0, Zmax = 4, Zmin = 0 };
            IParticleGenerator particleGenerator = new RandomParticleGenerator(new ContinuousUniform());
            IResampler resampler = new MultinomialResampler();
            INoiseGenerator noiseGenerator = new RandomNoiseGenerator(new ContinuousUniform());
            ISmoother smoother = new MovingAverageSmoother(200);
            ParticleFilter filter = new ParticleFilter(250, 0.1f, fieldsize, particleGenerator, resampler, noiseGenerator, smoother);

            for (int i = 0; i < 10; i++)
            {
                filter.CalculatePose(i);
            }

            Assert.Pass();
        }

        /// <summary>
        /// Test if added feedback receivers receive feedback.
        /// </summary>
        [Test]
        public void TestAddingFeedbackReceivers()
        {
            FieldSize fieldsize = new FieldSize { Xmin = 0, Xmax = 4, Ymax = 2, Ymin = 0, Zmax = 4, Zmin = 0 };
            IParticleGenerator particleGenerator = new RandomParticleGenerator(new ContinuousUniform());
            IResampler resampler = new MultinomialResampler();
            INoiseGenerator noiseGenerator = new RandomNoiseGenerator(new ContinuousUniform());
            ISmoother smoother = new MovingAverageSmoother(200);
            ParticleFilter filter = new ParticleFilter(250, 0.1f, fieldsize, particleGenerator, resampler, noiseGenerator, smoother);
            Mock<IOrientationFeedbackReceiver> orifeed = new Mock<IOrientationFeedbackReceiver>();
            filter.RegisterReceiver(orifeed.Object);
            Mock<IPositionFeedbackReceiver> posfeed = new Mock<IPositionFeedbackReceiver>();
            filter.RegisterReceiver(posfeed.Object);
            filter.CalculatePose(10);
            orifeed.Verify(f => f.NotifyOrientationFeedback(It.IsAny<FeedbackData<Vector3>>()), Times.Once);
            posfeed.Verify(f => f.NotifyPositionFeedback(It.IsAny<FeedbackData<Vector3>>()), Times.Once);
        }

        /// <summary>
        /// Test if unregistered feedback receivers do not receive feedback.
        /// </summary>
        [Test]
        public void TestRemovingFeedbackReceivers()
        {
            FieldSize fieldsize = new FieldSize { Xmin = 0, Xmax = 4, Ymax = 2, Ymin = 0, Zmax = 4, Zmin = 0 };
            IParticleGenerator particleGenerator = new RandomParticleGenerator(new ContinuousUniform());
            IResampler resampler = new MultinomialResampler();
            INoiseGenerator noiseGenerator = new RandomNoiseGenerator(new ContinuousUniform());
            ISmoother smoother = new MovingAverageSmoother(200);
            ParticleFilter filter = new ParticleFilter(250, 0.1f, fieldsize, particleGenerator, resampler, noiseGenerator, smoother);
            Mock<IOrientationFeedbackReceiver> orifeed = new Mock<IOrientationFeedbackReceiver>();
            filter.RegisterReceiver(orifeed.Object);
            filter.RegisterReceiver(orifeed.Object);
            filter.UnregisterReceiver(orifeed.Object);
            filter.UnregisterReceiver(orifeed.Object);
            Mock<IPositionFeedbackReceiver> posfeed = new Mock<IPositionFeedbackReceiver>();
            filter.RegisterReceiver(posfeed.Object);
            filter.RegisterReceiver(posfeed.Object);
            filter.UnregisterReceiver(posfeed.Object);
            filter.UnregisterReceiver(posfeed.Object);
            filter.CalculatePose(10);
            posfeed.Verify(f => f.NotifyPositionFeedback(It.IsAny<FeedbackData<Vector3>>()), Times.Never);
            orifeed.Verify(f => f.NotifyOrientationFeedback(It.IsAny<FeedbackData<Vector3>>()), Times.Never);
        }
    }
}