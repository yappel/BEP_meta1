namespace Assets.Scripts.Inputsensors
{
    /// <summary>
    ///   A LocationSource provides information about the location of the user.
    /// </summary>
    public interface LocationSource
    {
        /// <summary>
        /// Gets the location vector.
        /// </summary>
        /// <returns>A SensorVector3 containing the location in the xyz dimensions in meters and a deviation to specify the certainty of this data</returns>
        SensorVector3 GetLocation();
    }
}
