/// <summary>
///   Abstract class for user localisation. Every child will implement their own filter.
/// </summary>
public abstract class AbstractUserLocalisation {
    // Position coordinates.
    private Vector3 location;

    // Location coordinates.
    private Vector3 position;

    /// <summary>
    ///   Calculate the new location of the user based on the visible markers and accelorometer data.
    /// </summary>
    /// <param name="predictions">Predicted locations based on IMU data and visible markers.</param>
    public abstract void ProcessLocation(Vector4[] predictions);

    /// <summary>
    ///  Return a Vector3 with the calculated position.
    /// </summary>
    /// <returns>Vector3</returns>
    public Vector3 GetPosition()
    {
        return position;
    }

    /// <summary>
    ///  Return a Vector3 with the calculated rotation.
    /// </summary>
    /// <returns>Vector3</returns>
    public Vector3 GetRotation()
    {
        return location;
    }
}