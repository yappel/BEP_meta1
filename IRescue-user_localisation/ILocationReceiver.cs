// <copyright file="ILocationReceiver.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.UserLocalisation
{
    using Assets.Scripts.Inputsensors;

    /// <summary>
    /// A location receiver is able to register a location source
    /// </summary>
    public interface ILocationReceiver
    {
        /// <summary>
        /// Registers a location source.
        /// </summary>
        /// <param name="source">The location source to register</param>
        void RegisterLocationReceiver(ILocationSource source);
    }
}
