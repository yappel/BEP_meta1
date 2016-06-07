namespace IRescue.UserLocalisation.Feedback
{
    public interface IVelocityFeedbackProvider
    {

        /// <summary>
        /// Registers a feedback receiver so it will be notified when new feedback is available.
        /// </summary>
        /// <param name="receiver">The receiver to register.</param>
        void RegisterReceiver(IVelocityFeedbackReceiver receiver);

        /// <summary>
        /// Unregisters a feedback receiver so it will not be notified when new feedback is available.
        /// The feedback is provided in the form of velocity in m/s in the xyz axis.
        /// </summary>
        /// <param name="receiver">The receiver to unregister.</param>
        void UnregisterReceiver(IVelocityFeedbackReceiver receiver);
    }
}
