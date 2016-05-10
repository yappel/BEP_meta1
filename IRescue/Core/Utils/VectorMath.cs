// <copyright file="VectorMath.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Utils
{
    using IRescue.Core.DataTypes;
    using MathNet.Numerics.LinearAlgebra.Single;

    public static class VectorMath
    {
        public static Vector3 RotateVector(Vector3 vector, RotationMatrix transformation)
        {
            DenseVector vec = transformation * vector;
            return new Vector3(vec.Values);
        }

        public static Vector3 RotateVector(Vector3 vector, float roll, float pitch, float yaw)
        {
            return RotateVector(vector, new RotationMatrix(roll, pitch, yaw));
        }


    }
}
