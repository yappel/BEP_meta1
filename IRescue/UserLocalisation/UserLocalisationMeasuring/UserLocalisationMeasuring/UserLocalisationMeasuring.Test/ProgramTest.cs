// <copyright file="ProgramTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.PosePrediction;
    using IRescue.UserLocalisationMeasuring;
    using IRescue.UserLocalisationMeasuring.DataGeneration;
    using IRescue.UserLocalisationMeasuring.DataProcessing;

    using MathNet.Numerics.Distributions;
    using MathNet.Numerics.Random;

    using NUnit.Framework;

    /// <summary>
    /// TODO TODO
    /// </summary>
    public class ProgramTest
    {
        private OrientationScenario oriscen1;

        private OrientationScenario oriscen2;

        private PositionScenario posscen1;

        private PositionScenario posscen2;

        private FieldSize fieldsize;

        [SetUp]
        public void Setup()
        {
            this.fieldsize = new FieldSize { Xmin = 0, Xmax = 10, Ymin = 0, Ymax = 2, Zmin = 0, Zmax = 10 };
            RandomSource random = new SystemRandomSource();
            ////"Realistic" movements
            this.oriscen2 = new OrientationScenario(
                p => this.Mod((float)((60 * Math.Sin((Math.PI / 16) * p)) + 360), 360),
                p => this.Mod((float)((90 * Math.Sin((Math.PI / 16) * p)) + 360), 360),
                p => this.Mod((float)((30 * Math.Sin((Math.PI / 16) * p)) + 360), 360),
                this.timestamps(0, 400),
                () => (float)ContinuousUniform.Sample(-3, 3),
                1.5f);
            this.posscen2 = new PositionScenario(
                p => this.Scene2PosXy(p, 60),
                p => (float)((0.2f * Math.Sin((Math.PI / 25) * p)) + 1.8f),
                p => this.Scene2PosXy(p, 90),
                this.timestamps(0, 400),
                () => (float)Normal.Sample(0, 0.025),
                0.025f);

            ////No movement
            this.oriscen1 = new OrientationScenario(
                p => 0,
                p => 0,
                p => 0,
                this.timestamps(0, 400),
                () => (float)ContinuousUniform.Sample(-3, 3),
                1.5f);
            this.posscen1 = new PositionScenario(
                p => (float)(0.5 * (this.fieldsize.Xmax - this.fieldsize.Xmin)),
                p => (float)(0.5 * (this.fieldsize.Ymax - this.fieldsize.Ymin)),
                p => (float)(0.5 * (this.fieldsize.Zmax - this.fieldsize.Zmin)),
                this.timestamps(0, 400),
                () => (float)Normal.Sample(0, 0.025),
                0.025f);
        }

        /// Perform the modulo operation returning a value between 0 and b-1.
        /// </summary>
        /// <param name="a">The number to perform modulo on.</param>
        /// <param name="b">The number to divide by.</param>
        /// <returns>The modulo result.</returns>
        private float Mod(float a, float b)
        {
            return ((a % b) + b) % b;
        }

        private float Scene2PosXy(long x, float ticks)
        {
            float margin = 0.2f;
            float xsize = ((this.fieldsize.Xmax - this.fieldsize.Xmin) / 2) - (2 * margin);
            return (float)((xsize * Math.Sin(Math.PI / ticks)) + margin + xsize);
        }

        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestMethod2()
        {
            // PositionScenario posscen2 = new PositionScenario(
            // (p => 0.8f), (p => 0.8f), (p => 0.8f), timestamps(0, 400), (() => 0f), 0.05f);
            int particleamount = 30;
            ParticleFilter filter1 = this.creatFilter(particleamount);
            filter1.AddOrientationSource(this.oriscen1);
            filter1.AddPositionSource(this.posscen1);

            // ParticleFilter filter2 = creatFilter(particleamount);
            // filter2.AddOrientationSource(oriscen2);
            // filter2.AddPositionSource(posscen2);
            // var analyser = new LocalizerAnalyser(1, 15, new List<AbstractUserLocalizer> { filter2 }, this.posscen2,
            // this.oriscen2);
            // Console.WriteLine("Accuracy: {0} , AvRuntime:{1} , Precision:{2}", analyser.Accuracy, analyser.AverageRuntime, analyser.Precision);
            var resultpath = Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\Filter_results.txt");
            var particlepath = Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\Filter_particles.txt");
            var actualpath = Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\Filter_actual.txt");

            File.WriteAllText(resultpath, string.Empty);
            File.WriteAllText(particlepath, string.Empty);
            File.WriteAllText(actualpath, string.Empty);
            StringBuilder builder1 = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            StringBuilder builder3 = new StringBuilder();

            Pose pose = null;
            for (int j = 1; j < 300; j++)
            {
                builder3.AppendFormat(
                    "{0} {1} {2} {3} {4} {5}",
                    1 * this.Sin(j, 120),
                    1 * this.Sin(j, 120),
                    1 * this.Sin(j, 120),
                    360 * this.Sin(j, 120),
                    360 * this.Sin(j, 120),
                    360 * this.Sin(j, 120));
                builder3.AppendLine();
                pose = filter1.CalculatePose(j);
                builder1.AppendFormat(
                    "{0} {1} {2} {3} {4} {5}",
                    pose.Position.X,
                    pose.Position.Y,
                    pose.Position.Z,
                    pose.Orientation.X,
                    pose.Orientation.Y,
                    pose.Orientation.Z);
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

            File.AppendAllText(resultpath, builder1.ToString());
            File.AppendAllText(particlepath, builder2.ToString());
            File.AppendAllText(actualpath, builder3.ToString());
        }

        [Test]
        public void TestSin16()
        {
            for (int i = 0; i < 120; i++)
            {
                Console.Write("{0} : ", i);
                Console.WriteLine(this.Sin16(i));
                Console.WriteLine(this.Sin(i, 120));
            }

            Assert.AreEqual(0.1f, this.Sin16(0), 0.001);
            Assert.AreEqual(0.9f, this.Sin16(16), 0.001);
            Assert.AreEqual(0.1f, this.Sin16(32), 0.001);
        }

        ////-----------------------------------------------------------------------------------------------------------------
        ////----------------------------------------Scenario creation--------------------------------------------------------
        ////-----------------------------------------------------------------------------------------------------------------

        ////-----------------------------------------------------------------------------------------------------------------
        ////----------------------------------------Writing to file----------------------------------------------------------
        ////-----------------------------------------------------------------------------------------------------------------
        [Test]
        public void WriteToFiles()
        {
            PositionScenario posscen = this.posscen2;
            OrientationScenario oriscen = this.oriscen2;
            int sceneid = 22;


            int particles = 30;
            float noise = 0.2f;
            double cdfmargin = 0.001;
            int algos = 1111;
            int runamount = 10;

            List<ParticleFilter> filterlist = new List<ParticleFilter>();
            for (int i = 0; i < runamount; i++)
            {
                IParticleGenerator particlegen = new RandomParticleGenerator(new SystemRandomSource());
                IResampler resampler = new MultinomialResampler();
                INoiseGenerator noisegen = new RandomNoiseGenerator(new SystemRandomSource());
                IPosePredictor posepredictor = new LinearPosePredicter();
                ParticleFilter filter = new ParticleFilter(
                    this.fieldsize,
                    particles,
                    cdfmargin,
                    noise,
                    particlegen,
                    posepredictor,
                    noisegen,
                    resampler);
                filterlist.Add(filter);
                filter.AddPositionSource(posscen);
                filter.AddOrientationSource(oriscen);
            }

            LocalizerAnalyser analyzer = new LocalizerAnalyser(
                120,
                filterlist,
                posscen,
                oriscen,
                sceneid,
                cdfmargin,
                noise,
                algos);
        }

        private ParticleFilter creatFilter(int particleamount)
        {
            IParticleGenerator particlegen = new RandomParticleGenerator(new SystemRandomSource());
            INoiseGenerator noisegen = new RandomNoiseGenerator(new SystemRandomSource());
            IResampler resampler = new MultinomialResampler();

            // IResampler resampler = new SimpleResampler();
            IPosePredictor posepredictor = new LinearPosePredicter();
            ParticleFilter filter1 =
                new ParticleFilter(
                    new FieldSize { Xmax = 1, Xmin = 0, Ymax = 1, Ymin = 0, Zmax = 1, Zmin = 0 },
                    particleamount,
                    0.001,
                    0.2f,
                    particlegen,
                    posepredictor,
                    noisegen,
                    resampler);
            return filter1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="frames">The amount of frames it takes to go from 0.1 to 0.9</param>
        /// <returns></returns>
        private float Sin(long timestamp, double frames)
        {
            double x = Math.PI / (2 * frames);
            return Math.Abs((0.8f * (float)Math.Sin(x * timestamp)) + 0.1f);
        }

        private float Sin16(long timestamp)
        {
            double x = Math.PI / 32;
            return Math.Abs((0.8f * (float)Math.Sin(x * timestamp)) + 0.1f);
        }

        private long[] timestamps(int from, int to)
        {
            long[] sf = new long[((to - from) / 2)];
            for (int i = from; i < (to - from) / 2; i++)
            {
                sf[i] = from + i;
            }

            return sf;
        }
    }
}