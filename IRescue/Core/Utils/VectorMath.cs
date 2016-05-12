// <copyright file="VectorMath.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Utils
{
    using IRescue.Core.DataTypes;
    using MathNet.Numerics.LinearAlgebra.Single;

    /// <summary>
    /// Static utility class for common vector operations.
    /// </summary>
    public static class VectorMath
    {
        /// <summary>
        /// Rotates the specified vector with the specified rotation matrix. Stores the result in the specified vector.
        /// </summary>
        /// <param name="vector">The vector to be rotated.</param>
        /// <param name="transformation">The rotation matrix to rotate the vector with.</param>
        public static void RotateVector(Vector3 vector, RotationMatrix transformation)
        {
            RotateVector(vector, transformation, vector);
        }

        /// <summary>
        /// Rotates the specified vector with the specified pitch, yaw and roll. Stores the result in the supplied vector.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="xRotation">The x axis rotation in degrees.</param>
        /// <param name="yRotation">The y axis rotation in degrees.</param>
        /// <param name="zRotation">The z axis rotation in degrees.</param>
        public static void RotateVector(Vector3 vector, float xRotation, float yRotation, float zRotation)
        {
            RotateVector(vector, new RotationMatrix(xRotation, yRotation, zRotation), vector);
        }

        /// <summary>
        /// Rotates the specified vector with the specified rotation matrix and stores the result in result vector.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The rotation matrix.</param>
        /// <param name="result">Vector where the result is stored.</param>
        public static void RotateVector(Vector3 vector, RotationMatrix rotation, Vector3 result)
        {
            rotation.Multiply(vector, result);
        }

        /// <summary>
        /// Rotates the specified vector with the specified pitch, yaw and roll, and stores the result in the result vector.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="xRotation">The x axis rotation in degrees.</param>
        /// <param name="yRotation">The y axis rotation in degrees.</param>
        /// <param name="zRotation">The z axis rotation in degrees.</param>
        /// <param name="result">The vector where the result is stored.</param>
        public static void RotateVector(Vector3 vector, float xRotation, float yRotation, float zRotation, Vector3 result)
        {
            RotateVector(vector, new RotationMatrix(xRotation, yRotation, zRotation), result);
        }
    }
}
