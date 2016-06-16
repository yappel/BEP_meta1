// <copyright file="SaveStateTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.ObjectPlacing.States;
using IRescue.Core.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tests for <see cref="SaveState"/>
/// </summary>
public class SaveStateTest
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
    private SaveState saveState;

    /// <summary>
    /// Setup the test
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.stateContext = new StateContext();
        this.saveState = new SaveState(this.stateContext);
        this.stateContext.SetState(this.saveState);
    }

    /// <summary>
    /// Standard test for constructor.
    /// </summary>
    [Test]
    public void ConstructorTest()
    {
        Assert.AreNotEqual(0, GameObject.FindObjectsOfType<Button>().Length);
    }

    /// <summary>
    /// Standard test for this method when it cannot swap state
    /// </summary>
    [Test]
    public void OnBackButtonFalseTest()
    {
        this.saveState.OnBackButton();
        Assert.True(this.stateContext.CurrentState is SaveState);
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

        this.saveState.OnBackButton();
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }
}
