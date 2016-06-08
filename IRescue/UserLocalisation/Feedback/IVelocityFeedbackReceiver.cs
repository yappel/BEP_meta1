// <copyright file="IVelocityFeedbackReceiver.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Feedback
{
    using IRescue.Core.DataTypes;

    /// <summary>
    /// Interface for classes able to receive feedback data.
    /// </summary>
    public interface IVelocityFeedbackReceiver
    {
        /// <summary>
        /// Notifies the feedback provider that there is new feedback data available.
        /// </summary>
        /// <param name="data">The velocity in m/s in the xyz axis.</param>
        void NotifyVelocityFeedback(FeedbackData<Vector3> data);
    }
}
