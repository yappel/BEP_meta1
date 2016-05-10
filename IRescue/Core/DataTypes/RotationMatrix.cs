// <copyright file="RotationMatrix.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.DataTypes
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Single;
    using MathNet.Numerics.LinearAlgebra.Storage;
    using static MathNet.Numerics.Trig;

    public class RotationMatrix : DenseMatrix
    {
        public RotationMatrix() : base(3, 3)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roll">The rotation around the x axis in degrees.</param>
        /// <param name="pitch">The rotiation around the y axis in degrees.</param>
        /// <param name="yaw">The rotation around the z axis in degrees.</param>
        public RotationMatrix(float roll, float pitch, float yaw) : base(3, 3, CreateMatrixVector(roll, pitch, yaw))
        {
        }

        private static Single[] CreateMatrixVector(float roll, float pitch, float yaw)
        {
            double c = DegreeToRadian(roll);
            double b = DegreeToRadian(pitch);
            double a = DegreeToRadian(yaw);
            float[] rot =
            {
                (float) (Cos(a)*Cos(b)), (float) (Sin(a)*Cos(b)), (float) (-1*Sin(b)),
                (float) (Cos(a)*Sin(b)*Sin(c) - Sin(a)*Cos(c)), (float) (Sin(a)*Sin(b)*Sin(c) + Cos(a)*Cos(c)), (float) (Cos(b)*Sin(c)),
                (float) (Cos(a)*Sin(b)*Cos(c) + Sin(a)*Sin(c)), (float) (Sin(a)*Sin(b)*Cos(c) - Cos(a)*Sin(c)), (float) (Cos(b)*Cos(c))
            };
            return rot;
        }
    }
}
