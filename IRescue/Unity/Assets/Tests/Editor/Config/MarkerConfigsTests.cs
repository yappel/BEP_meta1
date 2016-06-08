// <copyright file="ImuSensorControllerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;

using Assets.Scripts.Unity.Config;
using Assets.Scripts.Unity.SensorControllers;

using IRescue.UserLocalisation.Sensors.IMU;

using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Tests for the <see cref="GeneralConfigs"/> class
/// </summary>
public class MarkerConfigsTests
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
        string markerconfigpath = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodMarkerConfig.ini");
        string markerconfigpath2 = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodMarkerConfig2.ini");
        List<string> errors;
        MarkerConfigs markers = new MarkerConfigs(markerconfigpath, markerconfigpath2, out errors);
        Assert.AreEqual(0.23, markers.GetConfig(0).Size);
        Assert.AreEqual(0, markers.GetConfig(0).Orientation.X);
        Assert.AreEqual(1, markers.GetConfig(1).Orientation.X);
        Assert.AreEqual(0, errors.Count);
    }

    [Test]
    public void TestFileIsCreated()
    {
        string configpath = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\NotExisting.ini");
        string configpath2 = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodMarkerConfig2.ini");
        try
        {
            Assert.IsFalse(System.IO.File.Exists(configpath));
            List<string> errors;
            MarkerConfigs configs = new MarkerConfigs(configpath, configpath2, out errors);
            errors.RemoveAll(s => s.Length == 0);
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(0.23, configs.GetConfig(0).Size);
            Assert.IsTrue(System.IO.File.Exists(configpath));
        }
        finally
        {
            System.IO.File.Delete(configpath);
        }

    }

    [Test]
    public void TestDefaultValueUsed()
    {
        string configpath = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\WrongMarkerConfig.ini");
        string configpath2 = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodGeneralConfig2.ini");
        List<string> errors;
        MarkerConfigs configs = new MarkerConfigs(configpath, configpath2, out errors);
        errors.RemoveAll(s => s.Length == 0);
        Assert.AreEqual(7, errors.Count);
        Assert.AreEqual(0, configs.GetConfig(2).Orientation.X);
    }
}
