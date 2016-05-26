using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRescue.UserLocalisation.Particle
{
    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;

    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;

    internal class LinearParticleController : AbstractParticleController
    {
        private Vector<float> values;

        public LinearParticleController(IResampler resampler, IParticleGenerator particleGenerator, int particleAmount, float minValue, float maxValue)
            : base(resampler, particleAmount, minValue, maxValue)
        {
            this.values = new DenseVector(particleAmount);
            this.values.SetValues(particleGenerator.Generate(particleAmount, minValue, maxValue));
        }

        public override int Count => this.values.Count;

        public override float[] Values
        {
            get
            {
                return this.values.ToArray<float>();
            }

            set
            {
                float[] newvalues = new float[value.Length];
                value.CopyTo(newvalues, 0);
                ////Todo check inputlength
                this.values.SetValues(newvalues);
            }
        }

        public override void SetValueAt(int index, float value)
        {
            this.values[index] = value;
        }

        public override float GetValueAt(int index)
        {
            return this.values[index];
        }

        public override float[] DistanceToValue(float othervalue)
        {
            Vector<float> res = new DenseVector(this.values.Count);
            this.values.Multiply(-1).Add(othervalue, res);
            return res.ToArray();
        }

        /// <summary>
        /// Calculates the weighted average based on the current particle values and corresponding weights.
        /// </summary>
        /// <returns>The weighted average weights of the particles.</returns>
        public override float WeightedAverage()
        {
            Vector<float> res = new DenseVector(this.Count);
            this.values.PointwiseMultiply(this.weights, res);
            return res.Sum();
        }

        public override void AddToValues(float[] values)
        {
            ////TODO check inputlength
            this.values.Add(new DenseVector(values));
        }
    }
}
