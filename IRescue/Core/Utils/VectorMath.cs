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
                throw new ArgumentException("Length of vector is not 2 or all values are 0");
            }

            return (float)Trig.RadianToDegree(Math.Atan2(vector[1], vector[0]));
        }

        /// <summary>
        /// Converts an angle to a 2d vector that points in the direction of the angle.
        /// </summary>
        /// <param name="angle">The angle in degrees</param>
        /// <returns>A 2d vector</returns>
        public static Vector<float> AngleToVector(float angle)
        {
            return new DenseVector(new[] { (float)Math.Cos(Trig.DegreeToRadian(angle)), (float)Math.Sin(Trig.DegreeToRadian(angle)) });
        }

        /// <summary>
        /// Changes the values of the given vector so that the vector has a certain length.
        /// </summary>
        /// <param name="vector">The vector to change the length of.</param>
        /// <param name="desiredLength">The desired length.</param>
        public static void SetLength(Vector<float> vector, float desiredLength)
        {
            double currentLength = vector.L2Norm();
            vector.Multiply((float)(desiredLength / currentLength), vector);
        }

        /// <summary>
        /// Converts an angle to a 2d vector with a certain length that points in the direction of the angle.
        /// </summary>
        /// <param name="angle">The angle in degrees</param>
        /// <param name="length">The desired length of the vector</param>
        /// <returns>A 2d vector</returns>
        public static Vector<float> AngleToVector(float angle, float length)
        {
            Vector<float> res = AngleToVector(angle);
            SetLength(res, length);
            return res;
        }

        /// <summary>
        /// Normalizes xyz Tait-Bryan angles.
        /// </summary>
        /// <param name="data">The xyz Tait-Bryan angles</param>
        /// <returns>The same or equivalent Tait-Bryan angles.</returns>
        public static Vector3 Normalize(Vector3 data)
        {
            return new Quaternion(new RotationMatrix(data.X, data.Y, data.Z)).EulerAnglesDegree;
        }
    }
}