// <copyright file="MarkerLocations.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors.Marker
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Core.DataTypes;

    /// <summary>
    ///   Class that knows the location of every marker and can give their attributes.
    /// </summary>
    public class MarkerLocations
    {
        /// <summary>
        ///   Hash table of the markers in the Scene.
        /// </summary>
        private Dictionary<int, Pose> markers;

        /// <summary>
        ///   Initializes a new instance of the MarkerLocations class with the markers from a defines file.
        /// </summary>
        /// <param name="path">Path to the xml file</param>
        public MarkerLocations(string path)
        {
            this.markers = new Dictionary<int, Pose>();
            this.LoadMarkers(path);
        }

        /// <summary>
        ///   Initializes a new instance of the MarkerLocations class without any predefined markers.
        /// </summary>
        public MarkerLocations()
        {
            this.markers = new Dictionary<int, Pose>();
        }

        /// <summary>
        ///   Adds a Marker to the list.
        /// </summary>
        /// <param name="id">id of the new marker</param>
        /// <param name="markerPose">The new marker</param>
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
                throw new UnallocatedMarkerException("Marker with id=" + id + " was tracked but not initialized in the XML");
            }
        }

        /// <summary>
        ///   Loads the given XML file and parses it to Markers.
        /// </summary>
        /// <param name="path">Path to the xml file</param>
        private void LoadMarkers(string path)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                XmlNodeList nodeList = xml.SelectNodes("/markers/marker");
                foreach (XmlNode node in nodeList)
                {
                    this.markers.Add(XmlConvert.ToInt32(node["id"].InnerText), this.XmlTransform(node));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: ", e);
            }
        }

        /// <summary>
        ///   Load the marker variables from the XML node.
        /// </summary>
        /// <param name="node">The current XML node.</param>
        /// <returns>The parsed marker.</returns>
        private Pose XmlTransform(XmlNode node)
        {
            XmlNode xmlPosition = node.SelectSingleNode("position");
            XmlNode xmlRotation = node.SelectSingleNode("rotation");
            Vector3 position = new Vector3(
                XmlConvert.ToInt32(xmlPosition.SelectSingleNode("x").InnerText),
                XmlConvert.ToInt32(xmlPosition.SelectSingleNode("y").InnerText),
                XmlConvert.ToInt32(xmlPosition.SelectSingleNode("z").InnerText));
            Vector3 rotation = new Vector3(
                XmlConvert.ToInt32(xmlRotation.SelectSingleNode("x").InnerText),
                XmlConvert.ToInt32(xmlRotation.SelectSingleNode("y").InnerText),
                XmlConvert.ToInt32(xmlRotation.SelectSingleNode("z").InnerText));
      
            return new Pose(position, rotation);
        }
    }
}