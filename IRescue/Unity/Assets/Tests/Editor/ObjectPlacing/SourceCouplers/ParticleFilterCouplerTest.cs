﻿// <copyright file="ParticleFilterCouplerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.SensorControllers;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Particle;
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
        FieldSize fieldSize = new FieldSize() { Xmax = 4, Xmin = 0, Ymax = 2, Ymin = 0, Zmax = 4, Zmin = 0 };
        this.coupler = new ParticleFilterCoupler(fieldSize);
    }

    /// <summary>
    /// Test for the particle get
    /// </summary>
    [Test]
    public void ConstructorTest()
    {
        Assert.True(this.coupler.GetLocalizer() is ParticleFilter);
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
