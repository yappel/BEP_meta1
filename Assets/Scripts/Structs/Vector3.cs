/// <summary>
///   Struct to create a independent 3D-Vector.
/// </summary>
public struct Vector3
{
    private float x, y, z;

    /// <summary>
    ///   Create a Vector3
    /// </summary>
    /// <param name="x">x value</param>
    /// <param name="y">y value</param>
    /// <param name="z">z value</param>
    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    ///   Return the X value.
    /// </summary>
    /// <returns>float</returns>
    public float GetX()
    {
        return x;
    }

    /// <summary>
    ///   Return the Y value.
    /// </summary>
    /// <returns>float</returns>
    public float GetY()
    {
        return y;
    }

    /// <summary>
    ///   Return the Z value.
    /// </summary>
    /// <returns>float</returns>
    public float GetZ()
    {
        return z;
    }
}