// <copyright file="AbstractSensorController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.SensorControllers
{
    using IRescue.UserLocalisation.Sensors;
    using UnityEngine;

    /// <summary>
    ///   abstract class for SensorControllers.
    /// </summary>
    public abstract class AbstractSensorController : MonoBehaviour
    {
        /// <summary>
        ///   Initializes the controller.
        /// </summary>
        public abstract void Init();

        /// <summary>
        ///   Return the acceleration source.
        /// </summary>
        /// <returns>The IAccelerationSource</returns>
        public abstract IAccelerationSource GetAccelerationSource();

        /// <summary>
        ///   Return the Displacement source.
        /// </summary>
        /// <returns>The IDisplacementSource</returns>
        public abstract IDisplacementSource GetDisplacementSource();

        /// <summary>
        ///   Return the Orientation source.
        /// </summary>
        /// <returns>The IOrientationSource</returns>
        public abstract IOrientationSource GetOrientationSource();

        /// <summary>
        ///   Return the position source.
        /// </summary>
        /// <returns>the IPositionSource</returns>
        public abstract IPositionSource GetPositionSource();

        /// <summary>
        ///   Return the velocity source.
        /// </summary>
        /// <returns>the IVelocitySource</returns>
        public abstract IVelocitySource GetVelocitySource();
    }
}