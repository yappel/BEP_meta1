// <copyright file="LocalizerFactory.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.SourceCouplers
{
    using System;
    using Enums;

    using IRescue.UserLocalisation;
    using IRescue.UserLocalisation.Particle;

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
        public static AbstractLocalizerCoupler Get(IUserLocalizer localizer)
        {
            switch (localizer.GetType().ToString())
            {
                case "ParticleFilter":
                    return new ParticleFilterCoupler((ParticleFilter)localizer);
                default:
                    throw new ArgumentException(string.Format("{0} is not an existing localizer filter", localizer.GetType()), nameof(localizer));
            }
        }
    }
}
