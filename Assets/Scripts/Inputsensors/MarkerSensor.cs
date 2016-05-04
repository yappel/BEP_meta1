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
    ///   All mapped Markers.
    /// </summary>
    private MarkerLocations markerLocations;

    /// <summary>
    ///   The predicted locations.
    /// </summary>
    private List<SensorVector3> locations;

    /// <summary>
    ///   Initializes a new instance of the MarkerSensor class.
    /// </summary>
    public MarkerSensor()
    {
        this.markerLocations = new MarkerLocations("./Assets/Maps/MarkerMap01.xml");
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
    /// <param name="visibleMarkerIds">Dictionary of the ids and transforms of the visible Markers.</param>
    public void UpdateLocations(Dictionary<int, IRVectorTransform> visibleMarkerIds)
    {
        this.locations = new List<SensorVector3>(visibleMarkerIds.Count);
        foreach (KeyValuePair<int, IRVectorTransform> pair in visibleMarkerIds)
        {
            try
            {
                Marker currentMarker = this.markerLocations.GetMarker(pair.Key);
                this.locations.Add(PseudoPositioning.GetPosition(currentMarker, pair.Value));
            }
            catch (UnallocatedMarkerException e)
            {
                Console.WriteLine("ERROR: ", e);
            }
        }
    }
}
