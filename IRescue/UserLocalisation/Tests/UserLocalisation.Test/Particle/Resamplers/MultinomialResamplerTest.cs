// <copyright file="MultinomialResamplerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace UserLocalisation.Test.Particle.Algos.Resamplers
{
    using System.Collections.Generic;

    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// Tests for multinomial resapling algorithm.
    /// </summary>
    public class MultinomialResamplerTest
    {
        private AbstractParticleController controller;

        /// <summary>
        /// A test subject
        /// </summary>
        private MultinomialResampler mr;

        private int particleAmount = 12;

        private Mock<IParticleGenerator> particleGenerator;

        /// <summary>
        /// Setup method
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.particleGenerator = new Mock<IParticleGenerator>();
            this.particleGenerator.Setup(foo => foo.Generate(It.IsAny<int>(), It.IsAny<float>(), It.IsAny<float>())).Returns<int, float, float>((par, min, max) =>
                {
                    float[] res = new float[par];
                    for (int i = 0; i < par; i++)
                    {
                        res[i] = (((float)i / par) * (max - min)) + min;
                    }

                    return res;
                });

            this.controller = new LinearParticleController(this.particleGenerator.Object, this.particleAmount, 0, 200);
            this.mr = new MultinomialResampler();
        }

        /// <summary>
        /// Test if particles with zero weight are not chosen.
        /// </summary>
        [Test]
        public void TestRightSpread()
        {
            this.controller.NormalizeWeights();
            List<float> orignalValues = new List<float>(this.controller.Values);
            float[] orignalWeights = this.controller.Weights;

            this.controller.SetWeightAt(0, 0);
            this.controller.SetWeightAt(1, 0);
            this.controller.SetWeightAt(2, 0);
            this.mr.Resample(this.controller);
            float weightsum = 0;
            foreach (float value in this.controller.Values)
            {
                weightsum += orignalWeights[orignalValues.IndexOf(value)];
            }

            Assert.IsTrue(weightsum > 1);
        }

        /// <summary>
        /// Test if particles with zero weight are not chosen.
        /// </summary>
        [Test]
        public void TestZerosNotChosen()
        {
            float original = this.controller.GetValueAt(0);
            this.controller.SetWeightAt(0, 0);
            this.mr.Resample(this.controller);
            Assert.AreNotEqual(original, this.controller.GetValueAt(0));
        }
    }
}