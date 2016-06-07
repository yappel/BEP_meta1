using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRescue.UserLocalisation.Feedback
{
    public interface IFeedbackReceiver<T>
    {
        /// <summary>
        /// Notifies the feedback provider that there is new feedback data available.
        /// </summary>
        /// <param name="data">The orientation in Tait-Bryan angles.</param>
        void Notify(FeedbackData<T> data);
    }
}
