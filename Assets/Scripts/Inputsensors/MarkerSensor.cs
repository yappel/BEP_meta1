// <copyright file="MarkerSensor.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Assets.Scripts.Inputsensors;

/// <summary>
///  This class keeps track of the location based on Markers.
/// </summary>
public class MarkerSensor : ILocationSource
{
    /// <summary>
    ///   The predicted locations.
    /// </summary>
    private List<SensorVector3> locations;

    /// <summary>
    ///   Gets or sets the MarkerLocations.
    /// </summary>
    public MarkerLocations MarkerLocations { get; set; }

    /// <summary>
    ///   Initializes a new instance of the MarkerSensor class.
    /// </summary>
    public MarkerSensor()
    {
        this.MarkerLocations = new MarkerLocations("./Assets/Maps/MarkerMap01.xml");
    }

    /// <summary>
    ///   Get the locations based on the visible markers.
    /// </summary>
    /// <returns>List of SensorVector3</returns>
    public List<SensorVector3> GetLocations()
    {
        return this.locations;
    }

    /// <summary>
    ///   Update the locations derived from Markers.
    /// </summary>
    /// <param name="visibleMarkerIds">Dictionary of the ids and transforms ((x,y,z), (pitch, yaw, rotation) in degrees) of the visible Markers.</param>
    public void UpdateLocations(Dictionary<int, IRVectorTransform> visibleMarkerIds)
    {
        this.locations = new List<SensorVector3>(visibleMarkerIds.Count);
        foreach (KeyValuePair<int, IRVectorTransform> pair in visibleMarkerIds)
        {
            try
            {
                Marker currentMarker = this.MarkerLocations.GetMarker(pair.Key);
                IRVector3 location = Positioning.GetPosition(currentMarker, pair.Value);
                this.locations.Add(new SensorVector3(location.GetX(), location.GetY(), location.GetZ(), 1));
            }
            catch (UnallocatedMarkerException e)
            {
                Console.WriteLine("ERROR: ", e);
            }
        }
    }
}
