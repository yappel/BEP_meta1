// <copyright file="IRVector3.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

/// <summary>
///   Struct to create an independent 3D-Vector.
/// </summary>
public struct IRVector3
{
    /// <summary>
    ///   x,y,z of the Vector
    /// </summary>
    private float x, y, z;

    /// <summary>
    ///   Initializes a new instance of the IRVector3 struct.
    /// </summary>
    /// <param name="x">x value</param>
    /// <param name="y">y value</param>
    /// <param name="z">z value</param>
    public IRVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    ///   Return the X value.
    /// </summary>
    /// <returns>float x location</returns>
    public float GetX()
    {
        return this.x;
    }

    /// <summary>
    ///   Set the X value.
    /// </summary>
    /// <param name="x">x value</param>
    public void SetX(float x)
    {
        this.x = x;
    }

    /// <summary>
    ///   Return the Y value.
    /// </summary>
    /// <returns>float y location</returns>
    public float GetY()
    {
        return this.y;
    }

    /// <summary>
    ///   Set the Y value.
    /// </summary>
    /// <param name="y">y value</param>
    public void SetY(float y)
    {
        this.y = y;
    }

    /// <summary>
    ///   Return the Z value.
    /// </summary>
    /// <returns>float z location</returns>
    public float GetZ()
    {
        return this.z;
    }

    /// <summary>
    ///   Set the Z value.
    /// </summary>
    /// <param name="z">z value</param>
    public void SetZ(float z)
    {
        this.z = z;
    }

    /// <summary>
    /// Adds another vector to this vector.
    /// </summary>
    /// <param name="toadd">The vector3 that will be added to this vector</param>
    public void Add(IRVector3 toadd)
    {
        this.SetX(this.GetX() + toadd.GetX());
        this.SetY(this.GetY() + toadd.GetY());
        this.SetZ(this.GetZ() + toadd.GetX());
    }

    /// <summary>
    /// Multiply all the x y and z with a certain number.
    /// </summary>
    /// <param name="number">The number to multiply with</param>
    /// <returns>The resutling vector</returns>
    public IRVector3 Multiply(float number)
    {
        this.SetX(this.x * number);
        this.SetY(this.y * number);
        this.SetZ(this.z * number);
        return this;
    }

    /// <summary>
    /// Check if an object is equals to this object.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>Boolean stating if the 2 objects are equal</returns>
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        IRVector3 vec = (IRVector3)obj;
        return (this.x == vec.GetX()) && (this.y == vec.GetY()) && (this.z == vec.GetZ());
    }

    /// <summary>
    /// Gets the hash code of this object.
    /// </summary>
    /// <returns>The hash code</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <inheritDoc/>
    public override string ToString()
    {
        return "[" + this.GetX() + " "+ this.GetY() + " " + this.GetZ() + "]";
    }
}