// <copyright file="Quaternion.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.Core.Datatypes
{
    using System;

    using IRescue.Core.DataTypes;

    using MathNet.Numerics;

    /// <summary>
    /// Represents a quaternion.
    /// </summary>
    public class Quaternion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> class.
        /// </summary>
        /// <param name="eulerx">The Tait-Bryan x angle in radian</param>
        /// <param name="eulery">The Tait-Bryan y angle in radian</param>
        /// <param name="eulerz">The Tait-Bryan z angle in radian</param>
        public Quaternion(float eulerx, float eulery, float eulerz)
        {
            double c1 = Math.Cos(eulery / 2);
            double c2 = Math.Cos(eulerx / 2);
            double c3 = Math.Cos(eulerz / 2);
            double s1 = Math.Sin(eulery / 2);
            double s2 = Math.Sin(eulerx / 2);
            double s3 = Math.Sin(eulerz / 2);
            this.W = (float)((c1 * c2 * c3) - (s1 * s2 * s3));
            this.X = (float)((s1 * s2 * c3) + (c1 * c2 * s3));
            this.Y = (float)((s1 * c2 * c3) + (c1 * s2 * s3));
            this.Z = (float)((c1 * s2 * c3) - (s1 * c2 * s3));
            this.EulerAnglesRadian = this.CalcEulerAngles();
            this.EulerAnglesDegree = new Vector3(this.EulerAnglesRadian.Map(f => (float)Trig.RadianToDegree(f)).ToArray());
        }

        /// <summary>
        /// Gets this quaternion converted to Tait-Bryan angles in degrees.
        /// </summary>
        public Vector3 EulerAnglesDegree { get; private set; }

        /// <summary>
        /// Gets this quaternion converted to Tait-Bryan angles in radian.
        /// </summary>
        public Vector3 EulerAnglesRadian { get; }

        /// <summary>
        /// Gets the w coordinate.
        /// </summary>
        public float W { get; }

        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Gets the z coordinate.
        /// </summary>
        public float Z { get; }

        private Vector3 CalcEulerAngles()
        {
            double y = Math.Atan2((2 * this.Y * this.W) - (2 * this.X * this.Z), 1 - (2 * Math.Pow(this.Y, 2)) - (2 * Math.Pow(this.Z, 2)));
            double x = Math.Asin((2 * this.X * this.Y) + (2 * this.Z * this.W));
            double z = Math.Atan2((2 * this.X * this.W) - (2 * this.Y * this.Z), 1 - (2 * Math.Pow(this.X, 2)) - (2 * Math.Pow(this.Z, 2)));
            return new Vector3((float)x, (float)y, (float)z);
        }
    }
}