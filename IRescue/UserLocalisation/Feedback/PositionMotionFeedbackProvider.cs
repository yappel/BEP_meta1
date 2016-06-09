// <copyright file="PositionMotionFeedbackProvider.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Feedback
{
    using System;
    using System.Collections.Generic;

    using IRescue.Core.DataTypes;

    /// <summary>
    /// Provides feedback about the positional motion of the user.
    /// </summary>
    public class PositionMotionFeedbackProvider : IPositionFeedbackReceiver, IVelocityFeedbackProvider
    {
        /// <summary>
        /// List with all registered <see cref="IVelocityFeedbackReceiver"/>s.
        /// </summary>
        private readonly List<IVelocityFeedbackReceiver> velReceivers;

        /// <summary>
        /// The previous received position feedback.
        /// </summary>
        private FeedbackData<Vector3>? previousData;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionMotionFeedbackProvider"/> class.
        /// </summary>
        public PositionMotionFeedbackProvider()
        {
            this.velReceivers = new List<IVelocityFeedbackReceiver>();
        }

        /// <inheritdoc/>
        public void NotifyPositionFeedback(FeedbackData<Vector3> data)
        {
            if ((this.previousData != null) && (data.TimeStamp <= this.previousData.Value.TimeStamp))
            {
                return;
            }

            FeedbackData<Vector3>? velocityFeedback = this.CalculateVelocityFeedback(data);

            if (velocityFeedback != null)
            {
                for (int index = 0; index < this.velReceivers.Count; index++)
                {
                    IVelocityFeedbackReceiver receiver = this.velReceivers[index];
                    receiver.NotifyVelocityFeedback(velocityFeedback.Value);
                }
            }

            this.previousData = data;
        }

        /// <summary>
        /// Registers a feedback receiver so it will be notified when new feedback is available.
        /// </summary>
        /// <param name="receiver">The receiver to register.</param>
        public void RegisterReceiver(IVelocityFeedbackReceiver receiver)
        {
            if (!this.velReceivers.Contains(receiver))
            {
                this.velReceivers.Add(receiver);
            }
        }

        /// <summary>
        /// Unregisters a feedback receiver so it will not be notified when new feedback is available.
        /// </summary>
        /// <param name="receiver">The receiver to unregister.</param>
        public void UnregisterReceiver(IVelocityFeedbackReceiver receiver)
        {
            if (this.velReceivers.Contains(receiver))
            {
                this.velReceivers.Remove(receiver);
            }
        }

        /// <summary>
        /// Calculated the current velocity based on the given data and <see cref="previousData"/>.
        /// </summary>
        /// <param name="data">The newly retrieved position data</param>
        /// <returns>The velocity at the timestamp of the newly received data.</returns>
        private FeedbackData<Vector3>? CalculateVelocityFeedback(FeedbackData<Vector3> data)
        {
            FeedbackData<Vector3>? feedbackData = this.previousData;
            if (feedbackData == null)
            {
                return null;
            }

            long timediff = data.TimeStamp - feedbackData.Value.TimeStamp;
            Vector3 traveledDist = new Vector3();
            data.Data.Subtract(feedbackData.Value.Data, traveledDist);
            Vector3 velocityData = new Vector3();
            traveledDist.Divide(timediff / 1000f, velocityData);
            FeedbackData<Vector3>? res = new FeedbackData<Vector3>
            {
                Data = velocityData,
                TimeStamp = data.TimeStamp,
                Stddev = (float)Math.Sqrt(Math.Pow(data.Stddev, 2) + Math.Pow(feedbackData.Value.Stddev, 2))
            };

            return res;
        }
    }
}