// <copyright file="Positioning.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using UnityEngine;

/// <summary>
///   Utility class to calculate the absolute position of a relative position.
/// </summary>
public static class Positioning
{ 
    /// <summary>
    ///   Convert a radian to a degree angle.
    /// </summary>
    private static double toRadian = Math.PI / 180;

    /// <summary>
    ///   Calculates the absolute position of relatePosition.
    /// </summary>
    /// <param name="absoluteLocation">The known absolute position.</param>
    /// <param name="relativeLocation">The position relative to the absolute location.</param>
    /// <returns>The location of the calculated absolute position.</returns>
    public static IRVectorTransform GetPosition(Marker absoluteLocation, IRVectorTransform relativeLocation)
    {
        IRVector3 absolutePosition = absoluteLocation.GetPosition();
        IRVector3 absoluteRotation = absoluteLocation.GetRotation();
        IRVector3 relativeRotation = relativeLocation.GetRotation();
        float distance = CalculateDistance(absolutePosition, relativeLocation.GetPosition());
        IRVectorTransform newLocation = CalculatePosition(absolutePosition, absoluteRotation, relativeRotation, distance);

        Debug.Log("ROTATION--- X:" + newLocation.GetRotation().GetX() + ", Y:" + newLocation.GetRotation().GetY() + ", Z:" + newLocation.GetRotation().GetZ());
        Debug.Log("POSITION--- X:" + newLocation.GetPosition().GetX() + ", Y:" + newLocation.GetPosition().GetY() + ", Z:" + newLocation.GetPosition().GetZ());

        return newLocation;
    }

    /// <summary>
    ///   Calculate the Euclidean distance.
    /// </summary>
    /// <param name="pos1">First coordinate</param>
    /// <param name="pos2">Second coordinate</param>
    /// <returns>3D euclidean distance</returns>
    private static float CalculateDistance(IRVector3 pos1, IRVector3 pos2)
    {
        float xValue = pos1.GetX() - pos2.GetX();
        float yValue = pos1.GetY() - pos2.GetY();
        float zValue = pos1.GetZ() - pos2.GetZ();

        return (float)Math.Sqrt((xValue * xValue) + (yValue * yValue) + (zValue * zValue));
    }

    /// <summary>
    ///   Calculate the Absolute location.
    /// </summary>
    /// <param name="absolutePosition">Position of the known point</param>
    /// <param name="absoluteRotation">Rotation of the known point</param>
    /// <param name="relativeRotation">Rotation of the relative point</param>
    /// <param name="distance">Distance to the absolute point as seen from the relative point</param>
    /// <returns>Location and rotation of the relatively given location</returns>
    private static IRVectorTransform CalculatePosition(
        IRVector3 absolutePosition, IRVector3 absoluteRotation, IRVector3 relativeRotation, float distance)
    {
        IRVector3 rotation = new IRVector3(
            relativeRotation.GetX() - absoluteRotation.GetX(),
            relativeRotation.GetY() - absoluteRotation.GetY(),
            relativeRotation.GetZ() - absoluteRotation.GetZ());
        IRVector3 position = new IRVector3(
            absolutePosition.GetX() + (float)(distance * Math.Cos(rotation.GetZ() * toRadian) * Math.Sin(rotation.GetY() * toRadian)), 
            absolutePosition.GetY() + (float)(distance * Math.Sin(rotation.GetZ() * toRadian)), 
            absolutePosition.GetZ() + (float)(distance * Math.Cos(rotation.GetZ() * toRadian) * Math.Cos(rotation.GetY() * toRadian)));

        return new IRVectorTransform(position, rotation);
    }
}