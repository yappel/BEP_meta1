// <copyright file="RunningStateTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.ObjectPlacing.States;
using IRescue.Core.Utils;
using NUnit.Framework;

/// <summary>
/// Tests for <see cref="RunningState"/>
/// </summary>
public class RunningStateTest
{
    /// <summary>
    /// The used state context
    /// </summary>
    private StateContext stateContext;

    /// <summary>
    /// The tested state
    /// </summary>
    private RunningState runningState;

    /// <summary>
    /// Setup the test
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.stateContext = new StateContext(null);
        this.runningState = new RunningState(this.stateContext);
        this.stateContext.SetState(this.runningState);
    }

    /// <summary>
    /// Standard test for this method when it cannot swap state
    /// </summary>
    [Test]
    public void OnConfigButtonFalseTest()
    {
        this.runningState.OnConfigButton();
        Assert.True(this.stateContext.CurrentState is RunningState);
    }

    /// <summary>
    /// Standard test for this method when it can swap states
    /// </summary>
    [Test]
    public void OnConfigButtonTest()
    {
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1500)
        {
        }

        this.runningState.OnConfigButton();
        Assert.True(this.stateContext.CurrentState is ConfigState);
    }
}
