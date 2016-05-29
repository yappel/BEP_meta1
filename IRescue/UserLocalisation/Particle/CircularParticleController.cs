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
            this.values.MapIndexed((index, value) => (value + valuesToAdd[index]), res);
            res.Modulus(360f, res);
            this.Values = res.ToArray();
        }

        public override float[] DistanceToValue(float othervalue)
        {

            float[] res = new float[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                double a = othervalue - this.values[i];
                res[i] = (float)(Euclid.Modulus(a + 180, 360) - 180);
            }

            return res;
        }

        /// <summary>
        /// Calculates the weighted average based on the current particle values and corresponding weights.
        /// </summary>
        /// <returns>The weighted average weights of the particles.</returns>
        public override float WeightedAverage()
        {
            Vector res = new DenseVector(2);
            for (int i = 0; i < this.values.Count; i++)
            {
                Vector<float> vector = VectorMath.AngleToVector(this.values[i], this.weights[i]);
                res.Add(vector, res);
            }

            if ((Math.Abs(res[0]) < 1E-6) && (Math.Abs(res[1]) < 1E-6))
            {
                return float.NaN;
            }
            else
            {
                return VectorMath.Vector2ToAngle(res);
            }
        }
    }
}