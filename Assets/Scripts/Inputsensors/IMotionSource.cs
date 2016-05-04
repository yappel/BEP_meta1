// <copyright file="IMotionSource.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Inputsensors
{
    /// <summary>
    ///   A MotionSource provides information about the motion of the user.
    /// </summary>
    public interface IMotionSource
    {
        /// <summary>
        /// Gets the motion vector.
        /// </summary>
        /// <returns>A SensorVector3 containing the motion in the xyz dimensions in m/s and a deviation to specify the certainty of this data</returns>
        SensorVector3 GetMotion();
    }
}