// <copyright file="TransformationMatrix.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.DataTypes
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
        /// Creates an identity 4x4 matrix which corresponds with no rotation around 
        /// all 3 axis and no translation on the 3 axis. Defaults w to 1.
        /// </summary>
        public TransformationMatrix() : base(4, 4)
        {
            this[0, 0] = 1;
            this[1, 1] = 1;
            this[2, 2] = 1;
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
        /// Initializes a new instance of the <see cref="TransformationMatrix"/> class.
        /// Creates a 4x4 transformation matrix with rotation and translation specified in 2 <see cref="Vector3"/> objects.
        /// </summary>
        /// <param name="translation">The translation vector to use.</param>
        /// <param name="rotation">The rotation vector to use.</param>
        /// <param name="w">The w value of the transformation matrix.</param>
        public TransformationMatrix(Vector3 translation, Vector3 rotation, float w) : this(translation.X, translation.Y, translation.Z, rotation.X, rotation.Y, rotation.Z, w)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationMatrix"/> class.
        /// Creates a 4x4 transformation matrix with rotation and translation specified in 2 <see cref="Vector3"/> objects.
        /// Sets the w value to default value 1.
        /// </summary>
        /// <param name="translation">The translation vector to use.</param>
        /// <param name="rotation">The rotation vector to use.</param>
        public TransformationMatrix(Vector3 translation, Vector3 rotation) : this(translation.X, translation.Y, translation.Z, rotation.X, rotation.Y, rotation.Z, 1)
        {
        }

        /// <summary>
        /// Returns the rotation matrix that is in the transformation matrix.
        /// </summary>
        /// <returns>The 3x3 rotation matrix which is in the transformation matrix.</returns>
        public RotationMatrix GetRotation()
        {
            RotationMatrix res = new RotationMatrix();
            this.SubMatrix(0, 3, 0, 3).CopyTo(res);
            return res;
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
                (float)(Cos(a) * Cos(b)), (float)(Sin(a) * Cos(b)), (float)(-1 * Sin(b)), 0,
                (float)((Cos(a) * Sin(b) * Sin(c)) - (Sin(a) * Cos(c))), (float)((Sin(a) * Sin(b) * Sin(c)) + (Cos(a) * Cos(c))), (float)(Cos(b) * Sin(c)), 0,
                (float)((Cos(a) * Sin(b) * Cos(c)) + (Sin(a) * Sin(c))), (float)((Sin(a) * Sin(b) * Cos(c)) - (Cos(a) * Sin(c))), (float)(Cos(b) * Cos(c)), 0,
                xt, yt, zt, w
            };
            return rot;
        }
    }
}
