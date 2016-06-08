// <copyright file="ParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using System.Collections.Generic;

    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation;
    using IRescue.UserLocalisation.Feedback;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;
    using IRescue.UserLocalisation.Sensors;

    /// <summary>
    /// Determines the pose of the user at certain timestamp using the particle filter method.
    /// </summary>
    public class ParticleFilter : IUserLocalizer, IPositionReceiver, IOrientationReceiver, IDisplacementReceiver
    {
        private PositionParticleFilter positionFilter;

        private OrientationParticleFilter orientationFilter;

        /// <summary>
        /// List with all registered <see cref="IPositionFeedbackReceiver"/>s.
        /// </summary>
        private List<IPositionFeedbackReceiver> posReceivers;

        private List<IOrientationFeedbackReceiver> oriReceivers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleFilter"/> class.
        /// </summary>
        /// <param name="noiseGenerator">See noiseGenerator argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="resampleNoiseSize">See resampleNoiseSize argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="resampler">See resampler argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="particleGenerator">See particleGenerator argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="particleAmount">See particleAmount argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        /// <param name="fieldSize">The dimensions of the area where the user can be.</param>
        /// <param name="smoother">See smoother argument of the constructor of <seealso cref="AbstractParticleFilter"/></param>
        public ParticleFilter(int particleAmount, float resampleNoiseSize, FieldSize fieldSize, IParticleGenerator particleGenerator, IResampler resampler, INoiseGenerator noiseGenerator, ISmoother smoother)
        {
            this.posReceivers = new List<IPositionFeedbackReceiver>();
            this.oriReceivers = new List<IOrientationFeedbackReceiver>();
            this.positionFilter = new PositionParticleFilter(noiseGenerator, resampleNoiseSize, resampler, particleGenerator, particleAmount, fieldSize, smoother);
            this.orientationFilter = new OrientationParticleFilter(noiseGenerator, resampleNoiseSize, resampler, particleGenerator, particleAmount, smoother.Clone());
        }

        /// <summary>
        ///   Calculates the <see cref="Pose"/> of the user at a given timestamp based on the information stored in the system.
        /// </summary>
        /// <param name="timeStamp">The timestamp of the point in time to calculate the <see cref="Pose"/> at.</param>
        /// <returns>The calculated <see cref="Pose"/></returns>
        public Pose CalculatePose(long timeStamp)
        {
            Pose result = new Pose(this.positionFilter.Calculate(timeStamp), this.orientationFilter.Calculate(timeStamp));
            this.NotifyPosFeedbackReceivers(timeStamp, result);
            this.NotifyOriFeedbackReceivers(timeStamp, result);
            return result;
        }

        private void NotifyPosFeedbackReceivers(long timeStamp, Pose result)
        {
            FeedbackData<Vector3> feedback = new FeedbackData<Vector3>()
            {
                Data = result.Position,
                Stddev = this.positionFilter.GetConfidence(),
                TimeStamp = timeStamp
            };

            for (int i = 0; i < this.posReceivers.Count; i++)
            {
                this.posReceivers[i].NotifyPositionFeedback(feedback);
            }
        }

        private void NotifyOriFeedbackReceivers(long timeStamp, Pose result)
        {
            FeedbackData<Vector3> feedback = new FeedbackData<Vector3>()
            {
                Data = result.Orientation,
                Stddev = this.orientationFilter.GetConfidence(),
                TimeStamp = timeStamp
            };

            for (int i = 0; i < this.oriReceivers.Count; i++)
            {
                this.oriReceivers[i].NotifyOrientationFeedback(feedback);
            }
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

        /// <summary>
        /// Registers a feedback receiver so it will be notified when new feedback is available.
        /// </summary>
        /// <param name="receiver">The receiver to register.</param>
        public void RegisterReceiver(IPositionFeedbackReceiver receiver)
        {
            if (this.posReceivers.Contains(receiver))
            {
                return;
            }

            this.posReceivers.Add(receiver);
        }

        /// <summary>
        /// Unregisters a feedback receiver so it will not be notified when new feedback is available.
        /// </summary>
        /// <param name="receiver">The receiver to unregister.</param>
        public void UnregisterReceiver(IPositionFeedbackReceiver receiver)
        {
            if (!this.posReceivers.Contains(receiver))
            {
                return;
            }

            this.posReceivers.Remove(receiver);
        }

        /// <summary>
        /// Registers a feedback receiver so it will be notified when new feedback is available.
        /// </summary>
        /// <param name="receiver">The receiver to register.</param>
        public void RegisterReceiver(IOrientationFeedbackReceiver receiver)
        {
            if (this.oriReceivers.Contains(receiver))
            {
                return;
            }

            this.oriReceivers.Add(receiver);
        }

        /// <summary>
        /// Unregisters a feedback receiver so it will not be notified when new feedback is available.
        /// </summary>
        /// <param name="receiver">The receiver to unregister.</param>
        public void UnregisterReceiver(IOrientationFeedbackReceiver receiver)
        {
            if (!this.oriReceivers.Contains(receiver))
            {
                return;
            }

            this.oriReceivers.Remove(receiver);
        }
    }
}