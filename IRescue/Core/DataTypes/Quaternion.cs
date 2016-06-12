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
            this.EulerAnglesRadian = new Vector3(this.EulerAnglesDegree.Map(f => (float)Trig.DegreeToRadian(f)).ToArray());
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