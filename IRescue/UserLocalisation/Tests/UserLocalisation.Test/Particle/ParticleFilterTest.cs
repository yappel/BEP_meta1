// <copyright file="ParticleFilterTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace UserLocalisation.Test.Particle
{
    using System.Collections.Generic;

    using IRescue.Core.DataTypes;
    using IRescue.Core.Distributions;
    using IRescue.UserLocalisation.Particle;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.PosePrediction;
    using IRescue.UserLocalisation.Sensors;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    ///     Test for the particles
    /// </summary>
    public class ParticleFilterTest
    {
        /// <summary>
        ///     Default probability distribution.
        /// </summary>
        private Mock<IDistribution> dist;

        /// <summary>
        ///     Size of the field
        /// </summary>
        private FieldSize fieldsize;

        /// <summary>
        ///     Filter to use in tests.
        /// </summary>
        private ParticleFilter filter;

        /// <summary>
        ///     Noise generator mock
        /// </summary>
        private Mock<INoiseGenerator> noisegen;

        /// <summary>
        ///     Particle list
        /// </summary>
        private float[] particles;

        /// <summary>
        ///     Pose predictor mock
        /// </summary>
        private Mock<IPosePredictor> posepredictor;

        /// <summary>
        ///     Particle generator mock
        /// </summary>
        private Mock<IParticleGenerator> ptclgen;

        /// <summary>
        ///     Resample mock
        /// </summary>
        private Mock<IResampler> resampler;

        /// <summary>
        ///     Initialization method
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.dist = new Mock<IDistribution>();
            this.ptclgen = new Mock<IParticleGenerator>();
            this.noisegen = new Mock<INoiseGenerator>();
            this.posepredictor = new Mock<IPosePredictor>();
            this.resampler = new Mock<IResampler>();
            this.fieldsize = new FieldSize { Xmax = 2, Xmin = 0, Ymax = 2, Ymin = 0, Zmax = 2, Zmin = 0 };
            var particleamount = 30;

            float[] particles = new float[particleamount];
            for (int i = 0; i < particleamount; i++)
            {
                particles[i] = 1;
                this.particles = particles;
            }

            float[] particles2 = new float[particleamount * 1];
            for (int i = 0; i < particleamount; i++)
            {
                particles2[i] = 1;
            }

            this.particles = particles;

            this.ptclgen.Setup(foo => foo.Generate(particleamount, It.IsAny<float>(), It.IsAny<float>())).Returns(particles);
            this.posepredictor.SetReturnsDefault(new float[] { 0, 0, 0, 0, 0, 0 });

            //this.filter = new ParticleFilter(this.fieldsize, particleamount, 0.005f, 0.0f, this.ptclgen.Object, this.posepredictor.Object, this.noisegen.Object, this.resampler.Object);
            this.filter = new ParticleFilter(particleamount, 0.01f, this.fieldsize, this.ptclgen.Object, this.resampler.Object, this.noisegen.Object);

            Mock<IPositionSource> possourcemock = new Mock<IPositionSource>();
            List<Measurement<Vector3>> returnlist = new List<Measurement<Vector3>> { new Measurement<Vector3>(new Vector3(2.5f, 1.8f, 2.5f), 0, this.dist.Object) };
            possourcemock.Setup(foo => foo.GetPositions(It.IsAny<long>(), It.IsAny<long>())).Returns(returnlist);
            Mock<IOrientationSource> orisourcemock = new Mock<IOrientationSource>();
            List<Measurement<Vector3>> returnlist2 = new List<Measurement<Vector3>> { new Measurement<Vector3>(new Vector3(40f, 40f, 40f), 0, this.dist.Object) };
            orisourcemock.Setup(foo => foo.GetOrientations(It.IsAny<long>(), It.IsAny<long>())).Returns(returnlist2);
            Mock<IDisplacementSource> dissourcemock = new Mock<IDisplacementSource>();
            dissourcemock.Setup(foo => foo.GetDisplacement(It.IsAny<long>(), It.IsAny<long>())).Returns(new Measurement<Vector3>(new Vector3(1f, 1f, 1f), 0, this.dist.Object));
            this.filter.AddPositionSource(possourcemock.Object);
            this.filter.AddOrientationSource(orisourcemock.Object);
            this.filter.AddDisplacementSource(dissourcemock.Object);
        }

        /// <summary>
        ///     Test if the filter doesn't crash when there are no sources.
        /// </summary>
        [Test]
        public void TestParticleFilterRunWithoutSources()
        {
            ParticleFilter filterr = new ParticleFilter(30, 0.01f, this.fieldsize, this.ptclgen.Object, this.resampler.Object, this.noisegen.Object);

            this.ptclgen.Setup(foo => foo.Generate(30, It.IsAny<int>(), It.IsAny<int>())).Returns(this.particles);
            this.posepredictor.SetReturnsDefault(new float[] { 0, 0, 0, 0, 0, 0 });
            for (int i = 0; i < 1000; i++)
            {
                filterr.CalculatePose(i);
            }

            Assert.Pass();
        }
    }
}