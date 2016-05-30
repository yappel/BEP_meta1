using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
using IRescue.UserLocalisation.Particle.Algos.Resamplers;

namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IRescue.Core.Utils;

    using MathNet.Numerics;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;

    internal class CircularParticleController : AbstractParticleController
    {
        private const float minAngle = 0;

        private const float maxAngle = 360 - float.Epsilon;


        public CircularParticleController(IParticleGenerator particleGenerator, int particleAmount)
            : base(particleAmount, minAngle, maxAngle, particleGenerator)
        {
        }

        public override void AddToValues(float[] valuesToAdd)
        {
            if (valuesToAdd.Length != this.Count)
            {
                throw new ArgumentException("Length of input array is not the same as the particle count");
            }

            Vector<float> res = new DenseVector(this.Count);
            this.values.MapIndexed((index, value) => AngleMath.Sum(value, valuesToAdd[index]), res);
            this.Values = res.ToArray();
        }

        public override float[] DistanceToValue(float othervalue)
        {

            float[] res = new float[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                res[i] = (float)AngleMath.SmallesAngle(this.values[i], othervalue);
            }

            return res;
        }

        /// <summary>
        /// Calculates the weighted average based on the current particle values and corresponding weights.
        /// </summary>
        /// <returns>The weighted average weights of the particles.</returns>
        public override float WeightedAverage()
        {
            return AngleMath.WeightedAverage(this.values.ToArray(), this.weights.ToArray());
        }
    }
}