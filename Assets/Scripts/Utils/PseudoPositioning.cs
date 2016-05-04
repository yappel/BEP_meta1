// <copyright file="PseudoPositioning.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

/// <summary>
///   Utility class to calculate the absolute position of a relative position.
/// </summary>
public static class PseudoPositioning
{
    /// <summary>
    ///   Calculates the absolute position of relatePosition.
    /// </summary>
    /// <param name="absoluteLocation">The known absolute position.</param>
    /// <param name="relativeLocation">The position relative to the absolute location.</param>
    /// <returns>The location of the calculated absolute position.</returns>
    public static SensorVector3 GetPosition(Marker absoluteLocation, IRVectorTransform relativeLocation)
    {
        IRVector3 absolutePosition = absoluteLocation.GetPosition();
        IRVector3 absoluteRotation = absoluteLocation.GetRotation();
        IRVector3 distancePosition = relativeLocation.GetPosition();
        IRVector3 distanceRotation = relativeLocation.GetRotation();

        return new SensorVector3(0, 0, 0, 1);
    }
}