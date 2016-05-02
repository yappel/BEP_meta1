/// <summary>
///   Class that knows the location of every marker and can give their attributes.
/// </summary>
public class MarkerPosition {
    // Array of the markers in the Scene.
    private Marker[] markers;

    public MarkerPosition()
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
        return markers[id];
    }
}
