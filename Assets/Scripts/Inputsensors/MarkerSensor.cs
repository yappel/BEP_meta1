// <copyright file="MarkerSensor.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Assets.Scripts.Inputsensors;

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
    private List<SensorVector3> locations;

    /// <summary>
    ///   The predicted rotations.
    /// </summary>
    private List<SensorVector3> rotations;

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
    public List<SensorVector3> GetLocations()
    {
        return this.locations;
    }

    /// <summary>
    ///   Get the rotations based on the visible markers.
    /// </summary>
    /// <returns>List of SensorVector3</returns>
    public List<SensorVector3> GetRotations()
    {
        return this.rotations;
    }

    /// <summary>
    ///   Update the locations derived from Markers.
    /// </summary>
    /// <param name="visibleMarkerIds">Dictionary of the ids and transforms ((x,y,z), (pitch, yaw, rotation) in degrees) of the visible Markers.</param>
    public void UpdateLocations(Dictionary<int, IRDoubleVector> visibleMarkerIds)
    {
        this.locations = new List<SensorVector3>(visibleMarkerIds.Count);
        this.rotations = new List<SensorVector3>(visibleMarkerIds.Count);
        foreach (KeyValuePair<int, IRDoubleVector> pair in visibleMarkerIds)
        {
            try
            {
                Marker currentMarker = this.MarkerLocations.GetMarker(pair.Key);
                IRDoubleVector location = Positioning.GetLocation(currentMarker, pair.Value);
                this.locations.Add(new SensorVector3(location.GetPosition().GetX(), location.GetPosition().GetY(), location.GetPosition().GetZ(), this.standardDeviation));
                this.rotations.Add(new SensorVector3(location.GetRotation().GetX(), location.GetRotation().GetY(), location.GetRotation().GetZ(), this.standardDeviation));
            }
            catch (UnallocatedMarkerException e)
            {
                Console.WriteLine("ERROR: ", e);
            }
        }
    }
}
