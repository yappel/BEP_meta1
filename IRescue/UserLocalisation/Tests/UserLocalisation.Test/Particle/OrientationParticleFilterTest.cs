// <copyright file="OrientationParticleFilterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using IRescue.Core.DataTypes;
    using IRescue.Core.Utils;
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
    /// TODO TODO
    /// </summary>
    public class OrientationParticleFilterTest
    {
        private OrientationParticleFilter filter;

        private Mock<IOrientationSource> orisource;

        private Core.Distributions.IDistribution oridist;

        private MathNet.Numerics.Distributions.IContinuousDistribution orinoise;

        private Mock<IOrientationSource> orisource2;

        [SetUp]
        public void SetUp()
        {

            this.filter = new OrientationParticleFilter(
                new RandomNoiseGenerator(new ContinuousUniform()),
                0.1f,
                new MultinomialResampler(),
                new RandomParticleGenerator(new ContinuousUniform()),
                200,
                new MovingAverageSmoother(500));


            this.oridist = new Core.Distributions.Normal(3);
            this.orinoise = new MathNet.Numerics.Distributions.Normal(0, 3);
            this.orisource = new Mock<IOrientationSource>();
            this.orisource2 = new Mock<IOrientationSource>();
            this.orisource.Setup(foo => foo.GetOrientationClosestTo(It.IsAny<long>(), It.IsAny<long>())).Returns<long, long>((ts, range) => this.Ori(ts));
            this.orisource2.Setup(foo => foo.GetOrientationClosestTo(It.IsAny<long>(), It.IsAny<long>())).Returns<long, long>((ts, range) => this.Ori(ts));
            this.filter.AddOrientationSource(this.orisource.Object);
            this.filter.AddOrientationSource(this.orisource2.Object);
        }



        private List<Measurement<Vector3>> Ori(long ts)
        {
            return new List<Measurement<Vector3>>
                       {
                           new Measurement<Vector3>(
                               new Vector3(
                               (float) (this.OriX(ts) + this.orinoise.Sample()),
                               (float) (this.Oriy(ts) + this.orinoise.Sample()),
                               (float) (this.Oriz(ts) + this.orinoise.Sample())),
                               ts,
                               this.oridist)
                       };
        }

        private List<Measurement<Vector3>> Ori2(long ts)
        {
            return new List<Measurement<Vector3>>
                       {
                           new Measurement<Vector3>(
                               new Vector3(
                               (float) (0 + this.orinoise.Sample()),
                               (float) (0 + this.orinoise.Sample()),
                               (float) (0 + this.orinoise.Sample())),
                               ts,
                               this.oridist)
                       };
        }

        private double Oriz(long ts)
        {
            return 15 * Math.Sin(ts / 2000d);
        }

        private double Oriy(long ts)
        {
            return 45 * Math.Sin(ts / 2000d);
        }

        private double OriX(long ts)
        {
            return 30 * Math.Sin(ts / 2000d);
        }

        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestMethod()
        {
            List<float> diffx = new List<float>();
            List<float> diffy = new List<float>();
            List<float> diffz = new List<float>();

            for (int ts = 1; ts < 10001; ts += 33)
            {
                Vector3 res = this.filter.Calculate(ts);
                diffx.Add((float)AngleMath.SmallesAngle(res.X, (float)this.OriX(ts)));
                diffy.Add((float)AngleMath.SmallesAngle(res.Y, (float)this.Oriy(ts)));
                diffz.Add((float)AngleMath.SmallesAngle(res.Z, (float)this.Oriz(ts)));
            }



            File.WriteAllLines(TestContext.CurrentContext.TestDirectory + "OrientationX.dat", diffx.Select(d => d.ToString()).ToArray());
            File.WriteAllLines(TestContext.CurrentContext.TestDirectory + "OrientationY.dat", diffy.Select(d => d.ToString()).ToArray());
            File.WriteAllLines(TestContext.CurrentContext.TestDirectory + "OrientationZ.dat", diffz.Select(d => d.ToString()).ToArray());
            Assert.True(diffx.Max() < 5 * this.orinoise.Maximum);
            Assert.True(diffx.Min() > 5 * this.orinoise.Minimum);
            Assert.True(diffy.Max() < 5 * this.orinoise.Maximum);
            Assert.True(diffy.Min() > 5 * this.orinoise.Minimum);
            Assert.True(diffz.Max() < 5 * this.orinoise.Maximum);
            Assert.True(diffz.Min() > 5 * this.orinoise.Minimum);
        }

        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestMethod2()
        {
            this.orisource.Setup(foo => foo.GetOrientationClosestTo(It.IsAny<long>(), It.IsAny<long>())).Returns<long, long>((ts, range) => this.Ori2(ts));
            this.orisource2.Setup(foo => foo.GetOrientationClosestTo(It.IsAny<long>(), It.IsAny<long>())).Returns<long, long>((ts, range) => this.Ori2(ts));
            List<float> diffx = new List<float>();
            List<float> diffy = new List<float>();
            List<float> diffz = new List<float>();


            for (int ts = 1; ts < 10001; ts += 33)
            {
                Vector3 res = this.filter.Calculate(ts);
                diffx.Add((float)AngleMath.SmallesAngle(res.X, 0));
                diffy.Add((float)AngleMath.SmallesAngle(res.Y, 0));
                diffz.Add((float)AngleMath.SmallesAngle(res.Z, 0));
            }



            File.WriteAllLines(TestContext.CurrentContext.TestDirectory + "OrientationX2.dat", diffx.Select(d => d.ToString()).ToArray());
            File.WriteAllLines(TestContext.CurrentContext.TestDirectory + "OrientationY2.dat", diffy.Select(d => d.ToString()).ToArray());
            File.WriteAllLines(TestContext.CurrentContext.TestDirectory + "OrientationZ2.dat", diffz.Select(d => d.ToString()).ToArray());
            Assert.True(diffx.Max() < 5 * this.orinoise.Maximum);
            Assert.True(diffx.Min() > 5 * this.orinoise.Minimum);
            Assert.True(diffy.Max() < 5 * this.orinoise.Maximum);
            Assert.True(diffy.Min() > 5 * this.orinoise.Minimum);
            Assert.True(diffz.Max() < 5 * this.orinoise.Maximum);
            Assert.True(diffz.Min() > 5 * this.orinoise.Minimum);
        }
    }
}
