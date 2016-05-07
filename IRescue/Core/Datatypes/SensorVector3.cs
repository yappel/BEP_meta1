// <copyright file="SensorVector3.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Datatypes
{
    /// <summary>
    ///   A vector of size 3 with a standardDeviation to represent the accuracy of the values
    /// </summary>
    public struct SensorVector3
    {
        /// <summary>
        ///   Gets or sets the standard deviation of the measured data.
        /// </summary>
        private float StandardDeviation { get; set; }

        /// <summary>
        ///     Gets or sets the y element of the vector
        /// </summary>
        private float X { get; set; }

        /// <summary>
        ///     Gets or sets the y element of the vector
        /// </summary>
        private float Y { get; set; }

        /// <summary>
        ///     Gets or sets the z element of the vector
        /// </summary>
        private float Z { get; set; }

        /// <summary>
        ///   Initializes a new instance of the SensorVector3 struct.
        /// </summary>
        /// <param name="x">x value</param>
        /// <param name="y">y value</param>
        /// <param name="z">z value</param>
        /// <param name="standardDeviation">Standard deviation of the prediction</param>
        public SensorVector3(float x, float y, float z, float standardDeviation)
        {
            this.StandardDeviation = standardDeviation;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}