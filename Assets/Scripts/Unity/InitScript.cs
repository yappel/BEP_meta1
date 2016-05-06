// <copyright file="InitScript.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Assets.Scripts.Inputsensors;
using Assets.Scripts.Unity.SensorControllers;
using Assets.Scripts.UserLocalisation;
using UnityEngine;

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed.")]

/// <summary>
///  This script initialized the entire setup. This is the only script that should be added to a GameObject in the Unity editor.
/// </summary>
public class InitScript : MonoBehaviour
{
    /// <summary>
    ///  Called when the game starts.
    /// </summary>
    void Start()
    {
        this.AddControllers();

        // Todo initialise localiser
        AbstractUserLocalizer localizer = null;
        this.InitControllers(localizer);
    }

    /// <summary>
    ///  Add the controllers to the GameObject.
    /// </summary>
    private void AddControllers()
    {
        IEnumerable<AbstractSensorController> sensorControllers = this.GetAbstractControllers();
        foreach (AbstractSensorController controller in sensorControllers)
        {
            gameObject.AddComponent(controller.GetType());
        }
    }

    /// <summary>
    ///  Get all the Scripts that inherit AbstractSensorController.
    /// </summary>
    /// <returns>List of the controller class types that inherit AbstractSensorController</returns>
    private IEnumerable<AbstractSensorController> GetAbstractControllers()
    {
        return typeof(AbstractSensorController)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AbstractSensorController)) && !t.IsAbstract)
            .Select(t => (AbstractSensorController)Activator.CreateInstance(t));
    }

    /// <summary>
    ///  Initialize the controllers and register them to a localizer.
    /// </summary>
    /// <param name="localizer">The Localizer filter</param>
    private void InitControllers(AbstractUserLocalizer localizer)
    {
        AbstractSensorController[] controllers = gameObject.GetComponents<AbstractSensorController>();
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i].Init();
            this.RegisterSources(localizer, controllers[i]);
        }
    }

    /// <summary>
    /// Register the sensor sources to a localizer.
    /// </summary>
    /// <param name="localizer">the localizer class used</param>
    /// <param name="sensor">the sensor controller</param>
    private void RegisterSources(AbstractUserLocalizer localizer, AbstractSensorController sensor)
    {
        this.RegisterLocationReceiver(localizer, sensor.GetLocationSource());
        this.RegisterMotionReceiver(localizer, sensor.GetMotionSource());
        this.RegisterRotationReceiver(localizer, sensor.GetRotationSource());
    }

    /// <summary>
    /// Registers a location source if the localizer can register it.
    /// </summary>
    /// <param name="localizer">The AbstractUserLocalizer</param>
    /// <param name="source">The location source to register</param>
    private void RegisterLocationReceiver(AbstractUserLocalizer localizer, ILocationSource source)
    {
        if (localizer is ILocationReceiver && source != null)
        {
            (localizer as ILocationReceiver).RegisterLocationReceiver(source);
        }
    }

    /// <summary>
    /// Registers a motion source if the localizer can register it.
    /// </summary>
    /// <param name="localizer">The AbstractUserLocalizer</param>
    /// <param name="source">The motion source to register</param>
    private void RegisterMotionReceiver(AbstractUserLocalizer localizer, IMotionSource source)
    {
        if (localizer is IMotionReceiver && source != null)
        {
            (localizer as IMotionReceiver).RegisterMotionSource(source);
        }
    }

    /// <summary>
    /// Registers a rotation source if the localizer can register it.
    /// </summary>
    /// <param name="localizer">The AbstractUserLocalizer</param>
    /// <param name="source">The rotation source to register</param>
    private void RegisterRotationReceiver(AbstractUserLocalizer localizer, IRotationSource source)
    {
        if (localizer is IRotationReceiver && source != null)
        {
            (localizer as IRotationReceiver).RegisterRotationSource(source);
        }
    }
}
