﻿// <copyright file="UnallocatedMarkerException.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Sensors.Marker
{
    using System;

    /// <summary>
    ///   Exception throws when a markerPose was found that was not initialized.
    /// </summary>
    public class UnallocatedMarkerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnallocatedMarkerException"/> class.
        /// </summary>
        /// <param name="message">Custom message for the Exception.</param>
        public UnallocatedMarkerException(string message)
            : base(message)
        {
        }
    }
}
