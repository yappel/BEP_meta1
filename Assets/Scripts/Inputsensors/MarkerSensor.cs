﻿// <copyright file="MarkerSensor.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using Assets.Scripts.Inputsensors;
using System;
using System.Collections.Generic;

/// <summary>
///  This class prepares an array of Vector 4 for user localization.
/// </summary>
public class MarkerSensor: ILocationSource
{
    /// <summary>
    ///   Weight of the marker locations.
    /// </summary>
    private const float WEIGHTMARKER = 1;

    /// <summary>
    ///   All mapped Markers.
    /// </summary>
    private MarkerLocations markerLocations;

    /// <summary>
    ///   Initializes a new instance of the PredictionWeightBuffer class.
    /// </summary>
    public MarkerSensor()
    {
        this.markerLocations = new MarkerLocations("./Assets/Maps/MarkerMap01.xml");
    }

    public List<SensorVector3> GetLocations()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///   Calculate possible locations based on the visible markers.
    /// </summary>
    /// <returns>IRVectorTransform the predicted location and rotation</returns>
    /// <param name="visibleMarkerIds">Hash table of the ids and transforms of the visible Markers.</param>
    public IRVectorTransform PredictLocation(Dictionary<int, IRVectorTransform> visibleMarkerIds)
    {
        List<IRVectorDeviation> predictedLocations = this.GetMarkerBasedLocations(visibleMarkerIds);
        //// Call to abstractuserlocalisation with predictedLocations.
        //// TODO add accelerometer prediction.
        return new IRVectorTransform(new IRVector3(0, 0, 0), new IRVector3(0, 0, 0));
    }

    /// <summary>
    ///   Get the locations based on the markers.
    /// </summary>
    /// <returns>Predicted locations and their standard deviations</returns>
    /// <param name="visibleMarkerIds">Hash table of the ids and transforms of the visible Markers.</param>
    private List<IRVectorDeviation> GetMarkerBasedLocations(Dictionary<int, IRVectorTransform> visibleMarkerIds)
    {
        List<IRVectorDeviation> predictions = new List<IRVectorDeviation>(visibleMarkerIds.Count);
        foreach (KeyValuePair<int, IRVectorTransform> pair in visibleMarkerIds)
        {
            try
            {
                Marker currentMarker = this.markerLocations.GetMarker(pair.Key);
                predictions.Add(this.GetUserToMarkerPosition(currentMarker, pair.Value));
            }
            catch (UnallocatedMarkerException e)
            {
                Console.WriteLine("ERROR: ", e);
            }
        }

        return predictions;
    }

    /// <summary>
    ///   Calculate the position based on the knows location and the relative distance to the detected marker.
    /// </summary>
    /// <returns>IRVectorTransform the predicted location and rotation</returns>
    /// <param name="storedMarker">The stored Marker of which the absolute location is known.</param>
    /// <param name="detectedMarker">The detected Marker.</param>
    private IRVectorDeviation GetUserToMarkerPosition(Marker storedMarker, IRVectorTransform detectedMarker)
    {
        IRVector3 absolutePosition = storedMarker.GetPosition();
        IRVector3 absoluteRotation = storedMarker.GetRotation();
        IRVector3 distancePosition = detectedMarker.GetPosition();
        IRVector3 distanceRotation = detectedMarker.GetRotation();

        // TODO return the location based on this data with the declared weight.
        return new IRVectorDeviation(new IRVector3(0, 0, 0), new IRVector3(0, 0, 0), WEIGHTMARKER);
    }
}