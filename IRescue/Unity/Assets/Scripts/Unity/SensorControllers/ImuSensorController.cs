// <copyright file="ImuSensorController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Assets.Scripts.Unity.SensorControllers;
using IRescue.Core.DataTypes;
using IRescue.UserLocalisation.Sensors;
using IRescue.UserLocalisation.Sensors.Marker;
using Meta;
using UnityEngine;
using Vector3 = IRescue.Core.DataTypes.Vector3;
using IRescue.UserLocalisation.Sensors.IMU;

/// <summary>
///   This class keeps track of visible markers and the probable user location based on that.
/// </summary>
public class ImuSensorController : AbstractSensorController
{
    /// <summary>
    ///   Keeps track of the user location and calculates the new one on update.
    /// </summary>
    private IMUSource imuSource;

    /// <summary>
    ///   The standard deviation of the acceleration
    /// </summary>
    private float accelerationStd = 0.0f;

    /// <summary>
    ///   The standard deviation of the orientation
    /// </summary>
    private float orientationStd = 0.0f;

    /// <summary>
    ///   The measurement buffer size
    /// </summary>
    private int bufferSize;

    /// <summary>
    ///   Method called when creating a UserController.
    /// </summary>
    public override void Init()
    {
        this.imuSource = new IMUSource(this.accelerationStd, this.orientationStd, this.bufferSize);
    }

    /// <summary>
    ///   Return the Orientation source.
    /// </summary>
    /// <returns>The IOrientationSource</returns>
    public new IOrientationSource GetOrientationSource()
    {
        return this.imuSource;
    }

    /// <summary>
    ///   Method calles on every frame.
    /// </summary>
    public void Update()
    {
        this.imuSource.AddMeasurements();
    }
}
