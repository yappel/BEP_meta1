// <copyright file="IOrientationSource.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors
{
    using System.Collections.Generic;
    using IRescue.Core.DataTypes;

    /// <summary>
    /// Interface which provides information on sensor orientation data in 3 axes.
    /// </summary>
    public interface IOrientationSource
    {
        /// <summary>
        /// Get the last known orientation measurement.
        /// </summary>
        /// <returns>The orientation measurement with time stamp and standard deviation.</returns>
        Measurement<Vector3> GetLastOrientation();

        /// <summary>
        /// Get the orientation measurement from the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp to take the measurement from in milliseconds.</param>
        /// <returns>The measurement at the specified timestamp with standard deviation.</returns>
        Measurement<Vector3> GetOrientation(long timeStamp);

        /// <summary>
        /// Get the orientations starting from the specified start time stamp up to and including the end time stamp.
        /// </summary>
        /// <param name="startTimeStamp">The start time stamp to include measurements from in milliseconds.</param>
        /// <param name="endTimeStamp">The end time stamp to include measurements up to in milliseconds.</param>
        /// <returns>A list of measurements with their time stamps and standard deviations.</returns>
        List<Measurement<Vector3>> GetOrientations(long startTimeStamp, long endTimeStamp);

        /// <summary>
        /// Get all the known orientation measurements from the source.
        /// </summary>
        /// <returns>A list of all measurements and their time stamps and standard deviations.</returns>
        List<Measurement<Vector3>> GetAllOrientations();
    }
}
