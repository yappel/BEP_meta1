// <copyright file="Vector3.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;

namespace IRescue.Core.DataTypes
{
    /// <summary>
    ///  Vector of length 3 which uses MathNet
    /// </summary>
    public class Vector3 : MathNet.Numerics.LinearAlgebra.Single.DenseVector
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="Vector3"/> class.
        /// </summary>
        /// <param name="x">x value</param>
        /// <param name="y">y value</param>
        /// <param name="z">z value</param>
        public Vector3(float x, float y, float z) : base(new float[] { x, y, z })
        {
        }

        /// <summary>
        ///  Initializes a new instance of the Vector3 class.
        /// </summary>
        /// <param name="vectorValues">array with the x, y and z values</param>
        public Vector3(float[] vectorValues) : base(vectorValues)
        {
            CheckLength(vectorValues);
        }

        /// <summary>
        ///  Gets or sets the X value.
        /// </summary>
        public float X
        {
            get
            {
                return this[0];
            }

            set
            {
                this[0] = value;
            }
        }

        /// <summary>
        ///  Gets or sets the Y value.
        /// </summary>
        public float Y
        {
            get
            {
                return this[1];
            }

            set
            {
                this[1] = value;
            }
        }

        /// <summary>
        ///  Gets or sets the Z value.
        /// </summary>
        public float Z
        {
            get
            {
                return this[2];
            }

            set
            {
                this[2] = value;
            }
        }

        /// <summary>
        ///  Check the length of the array to be 3.
        /// </summary>
        /// <param name="vectorValues">The array wit vector values.</param>
        private static void CheckLength(float[] vectorValues)
        {
            if (vectorValues.Length != 3)
            {
                throw new ArgumentException(String.Format("{0} is not of length 3", vectorValues.Length), "vectorValues");
            }
        }
    }
}
