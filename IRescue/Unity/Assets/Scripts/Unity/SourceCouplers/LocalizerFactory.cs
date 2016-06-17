// <copyright file="LocalizerFactory.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.SourceCouplers
{
    using System;
    using Enums;
    using IRescue.Core.DataTypes;

    /// <summary>
    ///  Factory to create a AbstractLocalizerCoupler.
    /// </summary>
    public static class LocalizerFactory
    {
        /// <summary>
        ///  Initializes the coupler
        /// </summary>
        /// <param name="localizer">Enum of the user filter name</param>
        /// <param name="fieldSize">The preferred size of the game field</param>
        /// <returns>The localizer coupler</returns>
        public static AbstractLocalizerCoupler Get(Filters localizer, FieldSize fieldSize)
        {
            switch (localizer)
            {
                case Filters.Particle:
                    return new ParticleFilterCoupler(fieldSize);
                default:
                    throw new ArgumentException(string.Format("{0} is not an existing localizer filter", localizer), "filter");
            }
        }
    }
}
