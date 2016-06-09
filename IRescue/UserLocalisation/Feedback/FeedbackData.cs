// <copyright file="FeedbackData.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Feedback
{
    /// <summary>
    /// Struct for the different properties of feedback.
    /// </summary>
    /// <typeparam name="T">The type of the feedback data</typeparam>
    public struct FeedbackData<T>
    {
        /// <summary>
        /// The data.
        /// </summary>
        public T Data;

        /// <summary>
        /// The standard deviation of the data.
        /// </summary>
        public float Stddev;

        /// <summary>
        /// The time stamp for which this data was calculated in milliseconds.
        /// </summary>
        public long TimeStamp;
    }
}