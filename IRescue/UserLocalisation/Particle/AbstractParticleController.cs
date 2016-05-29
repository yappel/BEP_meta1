namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Linq;

    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;

    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;

    public abstract class AbstractParticleController
    {
        public float maxValue;

        public float minValue;

        /// <summary>
        /// Contains the values of all the particles.
        /// </summary>
        protected Vector<float> values;

        protected Vector<float> weights;

        protected AbstractParticleController(int particleAmount, float minValue, float maxValue, IParticleGenerator particleGenerator)
        {
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.weights = new DenseVector(Enumerable.Repeat(1f / particleAmount, particleAmount).ToArray());
            this.values = new DenseVector(particleAmount);
            this.values.SetValues(particleGenerator.Generate(particleAmount, minValue, maxValue));
        }

        /// <summary>
        /// Gets the amount of particles.
        /// </summary>
        public virtual int Count => this.values.Count;

        /// <summary>
        /// Gets or sets the particle values.
        /// </summary>
        public virtual float[] Values
        {
            get
            {
                return this.values.ToArray<float>();
            }

            set
            {
                this.CheckArrayLength(value);
                if ((value.Max() > this.maxValue) || (value.Min() < this.minValue))
                {
                    throw new ArgumentOutOfRangeException($"All have value have to be bigger than the minimum and smaller than the maximum value. " + $"Min was {value.Min()}, max was {value.Max()}");
                }

                float[] newvalues = new float[value.Length];
                value.CopyTo(newvalues, 0);
                this.values.SetValues(newvalues);
            }
        }

        private void CheckArrayLength(float[] value)
        {
            if (value.Length != this.Count)
            {
                throw new ArgumentException("Array length does not equal the amount of particles");
            }
        }

        public virtual float[] Weights
        {
            get
            {
                return this.weights.ToArray<float>();
            }

            set
            {
                float[] newweights = new float[value.Length];
                value.CopyTo(newweights, 0);
                this.weights.SetValues(newweights);
            }
        }

        /// <summary>
        /// Pointwise adds an array of values to the existing values.
        /// </summary>
        /// <param name="values">List of the values to add.</param>
        public virtual void AddToValues(float[] values)
        {
            this.values.Add(new DenseVector(values), this.values);
            this.values.Map(val => val > this.maxValue ? this.maxValue : val, this.values);
            this.values.Map(val => val < this.minValue ? this.minValue : val, this.values);
        }

        public abstract float[] DistanceToValue(float othervalue);

        /// <summary>
        /// Gets the value at a certain index.
        /// </summary>
        /// <param name="index">The index to get the value of.</param>
        /// <returns>The value of the particle with the given index.</returns>
        public virtual float GetValueAt(int index)
        {
            return this.values[index];
        }

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
            float sum = this.weights.Sum();
            if (sum > float.Epsilon)
            {
                this.weights.Divide(sum, this.weights);
            }
            else
            {
                throw new DivideByZeroException("Cant normalize the weights when the sum of the weights is 0");
            }
        }

        /// <summary>
        /// Sets a value at an index.
        /// </summary>
        /// <param name="index">The index to place the value at.</param>
        /// <param name="value">The value to set.</param>
        public virtual void SetValueAt(int index, float value)
        {
            if ((this.minValue <= value) && (this.maxValue >= value))
            {
                this.values[index] = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Value is not between the minimum and maximum value");
            }
        }

        public virtual void SetWeightAt(int index, float weight)
        {
            this.weights[index] = weight;
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

        protected bool CheckSumWeightsNotZero()
        {
            return !(Math.Abs(this.weights.Sum()) < float.Epsilon);
        }

        public void AddToValues(float toadd)
        {
            this.AddToValues(Enumerable.Repeat(toadd, this.Count).ToArray());
        }
    }
}