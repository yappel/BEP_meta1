// <copyright file="IVelocityFeedbackReceiver.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Feedback
{
    using IRescue.Core.DataTypes;

    /// <summary>
    /// Interface for classes able to receive feedback data.
    /// </summary>
    public interface IVelocityFeedbackReceiver : IFeedbackReceiver<Vector3>
    {
    }
}
