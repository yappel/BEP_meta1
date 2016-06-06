// <copyright file="ImuSensorControllerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.Config;
using Assets.Scripts.Unity.SensorControllers;

using IRescue.UserLocalisation.Sensors.IMU;

using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Tests for the <see cref="GeneralConfigs"/> class
/// </summary>
public class GeneralConfigTests
{

    /// <summary>
    /// Set up the tests
    /// </summary>
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void TestLoadingBoth()
    {
        MarkerConfigs markers = new MarkerConfigs(@"Tests\Resources\GoodMarkerConfig.ini", @"Tests\Resources\GoodMarkerConfig.ini");
        GeneralConfigs configs = new GeneralConfigs(@"Tests\Resources\GoodGeneralConfig.ini", @"Tests\Resources\GoodGeneralConfig.ini", markers);
        Assert.Pass();
    }
}
