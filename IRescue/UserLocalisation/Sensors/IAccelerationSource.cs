﻿// <copyright file="IAccelerationSource.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors
{
    using System.Collections.Generic;
    using IRescue.Core.DataTypes;

    /// <summary>
    /// Interface which provides information on sensor acceleration data in 3 axes.
    /// </summary>
    public interface IAccelerationSource
    {
        /// <summary>
        /// Get the last known acceleration measurement from the sensor.
        /// </summary>
        /// <returns>The last measured acceleration with time stamp and standard deviation.</returns>
        Measurement<Vector3> GetLastAcceleration();

        /// <summary>
        /// Get the acceleration measurement from the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp to take the measurement from.</param>
        /// <returns>The measurement at the specified time stamp with standard deviation.</returns>
        Measurement<Vector3> GetAcceleration(long timeStamp);

        /// <summary>
        /// Get the accelerations starting from the specified start time stamp up to and including the end time stamp.
        /// </summary>
        /// <param name="startTimeStamp">The start time stamp to include measurements from.</param>
        /// <param name="endTimeStamp">The end time stamp to include measurements up to.</param>
        /// <returns>A list of measurements with their time stamps and standard deviations.</returns>
        List<Measurement<Vector3>> GetAccelerations(long startTimeStamp, long endTimeStamp);

        /// <summary>
        /// Get all the known acceleration measurements from the source.
        /// </summary>
        /// <returns>A list of all measurements and their time stamps and standard deviations.</returns>
        List<Measurement<Vector3>> GetAllAccelerations();
    }
}
