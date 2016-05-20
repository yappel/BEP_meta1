// <copyright file="Measurement.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.DataTypes
{
    using Distributions;

    /// <summary>
    ///     Class which holds a measurement taken at a specific time with a standard deviation for the measurement.
    /// </summary>
    /// <typeparam name="T">The type of the taken measurement.</typeparam>
    public class Measurement<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Measurement{T}" /> class.
        /// </summary>
        /// <param name="data">The measurement.</param>
        /// <param name="timeStamp">The time stamp at which the measurement was taken.</param>
        /// <param name="disttype">The type of distribution that describes the spread of the measurement.</param>
        public Measurement(T data, long timeStamp, IDistribution disttype)
        {
            this.Data = data;
            this.TimeStamp = timeStamp;
            this.DistributionType = disttype;
        }

        /// <summary>
        ///     Gets or sets the standard deviation of the measurement.
        /// </summary>
        public float Std { get; set; }

        /// <summary>
        ///     Gets or sets the time stamp at which the measurement was taken.
        /// </summary>
        public long TimeStamp { get; set; }

        /// <summary>
        ///     Gets or sets the actual measurement.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        ///     Gets or sets the type of distribution that describes the spread of the measurement.
        /// </summary>
        public IDistribution DistributionType { get; set; }
    }
}