// <copyright file="ParticleFilterCoupler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using IRescue.UserLocalisation.Particle;
using IRescue.UserLocalisation.Sensors;

namespace Assets.Scripts.Unity.SourceCouplers
{
    /// <summary>
    ///  This class couples sources to a localizer.
    /// </summary>
    public class ParticleFilterCoupler : AbstractLocalizerCoupler
    {
        /// <summary>
        /// The used filter.
        /// </summary>
        private ParticleFilter localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleFilterCoupler"/> class
        /// </summary>
        public ParticleFilterCoupler(ParticleFilter localizer)
        {
            this.localizer = localizer;
        }

        /// <summary>
        /// Register a acceleration source 
        /// </summary>
        /// <param name="source">The source to register</param>
        /// <returns>if the source was registered</returns>
        protected override bool RegisterAccelerationReceiver(IAccelerationSource source)
        {
            return false;
        }

        /// <summary>
        /// Register a displacement source
        /// </summary>
        /// <param name="source">The source to register</param>
        /// <returns>if the source was registered</returns>
        protected override bool RegisterDisplacementReceiver(IDisplacementSource source)
        {
            return false;
        }

        /// <summary>
        /// Register a orientation source
        /// </summary>
        /// <param name="source">The source to register</param>
        /// <returns>if the source was registered</returns>
        protected override bool RegisterOrientationReceiver(IOrientationSource source)
        {
            if (source != null)
            {
                this.localizer.AddOrientationSource(source);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Register a position source
        /// </summary>
        /// <param name="source">The source to register</param>
        /// <returns>if the source was registered</returns>
        protected override bool RegisterPositionReceiver(IPositionSource source)
        {
            if (source != null)
            {
                this.localizer.AddPositionSource(source);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Register a velocity source
        /// </summary>
        /// <param name="source">The source to register</param>
        /// <returns>if the source was registered</returns>
        protected override bool RegisterVelocityReceiver(IVelocitySource source)
        {
            return false;
        }
    }
}
