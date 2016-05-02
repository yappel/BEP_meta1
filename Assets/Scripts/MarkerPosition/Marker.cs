/// <summary>
///   Class that defines a known marker.
/// </summary>
public class Marker {
    private int id;
    private Vector3 location;

    /// <summary>
    ///   Create a Marker.
    /// </summary>
    /// <param name="id">id of the Marker</param>
    /// <param name="x">x value</param>
    /// <param name="y">y value</param>
    /// <param name="z">z value</param>
    public Marker(int id, int x, int y, int z)
    {
        this.id = id;
        location = new Vector3(x, y, z);
    }

    /// <summary>
    ///  Return the id of the Marker.
    /// </summary>
    /// <returns>int</returns>
    public int GetId()
    {
        return id;
    }

    /// <summary>
    ///   Return the location of the Marker.
    /// </summary>
    /// <returns>Vector3</returns>
    public Vector3 GetLocation()
    {
        return location;
    }
}
