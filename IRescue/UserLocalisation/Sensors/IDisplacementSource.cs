// <copyright file="IDisplacementSource.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors
{
    using IRescue.Core.DataTypes;

    /// <summary>
    /// Interface which provides information on sensor displacement data in 3 axes.
    /// </summary>
    public interface IDisplacementSource
    {
        /// <summary>
        /// Get the displacement from the specified start time stamp up to and including the end time stamp.
        /// </summary>
        /// <param name="startTimeStamp">The start time stamp to include measurements from.</param>
        /// <param name="endTimeStamp">The end time stamp to include measurements up to.</param>
        /// <returns>The total displacement over the specified time period.</returns>
        Measurement<Vector3> GetDisplacement(long startTimeStamp, long endTimeStamp);
    }
}
