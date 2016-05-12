// <copyright file="AbstractUserLocalizer.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation
{
    using System;
    using System.Collections.Generic;
    using Core.DataTypes;

    /// <summary>
    ///   Abstract class for user localization. Every child will implement their own filter.
    /// </summary>
    public abstract class AbstractUserLocalizer
    {
        /// <summary>
        ///   Calculates the <see cref="Pose"/> of the user at a given timestamp based on the information stored in the system.
        /// </summary>
        /// <param name="timeStamp">The timestamp of the point in time to calculate the <see cref="Pose"/> at.</param>
        public abstract Pose CalculatePose(long timeStamp);

    }
}