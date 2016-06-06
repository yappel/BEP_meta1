// <copyright file="MarkerLocations.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors.Marker
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using Core.DataTypes;

    /// <summary>
    ///   Class that knows the location of every marker and can give their attributes.
    /// </summary>
    public class MarkerLocations
    {
        /// <summary>
        /// Gets the default value to use when there is no entry for a marker in the config file.
        /// </summary>
        public Pose DefaultValue { get; private set; }

        /// <summary>
        ///   Hash table of the markers in the Scene.
        /// </summary>
        private Dictionary<int, Pose> markers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkerLocations"/> class with the markers from a defines file.
        /// </summary>
        /// <param name="path">Path to the xml file</param>
        /// <param name="defaultPose">The default value to use when there is no entry for a marker in the config file.</param>
        /// <param name="markers">Hash table of the markers in the Scene.</param>
        public MarkerLocations(Pose defaultPose, Dictionary<int, Pose> markers)
        {
            this.markers = markers;
            this.DefaultValue = defaultPose;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkerLocations"/> class.
        ///   Initializes a new instance of the MarkerLocations class without any predefined markers.
        /// </summary>
        public MarkerLocations()
        {
            this.markers = new Dictionary<int, Pose>();
            this.DefaultValue = new Pose();
        }

        /// <summary>
        ///   Adds a Marker to the list.
        /// </summary>
        /// <param name="id">id of the new marker</param>
        /// <param name="markerPose"><see cref="Pose"/> of the marker pose</param>
        public void AddMarker(int id, Pose markerPose)
        {
            this.markers.Add(id, markerPose);
        }

        /// <summary>
        ///   Return the Marker that has the given id.
        /// </summary>
        /// <param name="id">id of the required marker</param>
        /// <returns>Marker with the id</returns>
        public Pose GetMarker(int id)
        {
            if (this.markers.ContainsKey(id))
            {
                return this.markers[id];
            }
            else
            {
                throw new UnallocatedMarkerException("Marker with id=" + id + " was tracked but there was no entry in the config file.");
            }
        }
    }
}