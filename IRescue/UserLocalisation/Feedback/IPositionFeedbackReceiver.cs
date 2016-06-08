// <copyright file="IPoseFeedbackReceiver.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Feedback
{
    using IRescue.Core.DataTypes;

    /// <summary>
    /// Interface for classes able to receive feedback data.
    /// </summary>
    public interface IPositionFeedbackReceiver
    {
        /// <summary>
        /// Notifies the feedback provider that there is new feedback data available.
        /// </summary>
        /// <param name="data">The position in meters in the xyz axis.</param>
        void NotifyPositionFeedback(FeedbackData<Vector3> data);
    }
}
