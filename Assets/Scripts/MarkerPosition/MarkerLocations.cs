// <copyright file="MarkerLocations.cs" company="Delft Universite of Technology">
// Copyright (c) Delft Universite of Technology. All rights reserved.
// </copyright>

/// <summary>
///   Class that knows the location of every marker and can give their attributes.
/// </summary>
public class MarkerLocations
{
    /// <summary>
    ///   Array of the markers in the Scene.
    /// </summary>
    private Marker[] markers;

    /// <summary>
    ///   Initializes a new instance of the MarkerLocations class.
    /// </summary>
    public MarkerLocations()
    {
        // TODO intialise the markers array.
    }

    /// <summary>
    ///   Return the Marker that has the given id.
    /// </summary>
    /// <param name="id">id of the required marker</param>
    /// <returns>Marker with the id</returns>
    public Marker GetMarker(int id)
    {
        // TODO check if key exists.
        return this.markers[id];
    }
}
