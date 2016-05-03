// <copyright file="IRVectorDeviation.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

/// <summary>
///   StandardDeviation contains the position, rotation and standard deviation of a location.
/// </summary>
public class IRVectorDeviation : IRVectorTransform
{
    /// <summary>
    ///   The standard deviation of a prediction of a location.
    /// </summary>
    private float standardDeviation;

    /// <summary>
    ///   Initializes a new instance of the IRVectorDeviation class.
    /// </summary>
    /// <param name="position">Vector of the position</param>
    /// <param name="rotation">Vector of the rotation</param>
    /// <param name="standardDeviation">Standard deviation of the prediction</param>
    public IRVectorDeviation(IRVector3 position, IRVector3 rotation, float standardDeviation) : base(position, rotation)
    {
        this.standardDeviation = standardDeviation;
    }

    /// <summary>
    ///   Return the standard deviation.
    /// </summary>
    /// <returns>float the standard deviation</returns>
    public float GetStandardDeviation()
    {
        return this.standardDeviation;
    }

    /// <summary>
    ///   Set the standard deviation value.
    /// </summary>
    /// <param name="standardDeviation">new standard deviation</param>
    public void SetStandardDeviation(float standardDeviation)
    {
        this.standardDeviation = standardDeviation;
    }
}