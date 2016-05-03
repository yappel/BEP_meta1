// <copyright file="Marker.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

/// <summary>
///   Class that defines a known marker.
/// </summary>
public class Marker
{
    /// <summary>
    ///   The id of the marker according to AprilTags
    /// </summary>
    private int id;

    /// <summary>
    ///   Position of the Marker in the real world.
    /// </summary>
    private IRVector3 position;

    /// <summary>
    ///   Rotation of the Marker in the real world.
    /// </summary>
    private IRVector3 rotation;

    /// <summary>
    ///  Initializes a new instance of the Marker class.
    /// </summary>
    /// <param name="id">id of the Marker</param>
    /// <param name="position">Position of the Marker</param>
    /// <param name="rotation">Rotation of the marker</param>
    public Marker(int id, IRVector3 position, IRVector3 rotation)
    {
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }

    /// <summary>
    ///  Return the id of the Marker.
    /// </summary>
    /// <returns>Integer the id</returns>
    public int GetId()
    {
        return this.id;
    }

    /// <summary>
    ///   Return the position of the Marker.
    /// </summary>
    /// <returns>Vector3 the position</returns>
    public IRVector3 GetPosition()
    {
        return this.position;
    }

    /// <summary>
    ///   Set the rotation of the Marker.
    /// </summary>
    /// <param name="position">Position of the marker</param>
    public void SetPosition(IRVector3 position)
    {
        this.position = position;
    }

    /// <summary>
    ///   Return the rotation of the Marker.
    /// </summary>
    /// <returns>Vector3 the rotation</returns>
    public IRVector3 GetRotation()
    {
        return this.rotation;
    }

    /// <summary>
    ///   Set the rotation of the Marker.
    /// </summary>
    /// <param name="rotation">Rotation of the marker</param>
    public void SetRotation(IRVector3 rotation)
    {
        this.rotation = rotation;
    }
}
