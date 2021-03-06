﻿// <copyright file="PositionParticleFilterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace UserLocalisation.Test.Particle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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

    using IDistribution = IRescue.Core.Distributions.IDistribution;
    using Normal = IRescue.Core.Distributions.Normal;

    /// <summary>
    /// Tests for the particle filter.
    /// </summary>
    public class PositionParticleFilterTest
    {
        private IDistribution disdist;

        private IContinuousDistribution disnoise;

        private Mock<IDisplacementSource> dissource;

        private PositionParticleFilter filter;

        private IDistribution posdist;

        private IContinuousDistribution posnoise;

        private Mock<IPositionSource> possource;

        /// <summary>
        /// Setup method.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            FieldSize fieldsize = new FieldSize
            {
                Xmax = 4,
                Ymax = 3,
                Xmin = 0,
                Ymin = 0,
                Zmax = 4,
                Zmin = -4
            };

            this.filter = new PositionParticleFilter(
                new RandomNoiseGenerator(new ContinuousUniform()),
                0.1f,
                new MultinomialResampler(),
                new RandomParticleGenerator(new ContinuousUniform()),
                200,
                fieldsize,
                new MovingAverageSmoother(500));

            this.posdist = new Normal(0.1);
            this.posnoise = new MathNet.Numerics.Distributions.Normal(0, 0.1);
            this.possource = new Mock<IPositionSource>();
            this.possource.Setup(foo => foo.GetPositionsClosestTo(It.IsAny<long>(), It.IsAny<long>())).Returns<long, long>((ts, range) => this.Pos(ts));
            this.filter.AddPositionSource(this.possource.Object);

            this.disdist = new Normal(0.1);
            this.disnoise = new MathNet.Numerics.Distributions.Normal(0, 0.1);
            this.dissource = new Mock<IDisplacementSource>();
            this.dissource.Setup(foo => foo.GetDisplacement(It.IsAny<long>(), It.IsAny<long>())).Returns<long, long>(this.Dis);
            this.filter.AddDisplacementSource(this.dissource.Object);
        }

        /// <summary>
        /// Test the filter when the user is moving.
        /// </summary>
        [Test]
        public void TestMoving()
        {
            List<double> diffx = new List<double>();
            List<double> diffy = new List<double>();
            List<double> diffz = new List<double>();

            for (int ts = 1; ts < 10001; ts += 33)
            {
                Vector3 res = this.filter.Calculate(ts);
                diffx.Add((float)(res.X - this.Posx(ts)));
                diffy.Add((float)(res.Y - this.Posy(ts)));
                diffz.Add((float)(res.Z - this.Posz(ts)));

                Vector3 meas = this.Dis(ts - 33, ts).Data;
                Console.WriteLine($"{meas.X},{meas.Y},{meas.Z}");
            }

            File.WriteAllLines(TestContext.CurrentContext.TestDirectory + "PositionX.dat", diffx.Select(d => d.ToString()).ToArray());
            File.WriteAllLines(TestContext.CurrentContext.TestDirectory + "PositionY.dat", diffy.Select(d => d.ToString()).ToArray());
            File.WriteAllLines(TestContext.CurrentContext.TestDirectory + "PositionZ.dat", diffz.Select(d => d.ToString()).ToArray());
            Assert.True(diffx.Max() < 5 * this.posnoise.Maximum);
            Assert.True(diffx.Min() > 5 * this.posnoise.Minimum);
            Assert.True(diffy.Max() < 5 * this.posnoise.Maximum);
            Assert.True(diffy.Min() > 5 * this.posnoise.Minimum);
            Assert.True(diffz.Max() < 5 * this.posnoise.Maximum);
            Assert.True(diffz.Min() > 5 * this.posnoise.Minimum);
        }

        /// <summary>
        /// Test the filter when the user is standing still.
        /// </summary>
        [Test]
        public void TestNoMove()
        {
            this.possource.Setup(foo => foo.GetPositionsClosestTo(It.IsAny<long>(), It.IsAny<long>())).Returns<long, long>((ts, range) => this.Pos2(ts));
            this.dissource.Setup(foo => foo.GetDisplacement(It.IsAny<long>(), It.IsAny<long>())).Returns<long, long>(this.Dis2);

            List<double> diffx = new List<double>();
            List<double> diffy = new List<double>();
            List<double> diffz = new List<double>();

            for (int ts = 1; ts < 10001; ts += 33)
            {
                Vector3 res = this.filter.Calculate(ts);
                diffx.Add(res.X - 1);
                diffy.Add(res.Y - 1);
                diffz.Add(res.Z - 1);
            }

            Assert.True(diffx.Max() < 5 * this.posnoise.Maximum);
            Assert.True(diffx.Min() > 5 * this.posnoise.Minimum);
            Assert.True(diffy.Max() < 5 * this.posnoise.Maximum);
            Assert.True(diffy.Min() > 5 * this.posnoise.Minimum);
            Assert.True(diffz.Max() < 5 * this.posnoise.Maximum);
            Assert.True(diffz.Min() > 5 * this.posnoise.Minimum);
        }

        private Measurement<Vector3> Dis(long tsfrom, long tsto)
        {
            return
                new Measurement<Vector3>(
                    new Vector3(
                        (float)((this.Posx(tsto) - this.Posx(tsfrom)) + this.disnoise.Sample()),
                        (float)((this.Posy(tsto) - this.Posy(tsfrom)) + this.disnoise.Sample()),
                        (float)((this.Posz(tsto) - this.Posz(tsfrom)) + this.disnoise.Sample())),
                    tsto,
                    this.disdist);
        }

        private Measurement<Vector3> Dis2(long arg1, long arg2)
        {
            return new Measurement<Vector3>(new Vector3((float)this.disnoise.Sample(), (float)this.disnoise.Sample(), (float)this.disnoise.Sample()), arg2, this.disdist);
        }

        private List<Measurement<Vector3>> Pos(long ts)
        {
            return new List<Measurement<Vector3>>
                       {
                           new Measurement<Vector3>(
                               new Vector3(
                               (float)(this.Posx(ts) + this.posnoise.Sample()),
                               (float)(this.Posy(ts) + this.posnoise.Sample()),
                               (float)(this.Posz(ts) + this.posnoise.Sample())),
                               ts,
                               this.posdist)
                       };
        }

        private List<Measurement<Vector3>> Pos2(long ts)
        {
            return new List<Measurement<Vector3>>
                       {
                           new Measurement<Vector3>(new Vector3((float)(1 + this.posnoise.Sample()), (float)(1 + this.posnoise.Sample()), (float)(1 + this.posnoise.Sample())), ts, this.posdist)
                       };
        }

        private double Posx(long ts)
        {
            return Math.Sin((Math.PI * ts) / 10000d) + 2;
        }

        private double Posy(long ts)
        {
            return (0.1 * Math.Sin((Math.PI * ts) / 1000d)) + 1.8f;
        }

        private double Posz(long ts)
        {
            return (float)(3 * Math.Cos((Math.PI * ts) / 10000d));
        }
    }
}