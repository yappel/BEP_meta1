// <copyright file="PredictionWeightBuffer.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;

/// <summary>
///  This class prepares an array of Vector 4 for user localization.
/// </summary>
public class PredictionWeightBuffer
{
    /// <summary>
    ///   Weight of the marker locations.
    /// </summary>
    private const float WEIGHTMARKER = 1;

    /// <summary>
    ///   The used user localization technique.
    /// </summary>
    private AbstractUserLocalisation userLocalisation;

    /// <summary>
    ///   All mapped Markers.
    /// </summary>
    private MarkerLocations markerLocations;

    /// <summary>
    ///   Initializes a new instance of the PredictionWeightBuffer class.
    /// </summary>
    public PredictionWeightBuffer()
    {
        // TODO userLocalisation = new ...
        this.markerLocations = new MarkerLocations();
    }

    /// <summary>
    ///   Initializes a new instance of the PredictionWeightBuffer class.
    /// </summary>
    /// <returns>IRVectorTransform the predicted location and rotation</returns>
    /// <param name="visibleMarkerIds">Hash table of the ids and transforms of the visible Markers.</param>
    public IRVectorTransform PredictLocation(Dictionary<int, IRVectorTransform> visibleMarkerIds)
    {
        foreach (KeyValuePair<int, IRVectorTransform> pair in visibleMarkerIds)
        {
            Marker currentMarker = this.markerLocations.GetMarker(pair.Key);
            IRVector3 absolutePosition = currentMarker.GetPosition();
            IRVector3 absoluteRotation = currentMarker.GetRotation();
            IRVector3 distancePosition = pair.Value.GetPosition();
            IRVector3 distanceRotation = pair.Value.GetRotation();
            //// TODO Calculate location and add to List.
        }
        //// Call to abstractuserlocalisation
        return new IRVectorTransform(new IRVector3(0, 0, 0), new IRVector3(0, 0, 0));
    }
}
