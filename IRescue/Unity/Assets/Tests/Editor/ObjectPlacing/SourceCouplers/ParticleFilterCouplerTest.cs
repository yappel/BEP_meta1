// <copyright file="ParticleFilterCouplerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.SensorControllers;
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
/// Tests for <see cref="ParticleFilterCoupler"/>
/// </summary>
public class ParticleFilterCouplerTest
{
    /// <summary>
    /// The tested coupler
    /// </summary>
    private ParticleFilterCoupler coupler;

    /// <summary>
    /// Setup the tests
    /// </summary>
    [SetUp]
    public void Setup()
    {
        ParticleFilter filter = new ParticleFilter(
            10,
            1,
            new FieldSize(),
            new RandomParticleGenerator(new ContinuousUniform()),
            new MultinomialResampler(),
            new RandomNoiseGenerator(new ContinuousUniform()),
            new MovingAverageSmoother(1000));
        this.coupler = new ParticleFilterCoupler(filter);
    }

    /// <summary>
    /// Test for registering a controller
    /// </summary>
    public void RegisterSourceTest1()
    {
        Assert.True(this.coupler.RegisterSource(new ImuSensorController()));
    }

    /// <summary>
    /// Test for registering a marker sensor controller
    /// </summary>
    public void RegisterSourceTest2()
    {
        Assert.True(this.coupler.RegisterSource(new MarkerSensorController()));
    }
}
