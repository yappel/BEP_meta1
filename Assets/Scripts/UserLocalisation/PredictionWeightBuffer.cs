// <copyright file="PredictionWeightBuffer.cs" company="Delft Universite of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

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
    /// <param name="visibleMarkerIds">Array of the ids of the visible Markers.</param>
    public IRVectorTransform PredictLocation(int[] visibleMarkerIds)
    {
        // Call to abstractuserlocalisation
        // Each visibleMarkerId gets loaded from markerLocations
        // We need a way to also give the position + rotation from Unity (IRVectorTransform class can be used)
        return new IRVectorTransform(new IRVector3(0, 0, 0), new IRVector3(0, 0, 0));
    }
}
