// <copyright file="Measurement.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.DataTypes
{
    /// <summary>
    /// Class which holds a measurement taken at a specific time with a standard deviation for the measurement.
    /// </summary>
    /// <typeparam name="T">The type of the taken measurement.</typeparam>
    public class Measurement<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Measurement{T}"/> class.
        /// </summary>
        /// <param name="data">The measurement.</param>
        /// <param name="std">The standard deviation of the measurement.</param>
        /// <param name="timeStamp">The time stamp at which the measurement was taken.</param>
        public Measurement(T data, float std, long timeStamp)
        {
            this.Data = data;
            this.Std = std;
            this.TimeStamp = timeStamp;
        }

        /// <summary>
        /// Gets or sets the standard deviation of the measurement.
        /// </summary>
        public float Std { get; set; }

        /// <summary>
        /// Gets or sets the time stamp at which the measurement was taken.
        /// </summary>
        public long TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the actual measurement.
        /// </summary>
        public T Data { get; set; }
    }
}
