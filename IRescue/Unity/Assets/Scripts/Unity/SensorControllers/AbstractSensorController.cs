// <copyright file="AbstractSensorController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using IRescue.UserLocalisation.Sensors;

namespace Assets.Scripts.Unity.SensorControllers
{
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
        ///   Return the motion source.
        /// </summary>
        /// <returns>the ILocationSource</returns>
        public ILocationSource GetLocationSource()
        {
            return null;
        }

        /// <summary>
        ///   Return the motion source.
        /// </summary>
        /// <returns>The IMotionSource</returns>
        public IMotionSource GetMotionSource()
        {
            return null;
        }

        /// <summary>
        ///   Return the motion source.
        /// </summary>
        /// <returns>The IRotationSource</returns>
        public IRotationSource GetRotationSource()
        {
            return null;
        }
    }
}