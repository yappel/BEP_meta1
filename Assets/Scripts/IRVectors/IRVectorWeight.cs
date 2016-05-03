// <copyright file="IRVectorWeight.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

/// <summary>
///   IRVectorWeight contains the position, rotation and weight of a location.
/// </summary>
public class IRVectorWeight : IRVectorTransform
{
    /// <summary>
    ///   The weight of a location.
    /// </summary>
    private float weight;

    /// <summary>
    ///   Initializes a new instance of the IRVectorWeight class.
    /// </summary>
    /// <param name="position">Vector of the position</param>
    /// <param name="rotation">Vector of the rotation</param>
    /// <param name="weight">z value</param>
    public IRVectorWeight(IRVector3 position, IRVector3 rotation, float weight) : base(position, rotation)
    {
        this.weight = weight;
    }

    /// <summary>
    ///   Return the weight.
    /// </summary>
    /// <returns>float the weight</returns>
    public float GetWeight()
    {
        return this.weight;
    }

    /// <summary>
    ///   Set the weight value.
    /// </summary>
    /// <param name="weight">weight value</param>
    public void SetWeigth(float weight)
    {
        this.weight = weight;
    }
}