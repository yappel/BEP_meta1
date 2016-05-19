// <copyright file="ProgramTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
using IRescue.UserLocalisation.Particle.Algos.Resamplers;
using IRescue.UserLocalisation.PosePrediction;
using IRescue.UserLocalisation.Sensors;
using IRescue.UserLocalisationMeasuring.DataGeneration;
using IRescue.UserLocalisationMeasuring.DataProcessing;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using Moq;

namespace IRescue.UserLocalisation.Particle
{
    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class ProgramTest
    {
        private OrientationScenario oriscen2;
        private PositionScenario posscen2;
        private OrientationScenario oriscen1;
        private PositionScenario posscen1;

        [SetUp]
        public void Setup()
        {
            RandomSource random = new SystemRandomSource();
            this.oriscen2 = new OrientationScenario(
               (p => 360 * this.Sin(p, 30)), (p => 360 * this.Sin(p, 30)), (p => 360 * this.Sin(p, 30)), timestamps(0, 400), (() => (float)((random.NextDouble() * 6) - 3)), 1.5f);
            this.posscen2 = new PositionScenario(
                (p => 5f * this.Sin(p, 30)), (p => 2f * this.Sin(p, 30)), (p => 5f * this.Sin(p, 30)), timestamps(0, 400), (() => (float)(Normal.Sample(0, 0.025))), 0.025f);
            this.oriscen1 = new OrientationScenario(
                   (p => 360 * this.Sin(p, 30)), (p => 360 * this.Sin(p, 30)), (p => 360 * this.Sin(p, 30)), timestamps(0, 400), (() => (float)((random.NextDouble() * 6) - 3)), 1.5f);
            this.posscen1 = new PositionScenario(
                (p => 1f * this.Sin(p, 120)), (p => 1f * this.Sin(p, 120)), (p => 1f * this.Sin(p, 120)), timestamps(0, 400), (() => (float)(Normal.Sample(0, 0.025))), 0.025f);
        }

        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestMethod()
        {
            IRescue.UserLocalisationMeasuring.Program.CalcBestParticleSettings();
        }

        private float Sin16(long timestamp)
        {
            double x = Math.PI / 32;
            return Math.Abs((0.8f * (float)Math.Sin(x * timestamp)) + 0.1f);
        }

        private float Sin(long timestamp, double frames)
        {
            double x = Math.PI / (2 * frames);
            return Math.Abs((0.8f * (float)Math.Sin(x * timestamp)) + 0.1f);
        }

        [Test]
        public void TestSin16()
        {
            for (int i = 0; i < 120; i++)
            {
                Console.Write("{0} : ", i);
                Console.WriteLine(Sin16(i));
                Console.WriteLine(Sin(i, 120));
            }
            Assert.AreEqual(0.1f, this.Sin16(0), 0.001);
            Assert.AreEqual(0.9f, this.Sin16(16), 0.001);
            Assert.AreEqual(0.1f, this.Sin16(32), 0.001);
        }

        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestMethod2()
        {

            //PositionScenario posscen2 = new PositionScenario(
            //    (p => 0.8f), (p => 0.8f), (p => 0.8f), timestamps(0, 400), (() => 0f), 0.05f);

            int particleamount = 30;
            ParticleFilter filter1 = creatFilter(particleamount);
            filter1.AddOrientationSource(oriscen1);
            filter1.AddPositionSource(posscen1);

            //ParticleFilter filter2 = creatFilter(particleamount);
            //filter2.AddOrientationSource(oriscen2);
            //filter2.AddPositionSource(posscen2);
            //var analyser = new LocalizerAnalyser(1, 15, new List<AbstractUserLocalizer> { filter2 }, this.posscen2,
            //    this.oriscen2);
            //Console.WriteLine("Accuracy: {0} , AvRuntime:{1} , Precision:{2}", analyser.Accuracy, analyser.AverageRuntime, analyser.Precision);

            var resultpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\Filter_results.txt");
            var particlepath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\Filter_particles.txt");
            var actualpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\Filter_actual.txt");


            System.IO.File.WriteAllText(resultpath, "");
            System.IO.File.WriteAllText(particlepath, "");
            System.IO.File.WriteAllText(actualpath, "");
            StringBuilder builder1 = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            StringBuilder builder3 = new StringBuilder();


            Pose pose = null;
            for (int j = 1; j < 300; j++)
            {
                builder3.AppendFormat("{0} {1} {2} {3} {4} {5}", 1 * Sin(j, 120), 1 * Sin(j, 120), 1 * Sin(j, 120), 360 * Sin(j, 120), 360 * Sin(j, 120), 360 * Sin(j, 120));
                builder3.AppendLine();
                pose = filter1.CalculatePose(j);
                builder1.AppendFormat("{0} {1} {2} {3} {4} {5}", pose.Position.X, pose.Position.Y, pose.Position.Z, pose.Orientation.X, pose.Orientation.Y, pose.Orientation.Z);
                builder1.AppendLine();

                for (int i = 0; i < particleamount; i++)
                {
                    for (int jj = 0; jj < 6; jj++)
                    {
                        builder2.AppendFormat("{0} {1} ", filter1.Particles[i, jj], filter1.Weights[i, jj]);
                    }
                    builder2.AppendLine();
                }
            }
            System.IO.File.AppendAllText(resultpath, builder1.ToString());
            System.IO.File.AppendAllText(particlepath, builder2.ToString());
            System.IO.File.AppendAllText(actualpath, builder3.ToString());
        }

        private ParticleFilter creatFilter(int particleamount)
        {
            IParticleGenerator particlegen = new RandomParticleGenerator(new SystemRandomSource());
            INoiseGenerator noisegen = new RandomNoiseGenerator(new SystemRandomSource());
            IResampler resampler = new MultinomialResampler();
            //IResampler resampler = new SimpleResampler();
            IPosePredictor posepredictor = new LinearPosePredicter();
            ParticleFilter filter1 = new ParticleFilter(new FieldSize() { Xmax = 1, Xmin = 0, Ymax = 1, Ymin = 0, Zmax = 1, Zmin = 0 }, particleamount, 0.001, 0.2f, particlegen,
            posepredictor, noisegen, resampler);
            return filter1;
        }

        private long[] timestamps(int from, int to)
        {
            long[] sf = new long[to - from];
            for (int i = 0; i < to - from; i++)
            {
                sf[i] = from + i;
            }
            return sf;
        }
    }
}
