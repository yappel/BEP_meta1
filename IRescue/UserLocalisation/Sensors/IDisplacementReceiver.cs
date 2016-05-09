// <copyright file="IDisplacementReceiver.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Sensors
{
    /// <summary>
    /// Interface which allows a <see cref="IDisplacementSource"/> to be added from which data can be extracted.
    /// </summary>
    public interface IDisplacementReceiver
    {
        /// <summary>
        /// Add a displacement source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        void AddDisplacementSource(IDisplacementSource source);
    }
}
