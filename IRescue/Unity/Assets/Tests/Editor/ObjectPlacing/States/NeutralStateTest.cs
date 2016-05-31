// <copyright file="NeutralStateTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity;
using Assets.Scripts.Unity.ObjectPlacing.States;
using IRescue.Core.Utils;
using Meta;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tests for <see cref="NeutralState"/>
/// </summary>
public class NeutralStateTest
{
    /// <summary>
    /// The used state context
    /// </summary>
    private StateContext stateContext;

    /// <summary>
    /// The tested state
    /// </summary>
    private NeutralState neutralState;

    /// <summary>
    /// Setup the test
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.stateContext = new StateContext(null);
        this.neutralState = new NeutralState(this.stateContext);
        this.stateContext.SetState(this.neutralState);
    }

    /// <summary>
    /// Standard test for constructor.
    /// </summary>
    [Test]
    public void ConstructorTest()
    {
        Assert.True(GameObject.FindObjectsOfType<Button>().Length >= 1);
    }

    /// <summary>
    /// Test for the toggle button when you cannot switch states
    /// </summary>
    [Test]
    public void OnToggleButtonFalseTest()
    {
        this.neutralState.OnToggleButton();
        Assert.True(this.stateContext.CurrentState is NeutralState);
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

        this.neutralState.OnToggleButton();
        Assert.True(this.stateContext.CurrentState is ObjectSelectState);
    }

    /// <summary>
    /// Test for point when you cannot switch states
    /// </summary>
    [Test]
    public void OnPointFalseTest()
    {
        this.neutralState.OnPoint(new Vector3());
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }

    /// <summary>
    /// Test for point when you cannot switch states
    /// </summary>
    [Test]
    public void OnPointFalseTest2()
    {
        this.neutralState.OnPoint(new GameObject());
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }

    /// <summary>
    /// Test for point when you cannot switch states
    /// </summary>
    [Test]
    public void OnPointFalseTest3()
    {
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1751)
        {
        }

        this.neutralState.OnPoint(new GameObject());
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }

    /// <summary>
    /// Test for point when you cannot switch states
    /// </summary>
    [Test]
    public void OnPointFalseTest4()
    {
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1751)
        {
        }

        this.neutralState.OnPoint(new GameObject());
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }
}
