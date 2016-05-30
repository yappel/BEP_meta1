// <copyright file="ParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;
    using IRescue.UserLocalisation.Sensors;

    public class ParticleFilter : IUserLocalizer, IPositionReceiver, IOrientationReceiver, IDisplacementReceiver
    {
        private PositionParticleFilter positionFilter;

        private OrientationParticleFilter orientationFilter;

        public ParticleFilter(int particleAmount, float resampleNoiseSize, FieldSize fieldSize, IParticleGenerator particleGenerator, IResampler resampler, INoiseGenerator noiseGenerator, ISmoother smoother)
        {
            this.positionFilter = new PositionParticleFilter(noiseGenerator, resampleNoiseSize, resampler, particleGenerator, particleAmount, fieldSize, smoother);
            this.orientationFilter = new OrientationParticleFilter(noiseGenerator, resampleNoiseSize, resampler, particleGenerator, particleAmount, smoother);
        }

        /// <summary>
        ///   Calculates the <see cref="Pose"/> of the user at a given timestamp based on the information stored in the system.
        /// </summary>
        /// <param name="timeStamp">The timestamp of the point in time to calculate the <see cref="Pose"/> at.</param>
        /// <returns>The calculated <see cref="Pose"/></returns>
        public Pose CalculatePose(long timeStamp)
        {
            return new Pose(this.positionFilter.Calculate(timeStamp), this.orientationFilter.Calculate(timeStamp));
        }

        /// <summary>
        /// Add a position source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        public void AddPositionSource(IPositionSource source)
        {
            this.positionFilter.AddPositionSource(source);
        }

        /// <summary>
        /// Add a orientation source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        public void AddOrientationSource(IOrientationSource source)
        {
            this.orientationFilter.AddOrientationSource(source);
        }

        /// <summary>
        /// Add a displacement source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        public void AddDisplacementSource(IDisplacementSource source)
        {
            this.positionFilter.AddDisplacementSource(source);
        }
    }
}