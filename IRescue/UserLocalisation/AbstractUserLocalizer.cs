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
        /// Initializes a new instance of the <see cref="AbstractUserLocalizer"/> class.
        /// </summary>
        /// <param name="fieldsize">The maximum and minimum values of the positions</param>
        protected AbstractUserLocalizer(FieldSize fieldsize)
        {
            this.Fieldsize = fieldsize;
        }

        /// <summary>
        /// Gets or sets the size of the playfield where to locate the user in.
        /// </summary>
        public FieldSize Fieldsize { get; set; }

        /// <summary>
        ///   Calculates the <see cref="Pose"/> of the user at a given timestamp based on the information stored in the system.
        /// </summary>
        /// <param name="timeStamp">The timestamp of the point in time to calculate the <see cref="Pose"/> at.</param>
        /// <returns>The calculated <see cref="Pose"/></returns>
        public abstract Pose CalculatePose(long timeStamp);
    }
}