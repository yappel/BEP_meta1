// <copyright file="LocalizerFactory.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Assets.Scripts.Unity.SourceCouplers
{
    using System;
    using Enums;
    using IRescue.UserLocalisation.Particle;

    /// <summary>
    ///  Factory to create a AbstractLocalizerCoupler.
    /// </summary>
    public static class LocalizerFactory
    {
        /// <summary>
        ///  Initializes the coupler
        /// </summary>
        /// <param name="localizer">Enum of the user filter name</param>
        /// <returns>The localizer coupler</returns>
        public static AbstractLocalizerCoupler Get(Filters localizer)
        {
            switch (localizer)
            {
                case Filters.MonteCarlo:
                    //TODO NOT THE WAY IT SHOULD BE
                    return new MonteCarloCoupler(new ParticleFilter(new double[] { 300, 200, 300, 360, 360, 360},40,0.5));
                default:
                    throw new ArgumentException(string.Format("{0} is not an existing localizer filter", localizer), "filter");
            }
        }
    }
}
