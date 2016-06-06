// <copyright file="LocalizerFactoryTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Enums;
using Assets.Scripts.Unity.SourceCouplers;

using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Particle;
using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
using IRescue.UserLocalisation.Particle.Algos.Resamplers;
using IRescue.UserLocalisation.Particle.Algos.Smoothers;

using MathNet.Numerics.Distributions;

using NUnit.Framework;

/// <summary>
/// Tests for <see cref="LocalizerCouplerFactory"/>
/// </summary>
public class LocalizerFactoryTest
{
    /// <summary>
    /// Test for the particle get
    /// </summary>
    [Test]
    public void ParticleTest()
    {
        ParticleFilter filter = new ParticleFilter(
           10,
           1,
           new FieldSize(),
           new RandomParticleGenerator(new ContinuousUniform()),
           new MultinomialResampler(),
           new RandomNoiseGenerator(new ContinuousUniform()),
           new MovingAverageSmoother(1000));
        Assert.True(LocalizerCouplerFactory.Get(filter) is ParticleFilterCoupler);
    }
}
