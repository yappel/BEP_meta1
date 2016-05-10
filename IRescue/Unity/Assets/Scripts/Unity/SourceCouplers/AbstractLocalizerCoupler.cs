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
    ///  Gets or sets the Localizer of the coupler.
    /// </summary>
    public AbstractUserLocalizer Localizer { get; set; }

    /// <summary>
    /// Register the sensor sources to the localizer.
    /// </summary>
    /// <param name="sensor">the sensor controller</param>
    /// <returns>if the controller was registered once or more</returns>
    public abstract bool RegisterSource(AbstractSensorController sensor);

    /// <summary>
    /// Register a acceleration source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected bool RegisterAccelerationReceiver(IAccelerationSource source)
    {
        return false;
    }

    /// <summary>
    /// Register a displacement source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected bool RegisterDisplacementReceiver(IDisplacementSource source)
    {
        return false;
    }

    /// <summary>
    /// Register a orientation source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected bool RegisterOrientationReceiver(IOrientationSource source)
    {
        return false;
    }

    /// <summary>
    /// Register a position source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected bool RegisterPositionReceiver(IPositionSource source)
    {
        return false;
    }

    /// <summary>
    /// Register a velocity source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    protected bool RegisterVelocityReceiver(IVelocitySource source)
    {
        return false;
    }
}
