// <copyright file="IMotionSource.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using IRescue.Core.Datatypes;

namespace IRescue.UserLocalisation.Sensors
{
    using System.Collections.Generic;

    /// <summary>
    ///   A MotionSource provides information about the motion of the user.
    /// </summary>
    public interface IMotionSource
    {
        /// <summary>
        /// Gets the last acceleration vector from the MotionSource with the timestamp of the acceleration.
        /// </summary>
        /// <returns>MotionResult with the timestamp of the acceleration and the acceleration vector.</returns>
        MotionResult GetCurrentAcceleration();

        /// <summary>
        /// Gets the last known velocity vector from the MotionSource with the timestamp of the velocity.
        /// </summary>
        /// <returns>MotionResult with the timestamp of the velocity and the velocity vector.</returns>
        MotionResult GetCurrentVelocity();

        /// <summary>
        /// Gets a list of measured accelerations and timestamps from the MotionSource for the specified time period.
        /// </summary>
        /// <param name="startPeriod">The starting timestamp to include measurements from.</param>
        /// <param name="endPeriod">The ending timestamp after which no measurements are included.</param>
        /// <returns>A list of MotionResults with the timestamps and acceleration vectors.</returns>
        List<MotionResult> GetAccelerations(long startPeriod, long endPeriod);

        /// <summary>
        /// Gets a list of measured accelerations and timestamps from the MotionSource for the specified time period.
        /// </summary>
        /// <param name="startPeriod">The starting timestamp to include measurements from.</param>
        /// <param name="endPeriod">The ending timestamp after which no measurements are included.</param>
        /// <returns>A list of MotionResults with the timestamps and velocity vectors.</returns>
        List<MotionResult> GetVelocities(long startPeriod, long endPeriod);

        /// <summary>
        /// Gets the measured displacement from the MotionSource over a specified time period.
        /// </summary>
        /// <param name="startPeriod">The starting timestamp to calculate the displacement from.</param>
        /// <param name="endPeriod">The ending timestamp to calculate the displacement to.</param>
        /// <returns>SensorVector3 with the displacement in all axis.</returns>
        SensorVector3 GetDisplacement(long startPeriod, long endPeriod);
    }

    /// <summary>
    /// MotionResult is a tuple which stores a SensorVector3 measurement and the timestamp from the measurement.
    /// </summary>
    public class MotionResult
    {
        /// <summary>
        /// Gets or sets the timestamp at which the measurement was taken.
        /// </summary>
        public long TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the measurement.
        /// </summary>
        public SensorVector3 Result { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MotionResult"/> class.
        /// MotionResult has a specified timestamp for the measurement and SensorVector3 measurement.
        /// </summary>
        /// <param name="timeStamp">The timestamp at which the measurement was taken.</param>
        /// <param name="result">The measurement.</param>
        public MotionResult(long timeStamp, SensorVector3 result)
        {
            this.TimeStamp = timeStamp;
            this.Result = result;
        }
    }
}