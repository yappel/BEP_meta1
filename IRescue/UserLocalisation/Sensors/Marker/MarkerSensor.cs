// <copyright file="MarkerSensor.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors.Marker
{
    using System;
    using System.Collections.Generic;
    using Core.DataTypes;
    using Core.Distributions;
    using Core.Utils;
    using static MathNet.Numerics.Trig;

    /// <summary>
    ///  This class keeps track of the location based on Markers.
    /// </summary>
    public class MarkerSensor : IPositionSource, IOrientationSource
    {
        /// <summary>
        ///  The MarkerLocations.
        /// </summary>
        private MarkerLocations markerLocations;

        /// <summary>
        ///   The predicted rotations.
        /// </summary>
        private Measurement<Vector3>[] orientations;

        /// <summary>
        ///   The predicted locations.
        /// </summary>
        private Measurement<Vector3>[] positions;

        /// <summary>
        /// The type of probability distribution belonging to the measurements of the position.
        /// </summary>
        private IDistribution posDistType;

        /// <summary>
        /// The type of probability distribution belonging to the measurements of the orientation.
        /// </summary>
        private IDistribution oriDistType;

        /// <summary>
        /// Current index pointer;
        /// </summary>
        private int pointer = 0;

        /// <summary>
        /// Length of the buffer.
        /// </summary>
        private int bufferLength = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkerSensor"/> class.
        /// </summary>
        /// <param name="markerLocations">The MarkerLocations object storing all markers.</param>
        /// <param name="posDistType">The type of probability distribution belonging to the measurements of the position.</param>
        /// <param name="oriDistType">The type of probability distribution belonging to the measurements of the orientation.</param>
        public MarkerSensor(MarkerLocations markerLocations, IDistribution posDistType, IDistribution oriDistType)
        {
            this.markerLocations = markerLocations;
            this.posDistType = posDistType;
            this.oriDistType = oriDistType;
            this.orientations = new Measurement<Vector3>[this.bufferLength];
            this.positions = new Measurement<Vector3>[this.bufferLength];
            this.Measurements = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkerSensor"/> class with a given buffer size.
        /// </summary>
        /// <param name="markerLocations">The object storing all world marker locations.</param>
        /// <param name="bufferLength">Length of the buffer</param>
        /// <param name="posDistType">The type of the distribution of the positions measurements</param>
        /// <param name="oriDistType">The type of the distribution of the orientation measurements</param>
        public MarkerSensor(MarkerLocations markerLocations, int bufferLength, IDistribution posDistType, IDistribution oriDistType)
            : this(markerLocations, posDistType, oriDistType)
        {
            this.bufferLength = bufferLength;
            this.orientations = new Measurement<Vector3>[this.bufferLength];
            this.positions = new Measurement<Vector3>[this.bufferLength];
        }

        /// <summary>
        /// Gets the amount of measurements.
        /// </summary>
        public int Measurements { get; private set; }

        /// <summary>
        /// Update the locations derived from Markers.
        /// </summary>
        /// <param name="timeStamp">Time stamp at which the measurements were taken.</param>
        /// <param name="visibleMarkerIds">Dictionary of the ids and transforms ((x,y,z), (pitch, yaw, rotation) in degrees) of the visible Markers.</param>
        public void UpdateLocations(long timeStamp, Dictionary<int, Pose> visibleMarkerIds)
        {
            foreach (KeyValuePair<int, Pose> pair in visibleMarkerIds)
            {
                try
                {
                    Pose markerWorldPose = this.markerLocations.GetMarker(pair.Key);
                    Pose markerUserPose = pair.Value;

                    TransformationMatrix transformationUserToMarker = new TransformationMatrix(markerUserPose.Position, markerUserPose.Orientation);
                    TransformationMatrix transformationWorldToMarker = new TransformationMatrix(markerWorldPose.Position, markerWorldPose.Orientation);
                    TransformationMatrix transformationUserToWorld = new TransformationMatrix();
                    transformationWorldToMarker.Multiply(transformationUserToMarker.Inverse(), transformationUserToWorld);
                    Vector4 position = new Vector4(0, 0, 0, 1);
                    transformationUserToWorld.Multiply(position, position);

                    // TODO Could be faster => add method in Vector4 to create a Vector3 from it
                    Vector3 pos = new Vector3(position.X, position.Y, position.Z);
                    this.positions[this.pointer] = new Measurement<Vector3>(pos, timeStamp, this.posDistType);

                    RotationMatrix rotationUserToWorld = transformationUserToWorld.GetRotation();
                    Vector3 orientation = new Vector3((float)RadianToDegree(Math.Atan2(rotationUserToWorld[2, 1], rotationUserToWorld[2, 2])), (float)RadianToDegree(Math.Atan2(-1 * rotationUserToWorld[2, 0], Math.Sqrt(Math.Pow(rotationUserToWorld[2, 1], 2) + Math.Pow(rotationUserToWorld[2, 2], 2)))), (float)RadianToDegree(Math.Atan2(rotationUserToWorld[1, 0], rotationUserToWorld[0, 0])));
                    this.orientations[this.pointer] = new Measurement<Vector3>(orientation, timeStamp, this.oriDistType);

                    this.pointer = this.pointer >= this.bufferLength - 1 ? 0 : this.pointer + 1;
                    this.Measurements = this.Measurements < this.bufferLength ? this.Measurements + 1 : this.bufferLength;
                }
                catch (UnallocatedMarkerException e)
                {
                    Console.Error.WriteLine("ERROR: {0}", e.Message);
                }
            }
        }

        /// <summary>
        /// Get the last known orientation measurement.
        /// </summary>
        /// <returns>The orientation <see cref="Measurement{T}"/> with time stamp and standard deviation. Null if no data was found</returns>
        public Measurement<Vector3> GetLastOrientation()
        {
            return this.Measurements > 0 ? this.orientations[(((this.pointer - 1) % this.bufferLength) + this.bufferLength) % this.bufferLength] : null;
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
        /// Gets all the measurements closets to a given time stamp and within a given range of that time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp in milliseconds of the desired measurements.</param>
        /// <param name="range">The amount of milliseconds that the actual returned may differ from the desired time stamp.</param>
        /// <returns>A list of all measurements that have the the smallest difference in time stamp.</returns>
        public List<Measurement<Vector3>> GetOrientationClosestTo(long timeStamp, long range)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            long mindiff = long.MaxValue;
            for (int i = 0; i < this.Measurements; i++)
            {
                Measurement<Vector3> measurement = this.orientations[i];
                long diff = Math.Abs(measurement.TimeStamp - timeStamp);
                if (diff == mindiff)
                {
                    res.Add(measurement);
                }
                else if (diff < mindiff)
                {
                    res.Clear();
                    mindiff = diff;
                    res.Add(measurement);
                }
            }

            return res;
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
            return this.ArrayToList(this.orientations);
        }

        /// <summary>
        /// Get the last known position <see cref="Measurement{T}"/> from the sensor.
        /// </summary>
        /// <returns>The last measured acceleration with time stamp and standard deviation. Null if no data was found</returns>
        public Measurement<Vector3> GetLastPosition()
        {
            return this.Measurements > 0 ? this.positions[(((this.pointer - 1) % this.bufferLength) + this.bufferLength) % this.bufferLength] : null;
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
        /// Gets all the measurements closets to a given time stamp and within a given range of that time stamp.
        /// </summary>
        /// <param name="timeStamp">The time stamp in milliseconds of the desired measurements.</param>
        /// <param name="range">The amount of milliseconds that the actual returned may differ from the desired time stamp.</param>
        /// <returns>A list of all measurements that have the the smallest difference in time stamp.</returns>
        public List<Measurement<Vector3>> GetPositionsClosestTo(long timeStamp, long range)
        {
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>();
            long mindiff = long.MaxValue;
            for (int i = 0; i < this.Measurements; i++)
            {
                Measurement<Vector3> measurement = this.positions[i];
                long diff = Math.Abs(measurement.TimeStamp - timeStamp);
                if (diff == mindiff)
                {
                    res.Add(measurement);
                }
                else if (diff < mindiff)
                {
                    res.Clear();
                    mindiff = diff;
                    res.Add(measurement);
                }
            }

            return res;
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
            return this.ArrayToList(this.positions);
        }

        /// <summary>
        /// Convert an array to a list
        /// </summary>
        /// <param name="measurements">the array</param>
        /// <returns>the list in the correct order of measurements</returns>
        private List<Measurement<Vector3>> ArrayToList(Measurement<Vector3>[] measurements)
        {
            int length = this.GetIterationLength(measurements);
            List<Measurement<Vector3>> res = new List<Measurement<Vector3>>(length);
            for (int i = 0; i < length; i++)
            {
                res.Add(measurements[this.GetIndex(i)]);
            }

            return res;
        }

        /// <summary>
        /// Get a <see cref="Measurement{T}"/> with the give timestamp. This is NOT the recommended method since multiple markers can be seen in 1 iteration.
        /// </summary>
        /// <param name="measurements">The array of <see cref="Measurement{T}"/></param>
        /// <param name="timeStamp">The timestamp</param>
        /// <returns>The <see cref="Measurement{T}"/>. Null if no data was measured at this timestamp</returns>
        private Measurement<Vector3> GetTimeStampMeasurement(Measurement<Vector3>[] measurements, long timeStamp)
        {
            for (int i = 0; i < this.GetIterationLength(measurements); i++)
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
        private List<Measurement<Vector3>> GetTimeStampMeasurements(Measurement<Vector3>[] measurements, long startTimeStamp, long endTimeStamp)
        {
            int length = this.GetIterationLength(measurements);
            List<Measurement<Vector3>> timeMeasurements = new List<Measurement<Vector3>>();
            for (int i = 0; i < length; i++)
            {
                int index = this.GetIndex(i);
                if (measurements[index].TimeStamp >= startTimeStamp && measurements[index].TimeStamp <= endTimeStamp)
                {
                    timeMeasurements.Add(measurements[index]);
                }
            }

            return timeMeasurements;
        }

        /// <summary>
        /// Return the previous data point.
        /// </summary>
        /// <param name="i">iterator value</param>
        /// <returns>measurement i iterations ago</returns>
        private int GetIndex(int i)
        {
            return (((this.pointer - i) % this.bufferLength) + this.bufferLength - 1) % this.bufferLength;
        }

        /// <summary>
        /// Return the length of the data size.
        /// </summary>
        /// <param name="measurements">the measurements array</param>
        /// <returns>the length</returns>
        private int GetIterationLength(Measurement<Vector3>[] measurements)
        {
            return Math.Min(this.bufferLength, this.Measurements);
        }
    }
}