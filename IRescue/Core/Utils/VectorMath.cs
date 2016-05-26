// <copyright file="VectorMath.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Utils
{
    using System;

    using IRescue.Core.DataTypes;

    using MathNet.Numerics;
    using MathNet.Numerics.LinearAlgebra;
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

        /// <summary>
        /// Converts a 2d vector to an angle.
        /// </summary>
        /// <param name="vector">The 2d vector to convert</param>
        /// <returns>The theta component of the polar coordinate of the vector</returns>
        public static float Vector2ToAngle(Vector<float> vector)
        {
            if ((vector.Count != 2) || ((Math.Abs(vector[0]) < float.Epsilon) && (Math.Abs(vector[1]) < float.Epsilon)))
            {
                throw new ArgumentException("Length of vector is not 2");
            }

            if (Math.Abs(vector[0]) < float.Epsilon)
            {
                return vector[1] < 0 ? 270f : 90f;
            }

            if (Math.Abs(vector[1]) < float.Epsilon)
            {
                return vector[0] < 0 ? 180f : 0f;
            }

            float angletoadd = 0;
            if (vector[0] < 0)
            {
                angletoadd = 180;
            }
            else if (vector[1] < 0)
            {
                angletoadd = 360;
            }

            return (float)(Trig.RadianToDegree(Math.Atan2(vector[1], vector[0])) + angletoadd);
        }

        /// <summary>
        /// Converts an angle to a 2d vector that points in the direction of the angle.
        /// </summary>
        /// <param name="angle">The angle in degrees</param>
        /// <returns>A 2d vector</returns>
        public static Vector AngleToVector(float angle)
        {
            return new DenseVector(new[] { (float)Math.Cos(Trig.DegreeToRadian(angle)), (float)Math.Sin(Trig.DegreeToRadian(angle)) });
        }
    }
}