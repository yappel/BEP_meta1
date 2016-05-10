// <copyright file="MonteCarloCoupler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Unity.SensorControllers;
using IRescue.UserLocalisation.Particle;
using IRescue.UserLocalisation.Sensors;

/// <summary>
///  This class couples sources to a localizer.
/// </summary>
public static class MonteCarloCoupler
{
    /// <summary>
    /// Register the sensor sources to a localizer.
    /// </summary>
    /// <param name="localizer">the localizer class used</param>
    /// <param name="sensor">the sensor controller</param>
    /// <returns>if the controller was registered once or more</returns>
    public static bool RegisterSources(MonteCarloLocalizer localizer, AbstractSensorController sensor)
    {
        return RegisterAccelerationReceiver(localizer, sensor.GetAccelerationSource()) ||
            RegisterDisplacementReceiver(localizer, sensor.GetDisplacementSource()) ||
            RegisterOrientationReceiver(localizer, sensor.GetOrientationSource()) ||
            RegisterPositionReceiver(localizer, sensor.GetPositionSource()) ||
            RegisterVelocityReceiver(localizer, sensor.GetVelocitySource());
    }

    /// <summary>
    /// Register a acceleration source
    /// </summary>
    /// <param name="localizer">The localizer</param>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private static bool RegisterAccelerationReceiver(MonteCarloLocalizer localizer, IAccelerationSource source)
    {
        if (source != null)
        {
            // TODO
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register a displacement source
    /// </summary>
    /// <param name="localizer">The localizer</param>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private static bool RegisterDisplacementReceiver(MonteCarloLocalizer localizer, IDisplacementSource source)
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
    /// <param name="localizer">The localizer</param>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private static bool RegisterOrientationReceiver(MonteCarloLocalizer localizer, IOrientationSource source)
    {
        if (source != null)
        {
            // TODO
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register a position source
    /// </summary>
    /// <param name="localizer">The localizer</param>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private static bool RegisterPositionReceiver(MonteCarloLocalizer localizer, IPositionSource source)
    {
        if (source != null)
        {
            // TODO
            return true;
        }

        return false;
    }

    /// <summary>
    /// Register a velocity source
    /// </summary>
    /// <param name="localizer">The localizer</param>
    /// <param name="source">The source to register</param>
    /// <returns>if the source was registered</returns>
    private static bool RegisterVelocityReceiver(MonteCarloLocalizer localizer, IVelocitySource source)
    {
        if (source != null)
        {
            // TODO
            return true;
        }

        return false;
    }
}
