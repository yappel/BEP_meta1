namespace IRescue.UserLocalisation.Particle
{
    using System.Linq;

    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;
    using IRescue.UserLocalisation.Particle.Algos.Resamplers;

    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;

    public abstract class AbstractParticleController
    {
        private readonly IResampler resampler;

        protected Vector<float> weights;

        protected AbstractParticleController(IResampler resampler, int particleAmount, float minValue, float maxValue)
        {
            this.resampler = resampler;
            this.maxValue = minValue;
            this.minValue = maxValue;
            this.weights = new DenseVector(Enumerable.Repeat(1f / particleAmount, particleAmount).ToArray());
        }

        public abstract int Count { get; }

        public float maxValue;

        public float minValue;

        public abstract float[] Values { get; set; }

        public float[] Weights => this.weights.ToArray<float>();

        public abstract void AddToValues(float[] values);

        public abstract float[] DistanceToValue(float othervalue);

        public abstract float GetValueAt(int index);

        public virtual float GetWeightAt(int index)
        {
            return this.weights[index];
        }

        public void MultiplyWeightAt(int index, float scalar)
        {
            this.weights[index] *= scalar;
        }

        public void NormalizeWeights()
        {
            if (this.weights.Sum() > float.Epsilon)
            {
                this.weights.DivideByThis(this.weights.Sum(), this.weights);
            }
            else
            {
                this.weights.Map(w => 1f / this.Count);
            }
        }

        /// <summary>
        /// Resamples the particles so the ones with low weights do not survive.
        /// </summary>
        public void Resample()
        {
            this.resampler.Resample(this);
        }

        public abstract void SetValueAt(int index, float value);

        public virtual void SetWeightAt(int index, float weight)
        {
            this.weights[index] = weight;
        }

        public virtual void SetWeights(float[] weights)
        {
            this.weights.SetValues(weights);
        }

        public virtual void SetWeights(float weight)
        {
            this.weights.SetValues(Enumerable.Repeat(weight, this.Count).ToArray());
        }

        /// <summary>
        /// Calculates the weighted average based on the current particle values and corresponding weights.
        /// </summary>
        /// <returns>The weighted average weights of the particles.</returns>
        public abstract float WeightedAverage();
    }
}