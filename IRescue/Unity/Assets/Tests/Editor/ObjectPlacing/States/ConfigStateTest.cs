// <copyright file="ConfigStateTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.ObjectPlacing.States;
using IRescue.Core.Utils;
using NUnit.Framework;

/// <summary>
/// Tests for <see cref="RunningState"/>
/// </summary>
public class ConfigStateTest
{
    /// <summary>
    /// The used state context
    /// </summary>
    private StateContext stateContext;

    /// <summary>
    /// The tested state
    /// </summary>
    private ConfigState configState;

    /// <summary>
    /// Setup the test
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.stateContext = new StateContext();
        this.configState = new ConfigState(this.stateContext);
        this.stateContext.SetState(this.configState);
    }

    /// <summary>
    /// Standard test for this method when it cannot swap state
    /// </summary>
    [Test]
    public void OnBackButtonFalseTest()
    {
        this.configState.OnBackButton();
        Assert.True(this.stateContext.CurrentState is ConfigState);
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

        this.configState.OnBackButton();
        Assert.True(this.stateContext.CurrentState is RunningState);
    }

    /// <summary>
    /// Standard test for this method when it cannot swap state
    /// </summary>
    [Test]
    public void OnEditModeButtonFalseTest()
    {
        this.configState.OnEditModeButton();
        Assert.True(this.stateContext.CurrentState is ConfigState);
    }

    /// <summary>
    /// Standard test for this method when it can swap states
    /// </summary>
    [Test]
    public void OnEditModeButtonTest()
    {
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1500)
        {
        }

        this.configState.OnEditModeButton();
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }
}
