// <copyright file="ParticleFilterTest.cs" company="Delft University of Technology">
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
                // Read the stream to a string, and write the string to the console.
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] unparsed = line.Split(',');
                    oridata.Add(new Measurement<Vector3>(
                        new Vector3(float.Parse(unparsed[0]), float.Parse(unparsed[1]), float.Parse(unparsed[2])),
                        0,
                        new Normal(0.1)));
                    line = sr.ReadLine();
                }
            }

            using (StreamReader sr = new StreamReader(TestContext.CurrentContext.TestDirectory + @"\RealMarkerPosData_Expected_060_150_1.dat"))
            {
                // Read the stream to a string, and write the string to the console.
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

            StringBuilder res = new StringBuilder();
            List<Pose> results = new List<Pose>();
            long i = 0;
            while ((i < 30000) && (poscount < posdata.Count) && (oricount < oridata.Count))
            {
                Pose pose = filter.CalculatePose(i);
                res.AppendFormat($"{pose.Position.X},{pose.Position.Y},{pose.Position.Z},{pose.Orientation.X},{pose.Orientation.Y},{pose.Orientation.Z}" + Environment.NewLine);
                i += 33;
                results.Add(pose);
            }

            File.WriteAllText(TestContext.CurrentContext.TestDirectory + @"\RealMarkerResults.dat", res.ToString());
            Assert.AreEqual(posdata.Select(m => m.Data.X).Average(), results.ToArray().Select(p => p.Position.X).Average(), 0.1);
            Assert.AreEqual(posdata.Select(m => m.Data.Y).Average(), results.ToArray().Select(p => p.Position.Y).Average(), 0.1);
            Assert.AreEqual(posdata.Select(m => m.Data.Z).Average(), results.ToArray().Select(p => p.Position.Z).Average(), 0.1);
            Assert.AreEqual(oridata.Select(m => m.Data.X).Average(), results.ToArray().Select(p => p.Orientation.X).Average(), 0.1);
            Assert.AreEqual(oridata.Select(m => m.Data.Y).Average(), results.ToArray().Select(p => p.Orientation.Y).Average(), 0.1);
            Assert.AreEqual(oridata.Select(m => m.Data.Z).Average(), results.ToArray().Select(p => p.Orientation.Z).Average(), 0.1);
        }
    }
}