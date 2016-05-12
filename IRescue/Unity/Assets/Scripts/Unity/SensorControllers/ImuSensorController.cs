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
    private int bufferSize = 30;

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
        UnityEngine.Vector3 unity_acc = IMULocalizer.Instance.accelerometerValues;
        UnityEngine.Vector3 unity_ori = IMULocalizer.Instance.localizerOrientation;
        Vector3 acc = new Vector3(unity_acc.x * (float)(9.81 / 8192), unity_acc.y * (float)(9.81 / 8192), unity_acc.z * (float)(9.81 / 8192));
        Vector3 ori = new Vector3(unity_ori.x, unity_ori.y, unity_ori.z);
        this.imuSource.AddMeasurements(IRescue.Core.Utils.StopwatchSingleton.Time, acc, ori);
    }
}
