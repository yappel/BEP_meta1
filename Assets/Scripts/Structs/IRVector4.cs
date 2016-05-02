// <copyright file="IRVector4.cs" company="Delft Universite of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

/// <summary>
///   Struct to create a independent 3D-Vector.
/// </summary>
public class IRVector4 : IRVector3
{
    /// <summary>
    ///   The weight of a location.
    /// </summary>
    private float weight;

    /// <summary>
    ///   Initializes a new instance of the IRVector4 class.
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="z">z coordinate</param>
    /// <param name="weight">z value</param>
    public IRVector4(float x, float y, float z, float weight) : base(x, y, z)
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