// <copyright file="AbstractUserLocalisation.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;

/// <summary>
///   Abstract class for user localization. Every child will implement their own filter.
/// </summary>
public abstract class AbstractUserLocalisation
{
    /// <summary>
    ///   Position coordinates.
    /// </summary>
    private IRVector3 position;

    /// <summary>
    ///   Rotation coordinates.
    /// </summary>
    private IRVector3 rotation;

    /// <summary>
    ///   Calculate the new location of the user based on the visible markers and accelerometer data.
    /// </summary>
    /// <param name="locations">Predicted locations based on data like visible markers and GPS.</param>
    public abstract void ProcessLocation(List<IRVectorDeviation> locations);

    /// <summary>
    ///  Return a Vector3 with the calculated position.
    /// </summary>
    /// <returns>Vector3 the position</returns>
    public IRVector3 GetPosition()
    {
        return this.position;
    }

    /// <summary>
    ///  Return a Vector 3 with the calculated rotation.
    /// </summary>
    /// <returns> Vector 3 </returns>
    public IRVector3 GetRotation()
    {
        return this.rotation;
    }
}