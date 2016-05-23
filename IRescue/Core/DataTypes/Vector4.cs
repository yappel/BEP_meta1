// <copyright file="Vector4.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.DataTypes
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Single;

    /// <summary>
    /// Vector of length 4 which uses MathNet. Used in vector transformation calculations.
    /// </summary>
    public class Vector4 : DenseVector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class with 4 zeros.
        /// </summary>
        public Vector4() : base(4)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        /// <param name="w">The w value.</param>
        public Vector4(float x, float y, float z, float w) : base(new float[] { x, y, z, w })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class with default value of w=1.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        public Vector4(float x, float y, float z) : this(x, y, z, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class.
        /// </summary>
        /// <param name="vectorValues">Array with the x, y, z and w values</param>
        public Vector4(float[] vectorValues) : base(vectorValues)
        {
            CheckLength(vectorValues);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class.
        /// Copies the X, Y and Z values of the <see cref="Vector3"/> to the new vector.
        /// </summary>
        /// <param name="vector">The vector to copy into the new vector.</param>
        /// <param name="w">The w value.</param>
        public Vector4(Vector3 vector, float w) : this()
        {
            this.SetSubVector(0, 3, vector);
            this[3] = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> class.
        /// Copies the X, Y and Z values of the <see cref="Vector3"/> to the new vector.
        /// Sets the w value to default value 1.
        /// </summary>
        /// <param name="vector">The vector to copy into the new vector.</param>
        public Vector4(Vector3 vector) : this(vector, 1)
        {
        }

        /// <summary>
        /// Gets or sets the X value.
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
        /// Gets or sets the Y value.
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
        /// Gets or sets the Z value.
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
        /// Gets or sets the W value.
        /// </summary>
        public float W
        {
            get
            {
                return this[3];
            }

            set
            {
                this[3] = value;
            }
        }

        /// <summary>
        /// Check the length of the array to be 4.
        /// </summary>
        /// <param name="vectorValues">The array with vector values.</param>
        private static void CheckLength(float[] vectorValues)
        {
            if (vectorValues.Length != 4)
            {
                throw new ArgumentException(string.Format("{0} is not of length 4", vectorValues.Length), "vectorValues");
            }
        }
    }
}
