// <copyright file="WaterLevelControllerTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Tests for <see cref="WaterLevelController"/>
/// </summary>
public class WaterLevelControllerTest
{
    /// <summary>
    /// Test for the initialize method
    /// </summary>
    [Test]
    public void InitTest()
    {
        GameObject waterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        waterPlane.AddComponent<WaterLevelController>().Init(5, 10);
        Assert.AreEqual(5, waterPlane.transform.position.x);
        Assert.AreEqual(10, waterPlane.transform.position.z);
    }
}
