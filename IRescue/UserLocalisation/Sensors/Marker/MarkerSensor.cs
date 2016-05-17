﻿// <copyright file="MarkerSensor.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors.Marker
{
    using System;
    using System.Collections.Generic;
    using Core.DataTypes;
    using Core.Utils;

    /// <summary>
    ///  This class keeps track of the location based on Markers.
    /// </summary>
    public class MarkerSensor : IPositionSource, IOrientationSource
    {
        /// <summary>
        /// Error or the orientation in aprilTags
        /// </summary>
        private const float AprilTagsErrorOrientation = 2.0f;

        /// <summary>
        /// Error of the position in aprilTags.
        /// </summary>
        private const float AprilTagsErrorPosition = 0.1f;

        /// <summary>
        ///   Standard deviation of marker tracking.
        /// </summary>
        private readonly float standardDeviationOrientation;

        /// <summary>
        /// Standard deviation of the marker position tracking.
        /// </summary>
        private readonly float standardDeviationPosition;

        /// <summary>
        ///   The predicted locations.
        /// </summary>
        private List<Measurement<Vector3>> positions;

        /// <summary>
        ///   The predicted rotations.
        /// </summary>
        private List<Measurement<Vector3>> orientations;

        /// <summary>
        ///  The MarkerLocations.
        /// </summary>
        private MarkerLocations markerLocations;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkerSensor"/> class.
        /// </summary>
        /// <param name="standardDeviationOrientation">the standard deviation of the orientation</param>
        /// <param name="standardDeviationPosition">The standard deviation of the position</param>
        /// <param name="path">url to the xml file</param>
        public MarkerSensor(float standardDeviationOrientation, float standardDeviationPosition,  string path)
        {
            this.standardDeviationOrientation = standardDeviationOrientation + AprilTagsErrorOrientation;
            this.standardDeviationPosition = standardDeviationPosition + AprilTagsErrorPosition;
            this.markerLocations = new MarkerLocations(path);
        }

        /// <summary>
        ///   Update the locations derived from Markers.
        /// </summary>
        /// <param name="visibleMarkerIds">Dictionary of the ids and transforms ((x,y,z), (pitch, yaw, rotation) in degrees) of the visible Markers.</param>
        public void UpdateLocations(Dictionary<int, Pose> visibleMarkerIds)
        {
            long timeStamp = StopwatchSingleton.Time;
            this.positions = new List<Measurement<Vector3>>(visibleMarkerIds.Count);
            this.orientations = new List<Measurement<Vector3>>(visibleMarkerIds.Count);
            foreach (KeyValuePair<int, Pose> pair in visibleMarkerIds)
            {
                try
                {
                    Pose currentMarkerPose = this.markerLocations.GetMarker(pair.Key);
                    Pose location = AbRelPositioning.GetLocation(currentMarkerPose, pair.Value);
                    this.positions.Add(new Measurement<Vector3>(location.Position, this.standardDeviationOrientation, timeStamp));
                    this.orientations.Add(new Measurement<Vector3>(location.Orientation, this.standardDeviationOrientation, timeStamp));
                }
                catch (UnallocatedMarkerException e)
                {
                    Console.Error.WriteLine("ERROR: ", e.Message);
                }
            }
        }

        /// <summary>
        /// Get the last known orientation measurement.
        /// </summary>
        /// <returns>The orientation <see cref="Measurement{T}"/> with time stamp and standard deviation. Null if no data was found</returns>
        public Measurement<Vector3> GetLastOrientation()
        {
            return this.orientations.Count > 0 ? this.orientations[this.orientations.Count - 1] : null;
        }

        /// <summary>
        /// Get the orientation measurement from the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp to take the measurement from.</param>
        /// <returns>The <see cref="Measurement{T}"/> at the specified timestamp with standard deviation. Null if no data was found</returns>
        public Measurement<Vector3> GetOrientation(long timeStamp)
        {
            return this.GetTimeStampMeasurement(this.orientations, timeStamp);
        }

        /// <summary>
        /// Get the orientations starting from the specified start time stamp up to and including the end time stamp.
        /// </summary>
        /// <param name="startTimeStamp">The start time stamp to include measurements from.</param>
        /// <param name="endTimeStamp">The end time stamp to include measurements up to.</param>
        /// <returns>A list of <see cref="Measurement{T}"/> with their time stamps and standard deviations. Null if no data was found</returns>
        public List<Measurement<Vector3>> GetOrientations(long startTimeStamp, long endTimeStamp)
        {
            return this.GetTimeStampMeasurements(this.orientations, startTimeStamp, endTimeStamp);
        }

        /// <summary>
        /// Get all the known orientation <see cref="Measurement{T}"/> from the source.
        /// </summary>
        /// <returns>A list of all <see cref="Measurement{T}"/> and their time stamps and standard deviations.</returns>
        public List<Measurement<Vector3>> GetAllOrientations()
        {
            return this.orientations;
        }

        /// <summary>
        /// Get the last known position <see cref="Measurement{T}"/> from the sensor.
        /// </summary>
        /// <returns>The last measured acceleration with time stamp and standard deviation. Null if no data was found</returns>
        public Measurement<Vector3> GetLastPosition()
        {
            return this.positions.Count > 0 ? this.positions[this.positions.Count - 1] : null;
        }

        /// <summary>
        /// Get the position <see cref="Measurement{T}"/> from the specified time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp to take the measurement from.</param>
        /// <returns>The <see cref="Measurement{T}"/> at the specified time stamp with standard deviation. Null if no data was found</returns>
        public Measurement<Vector3> GetPosition(long timeStamp)
        {
            return this.GetTimeStampMeasurement(this.positions, timeStamp);
        }

        /// <summary>
        /// Get the positions starting from the specified start time stamp up to and including the end time stamp.
        /// </summary>
        /// <param name="startTimeStamp">The start time stamp to include measurements from.</param>
        /// <param name="endTimeStamp">The end time stamp to include measurements up to.</param>
        /// <returns>A list of <see cref="Measurement{T}"/> with their time stamps and standard deviations.</returns>
        public List<Measurement<Vector3>> GetPositions(long startTimeStamp, long endTimeStamp)
        {
            return this.GetTimeStampMeasurements(this.positions, startTimeStamp, endTimeStamp);
        }

        /// <summary>
        /// Get all the known position <see cref="Measurement{T}"/> from the source.
        /// </summary>
        /// <returns>A list of all <see cref="Measurement{T}"/> and their time stamps and standard deviations.</returns>
        public List<Measurement<Vector3>> GetAllPositions()
        {
            return this.positions;
        }

        /// <summary>
        /// Get a <see cref="Measurement{T}"/> with the give timestamp.
        /// </summary>
        /// <param name="measurements">The list of <see cref="Measurement{T}"/></param>
        /// <param name="timeStamp">The timestamp</param>
        /// <returns>The <see cref="Measurement{T}"/>. Null if no data was measured at this timestamp</returns>
        private Measurement<Vector3> GetTimeStampMeasurement(List<Measurement<Vector3>> measurements, long timeStamp)
        {
            for (int i = 0; i < measurements.Count; i++)
            {
                if (measurements[i].TimeStamp == timeStamp)
                {
                    return measurements[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Get the <see cref="Measurement{T}"/> between two timestamps.
        /// </summary>
        /// <param name="measurements">List of <see cref="Measurement{T}"/></param>
        /// <param name="startTimeStamp">The start timestamp</param>
        /// <param name="endTimeStamp">The end timestamp</param>
        /// <returns>List of <see cref="Measurement{T}"/> between the timestamps</returns>
        private List<Measurement<Vector3>> GetTimeStampMeasurements(
            List<Measurement<Vector3>> measurements, long startTimeStamp, long endTimeStamp)
        {
            List<Measurement<Vector3>> timeMeasurements = new List<Measurement<Vector3>>();
            for (int i = 0; i < measurements.Count; i++)
            {
                if (measurements[i].TimeStamp >= startTimeStamp && measurements[i].TimeStamp <= endTimeStamp)
                {
                    timeMeasurements.Add(measurements[i]);
                }
            }

            return timeMeasurements;
        }
    }
}