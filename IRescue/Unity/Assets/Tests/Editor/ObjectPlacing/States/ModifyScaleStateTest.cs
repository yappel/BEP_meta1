// <copyright file="ModifyScaleStateTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity;
using Assets.Scripts.Unity.ObjectPlacing.States;
using IRescue.Core.Utils;
using Meta;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Tests for <see cref="ModifyScaleState"/>
/// </summary>
public class ModifyScaleStateTest
{
    /// <summary>
    /// The used game object
    /// </summary>
    private GameObject gameObject;

    /// <summary>
    /// The used state context
    /// </summary>
    private StateContext stateContext;

    /// <summary>
    /// The tested state
    /// </summary>
    private ModifyScaleState modifyscaleState;

    /// <summary>
    /// Setup the test
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.gameObject = new GameObject();
        this.gameObject.AddComponent<MetaBody>();
        this.gameObject.AddComponent<BuildingPlane>();
        this.stateContext = new StateContext(null);
        this.modifyscaleState = new ModifyScaleState(this.stateContext, this.gameObject);
        this.stateContext.SetState(this.modifyscaleState);
    }

    /// <summary>
    /// Standard test for constructor.
    /// </summary>
    [Test]
    public void ConstructorTest()
    {
        Assert.True(this.gameObject.GetComponent<MetaBody>().scaleObjectOnTwoHandedGrab);
    }

    /// <summary>
    /// Standard test for this method when it cannot swap state
    /// </summary>
    [Test]
    public void OnBackButtonFalseTest()
    {
        this.modifyscaleState.OnBackButton();
        Assert.True(this.gameObject.GetComponent<MetaBody>().scaleObjectOnTwoHandedGrab);
        Assert.True(this.stateContext.CurrentState is ModifyScaleState);
    }

    /// <summary>
    /// Standard test for this method when it can swap states
    /// </summary>
    [Test]
    public void OnBackButtonTest()
    {
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1500)
        {
        }

        this.modifyscaleState.OnBackButton();
        Assert.False(this.gameObject.GetComponent<MetaBody>().scaleObjectOnTwoHandedGrab);
        Assert.True(this.stateContext.CurrentState is ModifyState);
    }
}
