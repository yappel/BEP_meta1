// <copyright file="IUserLocalizer.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation
{
    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Feedback;

    /// <summary>
    /// Determines the pose of the user at a certain timestamp.
    /// </summary>
    public interface IUserLocalizer : IPositionFeedbackProvider, IOrientationFeedbackProvider
    {
        /// <summary>
        ///   Calculates the <see cref="Pose"/> of the user at a given timestamp based on the information stored in the system.
        /// </summary>
        /// <param name="timeStamp">The timestamp of the point in time to calculate the <see cref="Pose"/> at.</param>
        /// <returns>The calculated <see cref="Pose"/></returns>
        Pose CalculatePose(long timeStamp);
    }
}