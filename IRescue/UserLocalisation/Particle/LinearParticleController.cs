// <copyright file="LinearParticleController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;

    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;

    /// <summary>
    /// Controller for particles with linear _values.
    /// </summary>
    internal class LinearParticleController : AbstractParticleController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearParticleController"/> class.
        /// </summary>
        /// <param name="particleGenerator">The class used to generate new particles</param>
        /// <param name="particleAmount">The amount of particles</param>
        /// <param name="minValue">The minimum value that a particle can have</param>
        /// <param name="maxValue">The maximum value that a particle can have</param>
        public LinearParticleController(IParticleGenerator particleGenerator, int particleAmount, float minValue, float maxValue)
            : base(particleAmount, minValue, maxValue, particleGenerator)
        {
        }

        /// <summary>
        /// Calculate the distance of the given value to every value of the particles.
        /// </summary>
        /// <param name="othervalue">The value to compare to.</param>
        /// <returns>The number that needs to be added to the value of a particle to get the parameter value</returns>
        public override float[] DistanceToValue(float othervalue)
        {
            Vector<float> res = new DenseVector(this.ValuesVector.Count);
            this.ValuesVector.Multiply(-1).Add(othervalue, res);
            return res.ToArray();
        }

        /// <summary>
        /// Calculates the weighted average based on the current particle _values and corresponding weights.
        /// </summary>
        /// <returns>The weighted average weights of the particles.</returns>
        public override float WeightedAverage()
        {
            if (!this.CheckSumWeightsNotZero())
            {
                return float.NaN;
            }

            Vector<float> res = new DenseVector(this.Count);
            this.ValuesVector.PointwiseMultiply(this.WeightsVector, res);
            return res.Sum() / this.WeightsVector.Sum();
        }
    }
}