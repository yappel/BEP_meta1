// <copyright file="IRVectorTransform.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

/// <summary>
///   Struct to create a independent 3D-Vector.
/// </summary>
public class IRVectorTransform
{
    /// <summary>
    ///   The location and rotation.
    /// </summary>
    private IRVector3 position, rotation;

    /// <summary>
    ///   Initializes a new instance of the IRVectorTransform class.
    /// </summary>
    /// <param name="position">location value</param>
    /// <param name="rotation">rotation value</param>
    public IRVectorTransform(IRVector3 position, IRVector3 rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    /// <summary>
    ///   Return the position.
    /// </summary>
    /// <returns>Vector3 position</returns>
    public IRVector3 GetPosition()
    {
        return this.position;
    }

    /// <summary>
    ///   Set the position value.
    /// </summary>
    /// <param name="position">position value</param>
    public void SetPosition(IRVector3 position)
    {
        this.position = position;
    }

    /// <summary>
    ///   Return the rotation.
    /// </summary>
    /// <returns>Vector3 position</returns>
    public IRVector3 GetRotation()
    {
        return this.rotation;
    }

    /// <summary>
    ///   Set the rotation value.
    /// </summary>
    /// <param name="rotation">rotation value</param>
    public void SetRotation(IRVector3 rotation)
    {
        this.rotation = rotation;
    }
}