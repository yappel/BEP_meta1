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

        private List<Vector> values;

        public CircularParticleController(IResampler resampler, IParticleGenerator particleGenerator, int particleAmount, float minValue, float maxValue)
            : base(resampler, particleAmount, minValue, maxValue)
        {
            this.values = particleGenerator.Generate(particleAmount, minValue, maxValue).Select(VectorMath.AngleToVector).ToList();
        }

        public override int Count => this.values.Count;

        public override float[] Values
        {
            get
            {
                return this.values.ConvertAll(VectorMath.Vector2ToAngle).ToArray();
            }

            set
            {
                this.values = value.Select(VectorMath.AngleToVector).ToList();
            }
        }

        public override void AddToValues(float[] values)
        {
            if (values.Length != this.Count)
            {
                throw new ArgumentException("Length of input array is not the same as the particle count");
            }

            List<Vector> vector = values.Select(VectorMath.AngleToVector).ToList();
            for (int i = 0; i < this.values.Count; i++)
            {
                Vector value = this.values[i];
                value.Add(vector[i], value);
            }
        }

        public override float[] DistanceToValue(float othervalue)
        {
            Vector vector = VectorMath.AngleToVector(othervalue);
            float[] res = new float[this.Count];
            for (int i = 0; i < this.values.Count; i++)
            {
                Vector value = this.values[i];
                double radianAngle = Math.Atan2((value[0] * vector[1]) - (vector[0] * value[1]), (value[0] * value[1]) + (vector[0] * vector[1]));
                res[i] = (float)Trig.RadianToDegree(radianAngle);
            }

            return res;
        }

        public override float GetValueAt(int index)
        {
            return VectorMath.Vector2ToAngle(this.values[index]);
        }

        public override void SetValueAt(int index, float value)
        {
            this.values[index] = VectorMath.AngleToVector(value);
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
                Vector<float> vector = this.values[i].Clone();
                vector.Multiply(this.weights[i], vector);
                res.Add(vector, res);
            }
            if ((Math.Abs(res[0]) < float.Epsilon) && (Math.Abs(res[1] - 1) < float.Epsilon))
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