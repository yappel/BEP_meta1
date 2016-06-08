﻿// <copyright file="ImuSensorControllerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;

using Assets.Scripts.Unity.Config;
using Assets.Scripts.Unity.SensorControllers;

using IniParser.Exceptions;

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
    public void TestLoadingGood()
    {
        string configpath = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodGeneralConfig.ini");
        string configpath2 = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodGeneralConfig2.ini");
        List<string> errors;
        GeneralConfigs configs = new GeneralConfigs(configpath, configpath2, out errors);
        errors.RemoveAll(s => s.Length == 0);
        Assert.AreEqual(0, errors.Count);
        Assert.AreEqual(0, configs.fieldSize.Xmin);

    }

    [Test]
    public void TestFileIsCreated()
    {
        string configpath = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\NotExisting.ini");
        string configpath2 = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodGeneralConfig2.ini");
        try
        {
            Assert.IsFalse(System.IO.File.Exists(configpath));
            List<string> errors;
            GeneralConfigs configs = new GeneralConfigs(configpath, configpath2, out errors);
            errors.RemoveAll(s => s.Length == 0);
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(0.1f, configs.fieldSize.Xmin);
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
        string configpath = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\AllWrongGeneralConfig.ini");
        string configpath2 = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodGeneralConfig2.ini");
        List<string> errors;
        GeneralConfigs configs = new GeneralConfigs(configpath, configpath2, out errors);
        errors.RemoveAll(s => s.Length == 0);
        Assert.AreEqual(12, errors.Count);
        Assert.AreEqual(3, configs.fieldSize.Xmax);
    }

}
