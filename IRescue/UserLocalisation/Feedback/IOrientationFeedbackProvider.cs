// <copyright file="IOrientationFeedbackProvider.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Feedback
{
    /// <summary>
    /// Provides feedback about the orientation in Tait-Bryan angles.
    /// </summary>
    public interface IOrientationFeedbackProvider
    {
        /// <summary>
        /// Registers a feedback receiver so it will be notified when new feedback is available.
        /// The feedback is provided in the form of Tait-Bryan angles.
        /// </summary>
        /// <param name="receiver">The receiver to register.</param>
        void RegisterReceiver(IOrientationFeedbackReceiver receiver);

        /// <summary>
        /// Unregisters a feedback receiver so it will not be notified when new feedback is available.
        /// </summary>
        /// <param name="receiver">The receiver to unregister.</param>
        void UnregisterReceiver(IOrientationFeedbackReceiver receiver);
    }
}