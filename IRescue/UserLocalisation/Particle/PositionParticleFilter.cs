﻿// <copyright file="PositionParticleFilter.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Particle.Algos.NoiseGenerators;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;
    using IRescue.UserLocalisation.Particle.Algos.Smoothers;
    using IRescue.UserLocalisation.Sensors;

    internal class PositionParticleFilter : AbstractParticleFilter, IPositionReceiver, IDisplacementReceiver
    {
        private List<IDisplacementSource> dislocationSources;

        private List<IPositionSource> positionSources;

        private Vector3 previousResult;

        public PositionParticleFilter(INoiseGenerator noiseGenerator, float resampleNoiseSize, IResampler resampler, IParticleGenerator particleGenerator, int particleAmount, FieldSize fieldSize, ISmoother smoother)
            : base(
                  resampler,
                  noiseGenerator,
                  new LinearParticleController(particleGenerator, particleAmount, fieldSize.Xmin, fieldSize.Xmax),
                  new LinearParticleController(particleGenerator, particleAmount, fieldSize.Ymin, fieldSize.Ymax),
                  new LinearParticleController(particleGenerator, particleAmount, fieldSize.Zmin, fieldSize.Zmax),
                  resampleNoiseSize,
                  smoother,
                  Enumerable.Average)
        {
            this.dislocationSources = new List<IDisplacementSource>();
            this.positionSources = new List<IPositionSource>();
        }

        /// <summary>
        /// Add a displacement source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        public void AddDisplacementSource(IDisplacementSource source)
        {
            this.dislocationSources.Add(source);
        }

        /// <summary>
        /// Add a position source to the receiver.
        /// </summary>
        /// <param name="source">The source from which data can be extracted.</param>
        public void AddPositionSource(IPositionSource source)
        {
            this.positionSources.Add(source);
        }

        protected override Vector3 ProcessResults()
        {
            Vector3 result = base.ProcessResults();
            this.previousResult = result;
            return result;
        }

        protected override void RetrieveMeasurements()
        {
            this.Measurements.Clear();
            this.CheckPositionSources();
            if (this.previousResult != null)
            {
                this.CheckDislocationSources();
            }
        }

        private void CheckDislocationSources()
        {
            foreach (IDisplacementSource source in this.dislocationSources)
            {
                Measurement<Vector3> meas = source.GetDisplacement(this.PreviousTimeStamp, this.CurrentTimeStamp);
                meas.Data.Add(this.previousResult, meas.Data);
                this.Measurements.Add(meas);
            }
        }

        private void CheckPositionSources()
        {
            foreach (IPositionSource source in this.positionSources)
            {
                this.Measurements.AddRange(source.GetPositionsClosestTo(this.CurrentTimeStamp, this.CurrentTimeStamp - this.PreviousTimeStamp));
            }
        }
    }
}