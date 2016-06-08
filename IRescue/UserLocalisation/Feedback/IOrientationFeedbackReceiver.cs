// <copyright file="IOrientationFeedbackReceiver.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Feedback
{
    using IRescue.Core.DataTypes;

    /// <summary>
    /// Interface for classes able to receive feedback data.
    /// </summary>
    public interface IOrientationFeedbackReceiver
    {
        /// <summary>
        /// Notifies the feedback provider that there is new feedback data available.
        /// </summary>
        /// <param name="data">The orientation in Tait-Bryan angles.</param>
        void NotifyOrientationFeedback(FeedbackData<Vector3> data);
    }
}
