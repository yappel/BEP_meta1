// <copyright file="Quaternion.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.Core.DataTypes
{
    using System;
    using System.Linq;

    using MathNet.Numerics;

    /// <summary>
    /// Represents a quaternion.
    /// </summary>
    public class Quaternion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> class.
        /// Creates quaternion based equivalent to a rotation matrix.
        /// </summary>
        /// <param name="rm">The rotation matrix.</param>
        public Quaternion(RotationMatrix rm)
        {
            float[] methods =
                {
                    rm[0, 0] + rm[1, 1] + rm[2, 2],
                    rm[0, 0] - rm[1, 1] - rm[2, 2],
                    ((-1 * rm[0, 0]) + rm[1, 1]) - rm[2, 2],
                    ((-1 * rm[0, 0]) - rm[1, 1]) + rm[2, 2]
                };

            switch (methods.ToList().IndexOf(methods.Max()))
            {
                case 0:
                    this.Diagonal1Solution(rm);
                    break;
                case 1:
                    this.Diagonal2Solution(rm);
                    break;
                case 2:
                    this.Diagonal3Solution(rm);
                    break;
                case 3:
                    this.Diagonal4Solution(rm);
                    break;
            }

            this.EulerAnglesDegree = this.CalcEulerAngles();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> class.
        /// Creates the quaternion w + xi + yj + zk.
        /// </summary>
        /// <param name="w">The w value of the quaternion. Sometimes called q0.</param>
        /// <param name="x">The x value of the quaternion. Sometimes called q1.</param>
        /// <param name="y">The y value of the quaternion. Sometimes called q2.</param>
        /// <param name="z">The z value of the quaternion. Sometimes called q3.</param>
        public Quaternion(float w, float x, float y, float z)
        {
            this.W = w;
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.EulerAnglesDegree = this.CalcEulerAngles();
        }

        /// <summary>
        /// Gets this quaternion converted to Tait-Bryan angles in degrees.
        /// </summary>
        public Vector3 EulerAnglesDegree { get; private set; }

        /// <summary>
        /// Gets the w coordinate.
        /// </summary>
        public float W { get; private set; }

        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// Gets the z coordinate.
        /// </summary>
        public float Z { get; private set; }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            Quaternion other = (Quaternion)obj;
            return (this.W == other.W && this.X == other.X && this.Y == other.Y && this.Z == other.Z) ||
                   (this.W == -other.W && this.X == -other.X && this.Y == -other.Y && this.Z == -other.Z);
        }

        /// <summary>
        /// Checks if another <see cref="Quaternion"/> is almost equal to this quaternion.
        /// </summary>
        /// <param name="obj">The quaternion to compare against</param>
        /// <param name="epsilon">The maximum amount the values may differ to still be considered equal.</param>
        /// <returns>True if the given <see cref="Quaternion"/> is equal to this quaternion.</returns>
        public bool Equals(Quaternion obj, float epsilon)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            Quaternion other = (Quaternion)obj;
            return (Math.Abs(this.W - other.W) <= epsilon
                && Math.Abs(this.X - other.X) <= epsilon
                && Math.Abs(this.Y - other.Y) <= epsilon
                && Math.Abs(this.Z - other.Z) <= epsilon) ||
                (Math.Abs(this.W + other.W) <= epsilon
                && Math.Abs(this.X + other.X) <= epsilon
                && Math.Abs(this.Y + other.Y) <= epsilon
                && Math.Abs(this.Z + other.Z) <= epsilon);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.W.GetHashCode() + this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode();
        }

        /// <summary>
        /// Creates string representation of the the quaternion.
        /// </summary>
        /// <returns>"[w x y z]"</returns>
        public override string ToString()
        {
            return $"[{this.W}, {this.X}, {this.Y}, {this.Z}]";
        }

        private Vector3 CalcEulerAngles()
        {
            return new RotationMatrix(this.W, this.X, this.Y, this.Z).EulerAngles;
        }

        /// <summary>
        /// See http://www.ee.ucr.edu/~farell/AidedNavigation/D_App_Quaternions/Rot2Quat.pdf
        /// </summary>
        /// <param name="rm">The rotation matrix</param>
        private void Diagonal1Solution(RotationMatrix rm)
        {
            this.W = (float)(0.5 * Math.Sqrt(1 + rm[0, 0] + rm[1, 1] + rm[2, 2]));
            this.X = (rm[2, 1] - rm[1, 2]) / (4 * this.W);
            this.Y = (rm[0, 2] - rm[2, 0]) / (4 * this.W);
            this.Z = (rm[1, 0] - rm[0, 1]) / (4 * this.W);
        }

        /// <summary>
        /// See http://www.ee.ucr.edu/~farell/AidedNavigation/D_App_Quaternions/Rot2Quat.pdf
        /// </summary>
        /// <param name="rm">The rotation matrix</param>
        private void Diagonal2Solution(RotationMatrix rm)
        {
            this.X = (float)(0.5 * Math.Sqrt((1 + rm[0, 0]) - rm[1, 1] - rm[2, 2]));
            this.W = (rm[2, 1] - rm[1, 2]) / (4 * this.X);
            this.Y = (rm[0, 1] + rm[1, 0]) / (4 * this.X);
            this.Z = (rm[0, 2] + rm[2, 0]) / (4 * this.X);
        }

        /// <summary>
        /// See http://www.ee.ucr.edu/~farell/AidedNavigation/D_App_Quaternions/Rot2Quat.pdf
        /// </summary>
        /// <param name="rm">The rotation matrix</param>
        private void Diagonal3Solution(RotationMatrix rm)
        {
            this.Y = (float)(0.5 * Math.Sqrt(((1 - rm[0, 0]) + rm[1, 1]) - rm[2, 2]));
            this.W = (rm[0, 2] - rm[2, 0]) / (4 * this.Y);
            this.X = (rm[0, 1] + rm[1, 0]) / (4 * this.Y);
            this.Z = (rm[1, 2] + rm[2, 1]) / (4 * this.Y);
        }

        /// <summary>
        /// See http://www.ee.ucr.edu/~farell/AidedNavigation/D_App_Quaternions/Rot2Quat.pdf
        /// </summary>
        /// <param name="rm">The rotation matrix</param>
        private void Diagonal4Solution(RotationMatrix rm)
        {
            this.Z = (float)(0.5 * Math.Sqrt((1 - rm[0, 0] - rm[1, 1]) + rm[2, 2]));
            this.W = (rm[1, 0] - rm[0, 1]) / (4 * this.Z);
            this.X = (rm[0, 2] + rm[2, 0]) / (4 * this.Z);
            this.Y = (rm[1, 2] + rm[2, 1]) / (4 * this.Z);
        }
    }
}