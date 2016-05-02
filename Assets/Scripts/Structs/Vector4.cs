/// <summary>
///   Struct to create a independent 3D-Vector.
/// </summary>
public struct Vector4
{
    private Vector3 location;
    private float weight;

    /// <summary>
    ///   Create a Vector3
    /// </summary>
    /// <param name="x">x value</param>
    /// <param name="y">y value</param>
    /// <param name="z">z value</param>
    /// <param name="weight">z value</param>
    public Vector4(float x, float y, float z, float weight)
    {
        location = new Vector3(x, y, z);
        this.weight = weight;
    }

    /// <summary>
    ///   Return the weight.
    /// </summary>
    /// <returns>float</returns>
    public float GetWeight()
    {
        return weight;
    }

    /// <summary>
    ///   Return the X value.
    /// </summary>
    /// <returns>float</returns>
    public float GetX()
    {
        return location.GetX();
    }

    /// <summary>
    ///   Return the Y value.
    /// </summary>
    /// <returns>float</returns>
    public float GetY()
    {
        return location.GetY();
    }

    /// <summary>
    ///   Return the Z value.
    /// </summary>
    /// <returns>float</returns>
    public float GetZ()
    {
        return location.GetZ();
    }
}