// <copyright file="Result.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos.Smoothers
{
    using IRescue.Core.DataTypes;

    /// <summary>
    /// Contains a pose with a time stamp.
    /// </summary>
    internal struct Result
    {
        /// <summary>
        /// The pose.
        /// </summary>
        internal Vector3 Vector3;

        /// <summary>
        /// The time stamp of the pose.
        /// </summary>
        internal long TimeStamp;
    }
}