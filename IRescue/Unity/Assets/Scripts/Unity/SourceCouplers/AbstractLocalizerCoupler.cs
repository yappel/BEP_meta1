// <copyright file="AbstractLocalizerCoupler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.SensorControllers;
using IRescue.UserLocalisation;
using IRescue.UserLocalisation.Sensors;

/// <summary>
///  This class couples sources to a localizer.
/// </summary>
public abstract class AbstractLocalizerCoupler
{
    /// <summary>
    /// Register the sensor sources to the localizer.
    /// </summary>
    /// <param name="sensor">the sensor controller</param>
    /// <returns>if the controller was registered once or more</returns>
    public bool RegisterSource(AbstractSensorController sensor)
    {
        bool res = false;
        res = this.RegisterAccelerationReceiver(sensor.GetAccelerationSource()) || res;
        res = this.RegisterDisplacementReceiver(sensor.GetDisplacementSource()) || res;
        res = this.RegisterOrientationReceiver(sensor.GetOrientationSource()) || res;
        res = this.RegisterPositionReceiver(sensor.GetPositionSource()) || res;
        res = this.RegisterVelocityReceiver(sensor.GetVelocitySource()) || res;
        return res; 
    }

    /// <summary>
    /// Return the localizer filter
    /// </summary>
    /// <returns>the localizer</returns>
    public abstract AbstractUserLocalizer GetLocalizer();

    /// <summary>
    /// Register a acceleration source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected abstract bool RegisterAccelerationReceiver(IAccelerationSource source);

    /// <summary>
    /// Register a displacement source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected abstract bool RegisterDisplacementReceiver(IDisplacementSource source);

    /// <summary>
    /// Register a orientation source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected abstract bool RegisterOrientationReceiver(IOrientationSource source);

    /// <summary>
    /// Register a position source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected abstract bool RegisterPositionReceiver(IPositionSource source);

    /// <summary>
    /// Register a velocity source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected abstract bool RegisterVelocityReceiver(IVelocitySource source);
}
