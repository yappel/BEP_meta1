// <copyright file="RotationMatrix.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.Core.DataTypes
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Single;

    using static MathNet.Numerics.Trig;

    /// <summary>
    /// Class which is a 3x3 matrix for rotations.
    /// </summary>
    public class RotationMatrix : DenseMatrix
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RotationMatrix"/> class.
        /// Creates an identity 3x3 matrix which corresponds with no rotation around
        /// all 3 axis.
        /// </summary>
        public RotationMatrix()
            : base(3, 3)
        {
            this[0, 0] = 1;
            this[1, 1] = 1;
            this[2, 2] = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotationMatrix"/> class.
        /// Create a 3x3 rotation matrix from the specified rotations around the 3 axes.
        /// </summary>
        /// <param name="xRotation">The rotation around the x axis in degrees.</param>
        /// <param name="yRotation">The rotation around the y axis in degrees.</param>
        /// <param name="zRotation">The rotation around the z axis in degrees.</param>
        public RotationMatrix(float xRotation, float yRotation, float zRotation)
            : base(3, 3, CreateMatrixVector(xRotation, yRotation, zRotation))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotationMatrix"/> class.
        /// Create a 3x3 rotation matrix from a quaternion.
        /// </summary>
        /// <param name="q1">The first or w parameter of the quaternion</param>
        /// <param name="q2">The second or x parameter of the quaternion</param>
        /// <param name="q3">The third or y parameter of the quaternion</param>
        /// <param name="q4">The fourth or z parameter of the quaternion</param>
        public RotationMatrix(float q1, float q2, float q3, float q4)
            : base(3, 3, CreateMatrixVector(q1, q2, q3, q4))
        {
        }

        /// <summary>
        /// Gets the XYZ Tait-bryan angles of this rotationmatrix.
        /// </summary>
        public Vector3 EulerAngles => new Vector3(
            (float)RadianToDegree(Math.Atan2(this[2, 1], this[2, 2])),
            (float)RadianToDegree(Math.Atan2(-1 * this[2, 0], Math.Sqrt(Math.Pow(this[2, 1], 2) + Math.Pow(this[2, 2], 2)))),
            (float)RadianToDegree(Math.Atan2(this[1, 0], this[0, 0])));

        private static float[] CreateMatrixVector(float q1, float q2, float q3, float q4)
        {
            ////Mathsource: http://www.ee.ucr.edu/~farrell/AidedNavigation/D_App_Quaternions/Rot2Quat.pdf
            return new[]
                       {
                           (q1 * q1) + (q2 * q2) - (q3 * q3) - (q4 * q4),    2 * ((q2 * q3) + (q1 * q4)),                           2 * ((q2 * q4) - (q1 * q3)),
                           2 * ((q2 * q3) - (q1 * q4)),                          (q1 * q1) - (q2 * q2) + (q3 * q3) - (q4 * q4),     2 * ((q1 * q2) + (q3 * q4)),
                           2 * ((q1 * q3) + (q2 * q4)),                          2 * ((q3 * q4) - (q1 * q2)),                           (q1 * q1) - (q2 * q2) - (q3 * q3) + (q4 * q4),
                       };
        }

        /// <summary>
        /// Create an array of the rotation matrix from specified rotations around the 3 axes.
        /// </summary>
        /// <param name="xRotation">The rotation around the x axis in degrees.</param>
        /// <param name="yRotation">The rotation around the y axis in degrees.</param>
        /// <param name="zRotation">The rotation around the z axis in degrees.</param>
        /// <returns>Array of floats which can be used by Math.NET to use for matrix.</returns>
        private static float[] CreateMatrixVector(float xRotation, float yRotation, float zRotation)
        {
            double c = DegreeToRadian(xRotation);
            double b = DegreeToRadian(yRotation);
            double a = DegreeToRadian(zRotation);
            float[] rot =
                {
                    (float)(Cos(a) * Cos(b)), (float)(Sin(a) * Cos(b)), (float)(-1 * Sin(b)),
                    (float)((Cos(a) * Sin(b) * Sin(c)) - (Sin(a) * Cos(c))), (float)((Sin(a) * Sin(b) * Sin(c)) + (Cos(a) * Cos(c))), (float)(Cos(b) * Sin(c)),
                    (float)((Cos(a) * Sin(b) * Cos(c)) + (Sin(a) * Sin(c))),  (float)((Sin(a) * Sin(b) * Cos(c)) - (Cos(a) * Sin(c))), (float)(Cos(b) * Cos(c))
                };
            return rot;
        }
    }
}