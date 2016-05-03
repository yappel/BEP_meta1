// <copyright file="MarkerLocations.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;

/// <summary>
///   Class that knows the location of every marker and can give their attributes.
/// </summary>
public class MarkerLocations
{
    /// <summary>
    ///   Hash table of the markers in the Scene.
    /// </summary>
    private Dictionary<int, Marker> markers;

    /// <summary>
    ///   Initializes a new instance of the MarkerLocations class.
    /// </summary>
    public MarkerLocations()
    {
        // TODO fill the Dictionary.
        this.markers = new Dictionary<int, Marker>();
    }

    /// <summary>
    ///   Return the Marker that has the given id.
    /// </summary>
    /// <param name="id">id of the required marker</param>
    /// <returns>Marker with the id</returns>
    public Marker GetMarker(int id)
    {
        if (this.markers.ContainsKey(id))
        {
            return this.markers[id];
        }
        else 
        {
            throw new UnallocatedMarkerException("Marker with id=" + id + " was tracked but not initialized in the XML");
        }
    }
}
