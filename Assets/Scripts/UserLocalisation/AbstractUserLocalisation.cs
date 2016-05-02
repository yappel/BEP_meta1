// <copyright file="AbstractUserLocalisation.cs" company="Delft Universite of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

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
    /// <param name="predictions">Predicted locations based on IMU data and visible markers.</param>
    public abstract void ProcessLocation(IRVector4[] predictions);

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