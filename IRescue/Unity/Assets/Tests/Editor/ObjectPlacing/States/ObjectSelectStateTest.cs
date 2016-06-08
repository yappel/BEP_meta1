// <copyright file="ObjectSelectStateTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.ObjectPlacing.States;
using IRescue.Core.Utils;
using Meta;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tests for <see cref="ObjectSelectState"/>
/// </summary>
public class ObjectSelectStateTest
{

    /// <summary>
    /// The used state context
    /// </summary>
    private StateContext stateContext;

    /// <summary>
    /// The tested state
    /// </summary>
    private ObjectSelectState objectSelectState;

    /// <summary>
    /// Setup the test
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.stateContext = new StateContext(null);
        this.objectSelectState = new ObjectSelectState(this.stateContext);
        this.stateContext.SetState(this.objectSelectState);
    }

    /// <summary>
    /// Standard test for constructor.
    /// </summary>
    [Test]
    public void ConstructorTest()
    {
        Assert.IsNotNull(GameObject.FindObjectOfType<Button>());
    }

    /// <summary>
    /// Standard test for constructor.
    /// </summary>
    [Test]
    public void SelectObjectButtonEventTest()
    {
        this.objectSelectState.SelectObjectButtonEvent("Test");
        Assert.True(this.stateContext.SelectedBuilding.Equals("Objects/Test"));
    }

    /// <summary>
    /// Test for the toggle button when you cannot switch states
    /// </summary>
    [Test]
    public void OnToggleButtonFalseTest()
    {
        this.objectSelectState.OnToggleButton();
        Assert.True(this.stateContext.CurrentState is ObjectSelectState);
    }

    /// <summary>
    /// Test for the toggle button when you can switch states
    /// </summary>
    [Test]
    public void OnToggleButtonTest()
    {
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1500)
        {
        }

        this.objectSelectState.OnToggleButton();
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }
}
