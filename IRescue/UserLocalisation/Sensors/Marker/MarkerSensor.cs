// <copyright file="MarkerSensor.cs" company="Delft University of Technology">
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
    public class MarkerSensor : ILocationSource, IRotationSource
    {
        /// <summary>
        ///   Standard deviation of marker tracking.
        /// </summary>
        private readonly float standardDeviation;

        /// <summary>
        ///   Path to the saved markers.
        /// </summary>
        private readonly string savePath = "./Assets/Maps/MarkerMap01.xml";

        /// <summary>
        ///   The predicted locations.
        /// </summary>
        private List<Measurement<Vector3>> locations;

        /// <summary>
        ///   The predicted rotations.
        /// </summary>
        private List<Measurement<Vector3>> rotations;

        /// <summary>
        ///   Gets or sets the MarkerLocations.
        /// </summary>
        public MarkerLocations MarkerLocations { get; set; }

        /// <summary>
        ///   Initializes a new instance of the MarkerSensor class.
        /// </summary>
        /// <param name="standardDeviation">The standard deviation of the sensor</param>
        public MarkerSensor(float standardDeviation)
        {
            this.standardDeviation = standardDeviation;
            this.MarkerLocations = new MarkerLocations(this.savePath);
        }

        /// <summary>
        ///   Get the locations based on the visible markers.
        /// </summary>
        /// <returns>List of SensorVector3</returns>
        public List<Measurement<Vector3>> GetLocations()
        {
            return this.locations;
        }

        /// <summary>
        ///   Get the rotations based on the visible markers.
        /// </summary>
        /// <returns>List of SensorVector3</returns>
        public List<Measurement<Vector3>> GetRotations()
        {
            return this.rotations;
        }

        /// <summary>
        ///   Update the locations derived from Markers.
        /// </summary>
        /// <param name="visibleMarkerIds">Dictionary of the ids and transforms ((x,y,z), (pitch, yaw, rotation) in degrees) of the visible Markers.</param>
        /// <param name="timeStamp">The current timestamp of the call</param>
        public void UpdateLocations(Dictionary<int, Pose> visibleMarkerIds, long timeStamp)
        {
            this.locations = new List<Measurement<Vector3>>(visibleMarkerIds.Count);
            this.rotations = new List<Measurement<Vector3>>(visibleMarkerIds.Count);
            foreach (KeyValuePair<int, Pose> pair in visibleMarkerIds)
            {
                try
                {
                    Pose currentMarkerPose = this.MarkerLocations.GetMarker(pair.Key);
                    Pose location = AbRelPositioning.GetLocation(currentMarkerPose, pair.Value);
                    this.locations.Add(new Measurement<Vector3>(location.Position, this.standardDeviation, timeStamp));
                    this.rotations.Add(new Measurement<Vector3>(location.Orientation, this.standardDeviation, timeStamp));
                }
                catch (UnallocatedMarkerException e)
                {
                    Console.WriteLine("ERROR: ", e);
                }
            }
        }
    }
}