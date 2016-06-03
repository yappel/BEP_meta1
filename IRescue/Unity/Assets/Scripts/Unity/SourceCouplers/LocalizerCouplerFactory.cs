// <copyright file="LocalizerFactory.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.SourceCouplers
{
    using System;
    using Enums;

    using IRescue.UserLocalisation;

    /// <summary>
    ///  Factory to create a AbstractLocalizerCoupler.
    /// </summary>
    public static class LocalizerCouplerFactory
    {
        /// <summary>
        ///  Initializes the coupler
        /// </summary>
        /// <param name="localizer">Enum of the user filter name</param>
        /// <returns>The localizer coupler</returns>
        public static AbstractLocalizerCoupler Get(AbstractUserLocalizer localizer)
        {
            switch (localizer.GetType())
            {
                case Filters.Particle:
                    return new ParticleFilterCoupler();
                default:
                    throw new ArgumentException(string.Format("{0} is not an existing localizer filter", localizer), "filter");
            }
        }
    }
}
