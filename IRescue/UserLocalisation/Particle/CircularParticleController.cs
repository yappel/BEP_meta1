// <copyright file="CircularParticleController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
    using System;

    using IRescue.Core.Utils;
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;

    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;

    /// <summary>
    /// Particle controller for particle values where the values of the particles are circular between 0 and 360, ie angles in degrees.
    /// </summary>
    internal class CircularParticleController : AbstractParticleController
    {
        private const float MaxAngle = 360 - float.Epsilon;

        private const float MinAngle = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularParticleController"/> class.
        /// </summary>
        /// <param name="particleGenerator">See the parameter particleGenerator of <seealso cref="AbstractParticleController"/></param>
        /// <param name="particleAmount">See the parameter particleAmount of <seealso cref="AbstractParticleController"/></param>
        public CircularParticleController(IParticleGenerator particleGenerator, int particleAmount)
            : base(particleAmount, MinAngle, MaxAngle, particleGenerator)
        {
        }

        /// <summary>
        /// Pointwise adds an array of values to the existing values.
        /// </summary>
        /// <param name="valuesToAdd">List of the values to add.</param>
        public override void AddToValues(float[] valuesToAdd)
        {
            if (valuesToAdd.Length != this.Count)
            {
                throw new ArgumentException("Length of input array is not the same as the particle count");
            }

            Vector<float> res = new DenseVector(this.Count);
            this.ValuesVector.MapIndexed((index, value) => AngleMath.Sum(value, valuesToAdd[index]), res);
            this.Values = res.ToArray();
        }

        /// <summary>
        /// Calculates the distance between all particle values and a given value.
        /// </summary>
        /// <param name="othervalue">The value to compare the particle values with.</param>
        /// <returns>The values that need to be add to the particle values to get the value given in the parameter.</returns>
        public override float[] DistanceToValue(float othervalue)
        {
            float[] res = new float[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                res[i] = (float)AngleMath.SmallesAngle(this.ValuesVector[i], othervalue);
            }

            return res;
        }

        /// <summary>
        /// Calculates the weighted average based on the current particle _values and corresponding weights.
        /// </summary>
        /// <returns>The weighted average weights of the particles.</returns>
        public override float WeightedAverage()
        {
            return AngleMath.WeightedAverage(this.ValuesVector.ToArray(), this.WeightsVector.ToArray());
        }
    }
}