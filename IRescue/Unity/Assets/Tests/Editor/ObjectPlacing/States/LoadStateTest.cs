// <copyright file="LoadStateTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity;
using Assets.Scripts.Unity.ObjectPlacing.States;
using IRescue.Core.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tests for <see cref="LoadState"/>
/// </summary>
public class LoadStateTest
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
    private LoadState loadState;

    /// <summary>
    /// Setup the test
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.stateContext = new StateContext();
        this.loadState = new LoadState(this.stateContext);
        this.stateContext.SetState(this.loadState);
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
        this.loadState.OnBackButton();
        Assert.True(this.stateContext.CurrentState is LoadState);
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

        this.loadState.OnBackButton();
        Assert.True(this.stateContext.CurrentState is NeutralState);
    }

    /// <summary>
    /// Standard test for the confirm button when you have selected nothing
    /// </summary>
    [Test]
    public void OnConfirmButtonFalseTest()
    {
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1500)
        {
        }

        this.loadState.OnConfirmButton();
        Assert.True(this.stateContext.CurrentState is LoadState);
    }

    /// <summary>
    /// Standard test for the confirm button when you have selected nothing
    /// </summary>
    [Test]
    public void OnLoadButtonFalseTest()
    {
        GameObject groundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        groundPlane.AddComponent<GroundPlane>().Init(new IRescue.Core.DataTypes.FieldSize());
        long now = StopwatchSingleton.Time;
        while (StopwatchSingleton.Time - now < 1500)
        {
        }

        this.loadState.SaveFilePath = "..\\Assets\\Tests\\Resources\\testload";
        this.loadState.OnConfirmButton();
        Assert.True(this.stateContext.CurrentState is NeutralState);
        Assert.AreEqual(2, GameObject.FindObjectsOfType<BuildingPlane>().Length);
        Assert.IsNotNull(GameObject.FindObjectOfType<BuildingPlane>().GetComponent<BuildingPlane>());
    }
}
