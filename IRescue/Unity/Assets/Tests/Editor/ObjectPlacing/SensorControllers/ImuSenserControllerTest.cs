// <copyright file="ImuSensorControllerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.SensorControllers;

using IRescue.UserLocalisation.Sensors.IMU;

using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Tests for the <see cref="MarkerSensorController"/> class
/// </summary>
public class ImuSensorControllerTest
{
    /// <summary>
    /// The tested controller
    /// </summary>
    private ImuSensorController imuSensorController;

    /// <summary>
    /// Set up the tests
    /// </summary>
    [SetUp]
    public void Setup()
    {
        var gameObject = new GameObject();
        this.imuSensorController = gameObject.AddComponent<ImuSensorController>();
        this.imuSensorController.Init(new IMUSource(new IRescue.Core.Distributions.Normal(1), new IRescue.Core.Distributions.Normal(1), 100));
    }

    /// <summary>
    /// Test for orientation source
    /// </summary>
    [Test]
    public void GetOrientationSourceTest()
    {
        Assert.IsNotNull(this.imuSensorController.GetOrientationSource());
    }

    /// <summary>
    /// Test for position source
    /// </summary>
    [Test]
    public void GetPositionSourceTest()
    {
        Assert.IsNull(this.imuSensorController.GetPositionSource());
    }

    /// <summary>
    /// Test for velocity source
    /// </summary>
    [Test]
    public void GetVelocitySourceTest()
    {
        Assert.IsNotNull(this.imuSensorController.GetVelocitySource());
    }

    /// <summary>
    /// Test for acceleration source
    /// </summary>
    [Test]
    public void GetAccelerationSourceTest()
    {
        Assert.IsNotNull(this.imuSensorController.GetAccelerationSource());
    }

    /// <summary>
    /// Test for displacement source
    /// </summary>
    [Test]
    public void GetDisplacementSourceTest()
    {
        Assert.IsNotNull(this.imuSensorController.GetDisplacementSource());
    }
}
