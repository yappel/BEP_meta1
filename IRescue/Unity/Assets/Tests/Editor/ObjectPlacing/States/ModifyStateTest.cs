// <copyright file="ModifyStateTest.cs" company="Delft University of Technology">
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
/// Tests for <see cref="ModifyState"/>
/// </summary>
public class ModifyStateTest
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
    private ModifyState modifyState;

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
        this.modifyState = new ModifyState(this.stateContext, this.gameObject);
        this.stateContext.SetState(this.modifyState);
    }

    /// <summary>
    /// Standard test for constructor.
    /// </summary>
    [Test]
    public void ConstructorTest()
    {
        Assert.True(GameObject.FindObjectOfType<Button>() != null);
    }

    /// <summary>
    /// Standard test for this method when it cannot swap state
    /// </summary>
    [Test]
    public void OnConfirmButtonFalseTest()
    {
        this.modifyState.OnConfirmButton();
        Assert.True(this.stateContext.CurrentState is ModifyState);
    }

    /// <summary>
    /// Standard test for this method when it can swap states
    /// </summary>
    [Test]
    public void OnConfirmButtonTest()
    {
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1500)
        {
        }

        this.modifyState.OnConfirmButton();
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }

    /// <summary>
    /// Test for the delete button when you cannot switch states
    /// </summary>
    [Test]
    public void OnDeleteButtonFalseTest()
    {
        this.modifyState.OnDeleteButton();
        Assert.IsNotNull(this.gameObject);
        Assert.True(this.stateContext.CurrentState is ModifyState);
    }

    /// <summary>
    /// Test for the delete button when you can switch states
    /// </summary>
    [Test]
    public void OnDeleteButtonTest()
    {
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1500)
        {
        }

        this.modifyState.OnDeleteButton();
        Assert.IsNotNull(this.gameObject);
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }
}
