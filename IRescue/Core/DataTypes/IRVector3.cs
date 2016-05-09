// <copyright file="IRVector3.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.DataTypes
{
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
    }
}