// <copyright file="AbstractSensorController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.SensorControllers
{
    using IRescue.UserLocalisation.Sensors;
    using UnityEngine;

    /// <summary>
    ///   Abstract class for SensorControllers.
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
        public IAccelerationSource GetAccelerationSource()
        {
            return null;
        }

        /// <summary>
        ///   Return the Displacement source.
        /// </summary>
        /// <returns>The IDisplacementSource</returns>
        public IDisplacementSource GetDisplacementSource()
        {
            return null;
        }

        /// <summary>
        ///   Return the Orientation source.
        /// </summary>
        /// <returns>The IOrientationSource</returns>
        public IOrientationSource GetOrientationSource()
        {
            return null;
        }

        /// <summary>
        ///   Return the position source.
        /// </summary>
        /// <returns>the IPositionSource</returns>
        public IPositionSource GetPositionSource()
        {
            return null;
        }

        /// <summary>
        ///   Return the velocity source.
        /// </summary>
        /// <returns>the IVelocitySource</returns>
        public IVelocitySource GetVelocitySource()
        {
            return null;
        }
    }
}