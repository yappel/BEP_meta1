// <copyright file="InitScript.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enums;
using Assets.Scripts.Unity.SensorControllers;
using Assets.Scripts.Unity.SourceCouplers;
using IRescue.UserLocalisation;
using UnityEngine;

/// <summary>
///  This script initialized the entire setup. This is the only script that should be added to a GameObject in the Unity editor.
/// </summary>
public class InitScript : MonoBehaviour
{
    /// <summary>
    /// Enum type of the filter that is going to be used
    /// </summary>
    private Filters usedFilter = Filters.Particle;

    /// <summary>
    /// Size of the used markers in meters
    /// </summary>
    private float markerSize = 0.23f;

    /// <summary>
    ///  Called when the game starts.
    /// </summary>
    public void Start()
    {
        Meta.MarkerDetector.Instance.SetMarkerSize(this.markerSize);
        this.AddControllers();
        AbstractLocalizerCoupler coupler = LocalizerFactory.Get(this.usedFilter);
        this.InitControllers(coupler);
        this.InitUser(coupler.GetLocalizer());
        this.InitMarker();
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
    ///  Get all the Scripts that inherit <see cref="AbstractSensorController"/>.
    /// </summary>
    /// <returns>List of the controller class types that inherit <see cref="AbstractSensorController"/></returns>
    private IEnumerable<AbstractSensorController> GetAbstractControllers()
    {
        return typeof(AbstractSensorController)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AbstractSensorController)) && !t.IsAbstract)
            .Select(t => (AbstractSensorController)Activator.CreateInstance(t));
    }

    /// <summary>
    ///  Initialize the controllers and register them to a localizer. 
    ///  Disable the controller if its source is not required by the current localizer method.
    /// </summary>
    /// <param name="localizer">The Localizer filter</param>
    private void InitControllers(AbstractLocalizerCoupler localizer)
    {
        AbstractSensorController[] controllers = gameObject.GetComponents<AbstractSensorController>();
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i].Init();
            controllers[i].enabled = localizer.RegisterSource(controllers[i]);
        }
    }

    /// <summary>
    ///  Initialize the <see cref="UserController"/> and a localizer to the user (Camera).
    /// </summary>
    /// <param name="localizer">The Localizer filter</param>
    private void InitUser(AbstractUserLocalizer localizer)
    {
        gameObject.AddComponent<UserController>().Init(localizer);
    }

    /// <summary>
    /// Create a cube with marker target behavior so that markers are tracked
    /// </summary>
    private void InitMarker()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.AddComponent<Rigidbody>();
        cube.transform.position = new Vector3(0, -10000, 0);
        cube.AddComponent<Meta.MetaBody>().markerTarget = true;
    }
}
