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
    public static IRVector3 GetPosition(Marker absoluteLocation, IRVectorTransform relativeLocation)
    {
        IRVector3 absolutePosition = absoluteLocation.GetPosition();
        IRVector3 absoluteRotation = absoluteLocation.GetRotation();
        IRVector3 relativeRotation = relativeLocation.GetRotation();
        float distance = CalculateDistance(absolutePosition, relativeLocation.GetPosition());
        IRVector3 newLocation = CalculatePosition(absolutePosition, absoluteRotation, relativeRotation, distance);

        Debug.Log("X:" + newLocation.GetX() + ", Y:" + newLocation.GetY() + ", Z:" + newLocation.GetZ());

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
    /// <returns>3D euclidean distance</returns>
    private static IRVector3 CalculatePosition(
        IRVector3 absolutePosition, IRVector3 absoluteRotation, IRVector3 relativeRotation, float distance)
    {
        float newX = (float)Math.Sin(relativeRotation.GetY() * toRadian) * distance;
        float newY = (float)Math.Cos(relativeRotation.GetX() * toRadian) * distance;
        float newZ = (float)Math.Cos(relativeRotation.GetY() * toRadian) * distance;

        return new IRVector3(newX, newY, newZ);
    }
}