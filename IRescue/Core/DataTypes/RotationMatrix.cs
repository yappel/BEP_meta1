// <copyright file="RotationMatrix.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.Core.DataTypes
{
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