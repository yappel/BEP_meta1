// <copyright file="Marker.cs" company="Delft Universite of Technology">
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
    ///   Location of the Marker in the real world.
    /// </summary>
    private IRVector3 location;

    /// <summary>
    ///  Initializes a new instance of the Marker class.
    /// </summary>
    /// <param name="id">id of the Marker</param>
    /// <param name="x">x value</param>
    /// <param name="y">y value</param>
    /// <param name="z">z value</param>
    public Marker(int id, int x, int y, int z)
    {
        this.id = id;
        this.location = new IRVector3(x, y, z);
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
    ///   Return the location of the Marker.
    /// </summary>
    /// <returns>Vector3 the location</returns>
    public IRVector3 GetLocation()
    {
        return this.location;
    }
}
