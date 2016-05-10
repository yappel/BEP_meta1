// <copyright file="MonteCarloCoupler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using Assets.Scripts.Unity.SensorControllers;
using IRescue.UserLocalisation;
using IRescue.UserLocalisation.Particle;
using IRescue.UserLocalisation.Sensors;

/// <summary>
///  This class couples sources to a localizer.
/// </summary>
public class MonteCarloCoupler : AbstractLocalizerCoupler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MonteCarloCoupler"/> class
    /// </summary>
    /// <param name="localizer">The used localizer</param>
    public MonteCarloCoupler(MonteCarloLocalizer localizer)
    {
        this.Localizer = localizer;
    }

    /// <summary>
    ///  Gets or sets the Localizer of the coupler.
    /// </summary>
    public MonteCarloLocalizer Localizer { get; set; }

    /// <summary>
    /// Register the sensor sources to the localizer.
    /// </summary>
    /// <param name="sensor">the sensor controller</param>
    /// <returns>if the controller was registered once or more</returns>
    public override bool RegisterSource(AbstractSensorController sensor)
    {
        return this.RegisterAccelerationReceiver(sensor.GetAccelerationSource()) ||
            this.RegisterDisplacementReceiver(sensor.GetDisplacementSource()) ||
            this.RegisterOrientationReceiver(sensor.GetOrientationSource()) ||
            this.RegisterPositionReceiver(sensor.GetPositionSource()) ||
            this.RegisterVelocityReceiver(sensor.GetVelocitySource());
    }

    /// <summary>
    /// Register a acceleration source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private new bool RegisterAccelerationReceiver(IAccelerationSource source)
    {
        if (source != null)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register a displacement source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private new bool RegisterDisplacementReceiver(IDisplacementSource source)
    {
        if (source != null)
        {
            // TODO
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register a orientation source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private new bool RegisterOrientationReceiver(IOrientationSource source)
    {
        if (source != null)
        {
            this.Localizer.AddOrientationSource(source);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register a position source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private new bool RegisterPositionReceiver(IPositionSource source)
    {
        if (source != null)
        {
            this.Localizer.AddPositionSource(source);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register a velocity source
    /// </summary>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private new bool RegisterVelocityReceiver(IVelocitySource source)
    {
        if (source != null)
        {
            // TODO
            return true;
        }

        return false;
    }

    public override AbstractUserLocalizer getLocalizer()
    {
        return this.Localizer;
    }
}
