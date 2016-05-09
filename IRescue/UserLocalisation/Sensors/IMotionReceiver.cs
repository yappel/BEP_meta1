﻿// <copyright file="IMotionReceiver.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors
{
    /// <summary>
    /// A motion receiver is able to register a motion source
    /// </summary>
    public interface IMotionReceiver
    {
        /// <summary>
        /// Registers a motion source.
        /// </summary>
        /// <param name="source">The motion source to register</param>
        void RegisterMotionSource(IMotionSource source);
    }
}