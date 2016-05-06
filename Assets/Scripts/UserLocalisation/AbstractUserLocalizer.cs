// <copyright file="AbstractUserLocalizer.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;

/// <summary>
///   Abstract class for user localization. Every child will implement their own filter.
/// </summary>
public abstract class AbstractUserLocalizer
{
    /// <summary>
    ///   Position coordinates.
    /// </summary>
    protected IRVector3 position;

    /// <summary>
    ///   Rotation coordinates.
    /// </summary>
    protected IRVector3 rotation;

    /// <summary>
    ///   Calculate the current location of the user.
    /// </summary>
    public abstract void CalculateLocation();

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