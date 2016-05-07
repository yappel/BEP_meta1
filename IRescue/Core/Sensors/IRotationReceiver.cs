// <copyright file="IRotationReceiver.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Sensors
{
    /// <summary>
    /// A rotation receiver is able to register a rotation source
    /// </summary>
    public interface IRotationReceiver
    {
        /// <summary>
        /// Registers a rotation source.
        /// </summary>
        /// <param name="source">The rotation source to register</param>
        void RegisterRotationSource(IRotationSource source);
    }
}
