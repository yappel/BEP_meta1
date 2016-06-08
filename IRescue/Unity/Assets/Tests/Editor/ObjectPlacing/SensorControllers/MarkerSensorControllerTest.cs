// <copyright file="MarkerSensorControllerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;

using Assets.Scripts.Unity.Config;

using IRescue.Core.Distributions;

using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Tests for the <see cref="MarkerSensorController"/> class
/// </summary>
public class MarkerSensorControllerTest
{
    /// <summary>
    /// The tested controller
    /// </summary>
    private MarkerSensorController markerSensorController;

    /// <summary>
    /// Set up the tests
    /// </summary>
    [SetUp]
    public void Setup()
    {
        var gameObject = new GameObject();
        this.markerSensorController = gameObject.AddComponent<MarkerSensorController>();
        string markerconfigpath = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodMarkerConfig.ini");
        string markerconfigpath2 = System.IO.Path.GetFullPath(@"Assets\Tests\Resources\GoodMarkerConfig2.ini");
        List<string> errors;
        MarkerConfigs markers = new MarkerConfigs(markerconfigpath, markerconfigpath2, out errors);
        this.markerSensorController.Init(markers);
    }

    /// <summary>
    /// Test for orientation source
    /// </summary>
    [Test]
    public void GetOrientationSourceTest()
    {
        Assert.IsNotNull(this.markerSensorController.GetOrientationSource());
    }

    /// <summary>
    /// Test for position source
    /// </summary>
    [Test]
    public void GetPositionSourceTest()
    {
        Assert.IsNotNull(this.markerSensorController.GetPositionSource());
    }

    /// <summary>
    /// Test for velocity source
    /// </summary>
    [Test]
    public void GetVelocitySourceTest()
    {
        Assert.IsNull(this.markerSensorController.GetVelocitySource());
    }

    /// <summary>
    /// Test for acceleration source
    /// </summary>
    [Test]
    public void GetAccelerationSourceTest()
    {
        Assert.IsNull(this.markerSensorController.GetAccelerationSource());
    }

    /// <summary>
    /// Test for displacement source
    /// </summary>
    [Test]
    public void GetDisplacementSourceTest()
    {
        Assert.IsNull(this.markerSensorController.GetDisplacementSource());
    }
}
