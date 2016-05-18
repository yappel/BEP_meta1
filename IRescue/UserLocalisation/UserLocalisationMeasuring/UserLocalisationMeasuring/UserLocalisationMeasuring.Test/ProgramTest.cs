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
        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestMethod()
        {
            IRescue.UserLocalisationMeasuring.Program.CalcBestParticleSettings();
        }

        /// <summary>
        /// TODO TODO
        /// </summary>
        [Test]
        public void TestMethod2()
        {
            RandomSource random = new SystemRandomSource();

            OrientationScenario oriscen1 = new OrientationScenario(
                (p => (float)Math.Sin(p)), (p => (float)Math.Sin(p)), (p => (float)Math.Cos(p)), timestamps(0, 400), (() => (float)((random.NextDouble() * 6) - 3)), 2f);
            PositionScenario posscen1 = new PositionScenario(
                (p => (float)Math.Sin(Math.PI * p / 16)), (p => (float)Math.Sin(p)), (p => (float)Math.Cos(p)), timestamps(0, 400), (() => (float)((random.NextDouble() * 0.1) - 0.05)), 0.05f);

            OrientationScenario oriscen2 = new OrientationScenario(
                (p => (float)Math.Sin(p)), (p => (float)Math.Sin(p)), (p => (float)Math.Cos(p)), timestamps(0, 400), (() => (float)((random.NextDouble() * 6) - 3)), 2f);
            PositionScenario posscen2 = new PositionScenario(
                (p => 0.1f), (p => 0.1f), (p => 0.2f), timestamps(0, 400), (() => (float)((random.NextDouble() * 0.1) - 0.05)), 0.05f);
            //PositionScenario posscen2 = new PositionScenario(
            //    (p => 0.8f), (p => 0.8f), (p => 0.8f), timestamps(0, 400), (() => 0f), 0.05f);

            int particleamount = 30;
            ParticleFilter filter1 = creatFilter(particleamount);
            filter1.AddOrientationSource(oriscen2);
            filter1.AddPositionSource(posscen1);

            var resultpath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\Filter_results.txt");
            var particlepath = System.IO.Path.GetFullPath("D:\\Users\\Yoeri 2\\Documenten\\MATLAB\\Filter_particles.txt");


            System.IO.File.WriteAllText(resultpath, "0 0 0 0 0 0 " + Environment.NewLine);
            System.IO.File.WriteAllText(particlepath, "");
            StringBuilder builder1 = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            for (int i = 0; i < particleamount; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    builder2.Append(filter1.Particles[i, j]);
                    builder2.Append(" ");
                    builder2.Append(filter1.Weights[i, j]);
                    builder2.Append(" ");
                }
                builder2.AppendLine();
            }

            Pose pose = null;
            for (int j = 1; j < 60; j++)
            {
                pose = filter1.CalculatePose(j);
                builder1.Clear();
                builder1.Append(pose.Position.X);
                builder1.Append(" ");
                builder1.Append(pose.Position.Y);
                builder1.Append(" ");
                builder1.Append(pose.Position.Z);
                builder1.Append(" ");
                builder1.Append(pose.Orientation.X);
                builder1.Append(" ");
                builder1.Append(pose.Orientation.Y);
                builder1.Append(" ");
                builder1.Append(pose.Orientation.Z);
                builder1.AppendLine();

                for (int i = 0; i < particleamount; i++)
                {
                    for (int jj = 0; jj < 6; jj++)
                    {
                        builder2.Append(filter1.Particles[i, jj]);
                        builder2.Append(" ");
                        builder2.Append(filter1.Weights[i, jj]);
                        builder2.Append(" ");
                    }
                    builder2.AppendLine();
                }
            }
            System.IO.File.AppendAllText(resultpath, builder1.ToString());
            System.IO.File.AppendAllText(particlepath, builder2.ToString());
        }

        private ParticleFilter creatFilter(int particleamount)
        {
            IParticleGenerator particlegen = new RandomParticleGenerator(new SystemRandomSource());
            INoiseGenerator noisegen = new RandomNoiseGenerator(new SystemRandomSource());
            IResampler resampler = new MultinomialResampler();
            //IResampler resampler = new SimpleResampler();
            IPosePredictor posepredictor = new LinearPosePredicter();
            ParticleFilter filter1 = new ParticleFilter(new FieldSize() { Xmax = 1, Xmin = 0, Ymax = 1, Ymin = 0, Zmax = 1, Zmin = 0 }, particleamount, 0.001, 0.01f, particlegen,
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
