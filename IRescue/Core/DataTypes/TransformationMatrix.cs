﻿// <copyright file="TransformationMatrix.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Datatypes
{
    using MathNet.Numerics.LinearAlgebra.Single;
    using static MathNet.Numerics.Trig;

    /// <summary>
    /// Class which is a 4x4 transformation matrix.
    /// </summary>
    public class TransformationMatrix : DenseMatrix
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationMatrix"/> class.
        /// Creates an empty 4x4 matrix which corresponds with no rotation around 
        /// all 3 axis and no translation on the 3 axis. Defaults w to 1.
        /// </summary>
        public TransformationMatrix() : base(4, 4)
        {
            this[3, 3] = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationMatrix"/> class.
        /// Creates a 4x4 transformation matrix which corresponds to a rotation followed by a translation.
        /// </summary>
        /// <param name="xt">X axis translation.</param>
        /// <param name="yt">Y axis translation.</param>
        /// <param name="zt">Z axis translation.</param>
        /// <param name="xr">X axis rotation in degrees.</param>
        /// <param name="yr">Y axis rotation in degrees.</param>
        /// <param name="zr">Z axis rotation in degrees.</param>
        /// <param name="w">W value.</param>
        public TransformationMatrix(float xt, float yt, float zt, float xr, float yr, float zr, float w) : base(4, 4, CreateMatrixArray(xt, yt, zt, xr, yr, zr, w))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationMatrix"/> class.
        /// Creates a 4x4 transformation matrix which corresponds to a rotation followed by a translation.
        /// Sets w to default value 1.
        /// </summary>
        /// <param name="xt">X axis translation.</param>
        /// <param name="yt">Y axis translation.</param>
        /// <param name="zt">Z axis translation.</param>
        /// <param name="xr">X axis rotation in degrees.</param>
        /// <param name="yr">Y axis rotation in degrees.</param>
        /// <param name="zr">Z axis rotation in degrees.</param>
        public TransformationMatrix(float xt, float yt, float zt, float xr, float yr, float zr) : this(xt, yt, zt, xr, yr, zr, 1)
        {
        }

        /// <summary>
        /// Create an array from the specified rotation, translation and w values.
        /// </summary>
        /// <param name="xt">X axis translation.</param>
        /// <param name="yt">Y axis translation.</param>
        /// <param name="zt">Z axis translation.</param>
        /// <param name="xr">X axis rotation in degrees.</param>
        /// <param name="yr">Y axis rotation in degrees.</param>
        /// <param name="zr">Z axis rotation in degrees.</param>
        /// <param name="w">W value.</param>
        /// <returns>Array which can be used in constructor of DenseMatrix.</returns>
        private static float[] CreateMatrixArray(float xt, float yt, float zt, float xr, float yr, float zr, float w)
        {
            double c = DegreeToRadian(xr);
            double b = DegreeToRadian(yr);
            double a = DegreeToRadian(zr);
            float[] rot =
            {
                (float)(Cos(a) * Cos(b)), (float)(Sin(a) * Cos(b)), (float)(-1 * Sin(b)), xt,
                (float)((Cos(a) * Sin(b) * Sin(c)) - (Sin(a) * Cos(c))), (float)((Sin(a) * Sin(b) * Sin(c)) + (Cos(a) * Cos(c))), (float)(Cos(b) * Sin(c)), yt,
                (float)((Cos(a) * Sin(b) * Cos(c)) + (Sin(a) * Sin(c))), (float)((Sin(a) * Sin(b) * Cos(c)) - (Cos(a) * Sin(c))), (float)(Cos(b) * Cos(c)), zt,
                0, 0, 0, w
            };
            return rot;
        }
    }
}
