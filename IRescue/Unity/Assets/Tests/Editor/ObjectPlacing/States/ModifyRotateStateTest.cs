// <copyright file="ModifyRotateStateTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity;
using Assets.Scripts.Unity.ObjectPlacing.States;
using IRescue.Core.Utils;
using Meta;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Tests for <see cref="ModifyRotateState"/>
/// </summary>
[TestFixture]
public class ModifyRotateStateTest
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
    private ModifyRotateState modifyRotateState;

    /// <summary>
    /// Setup the test
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.gameObject = new GameObject();
        this.gameObject.AddComponent<MetaBody>();
        this.gameObject.AddComponent<BuildingPlane>();
        this.stateContext = new StateContext();
        this.modifyRotateState = new ModifyRotateState(this.stateContext, this.gameObject);
        this.stateContext.SetState(this.modifyRotateState);
    }

    /// <summary>
    /// Standard test for constructor.
    /// </summary>
    [Test]
    public void ConstructorTest()
    {
        Assert.True(this.gameObject.GetComponent<MetaBody>().rotateObjectOnTwoHandedGrab);
    }

    /// <summary>
    /// Standard test for this method when it cannot swap state
    /// </summary>
    [Test]
    public void OnBackButtonFalseTest()
    {
        this.modifyRotateState.OnBackButton();
        Assert.True(this.gameObject.GetComponent<MetaBody>().rotateObjectOnTwoHandedGrab);
        Assert.True(this.stateContext.CurrentState is ModifyRotateState);
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

        this.modifyRotateState.OnBackButton();
        Assert.False(this.gameObject.GetComponent<MetaBody>().rotateObjectOnTwoHandedGrab);
        Assert.True(this.stateContext.CurrentState is ModifyState);
    }
}
