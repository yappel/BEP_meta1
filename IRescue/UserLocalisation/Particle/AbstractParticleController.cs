// <copyright file="AbstractParticleController.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle
{
    using System;
    using System.Linq;

    using IRescue.UserLocalisation.Particle.Algos.ParticleGenerators;

    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;

    /// <summary>
    /// Parent class for the class that controls the values and weights of particles.
    /// </summary>
    public abstract class AbstractParticleController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractParticleController"/> class.
        /// </summary>
        /// <param name="particleAmount"><see cref="Count"/></param>
        /// <param name="minValue"><see cref="MinValue"/></param>
        /// <param name="maxValue"><see cref="MaxValue"/></param>
        /// <param name="particleGenerator">The class used to create the initial collection of particle values.</param>
        protected AbstractParticleController(int particleAmount, float minValue, float maxValue, IParticleGenerator particleGenerator)
        {
            this.MaxValue = maxValue;
            this.MinValue = minValue;
            this.WeightsVector = new DenseVector(Enumerable.Repeat(1f / particleAmount, particleAmount).ToArray());
            this.ValuesVector = new DenseVector(particleAmount);
            this.ValuesVector.SetValues(particleGenerator.Generate(particleAmount, minValue, maxValue));
        }

        /// <summary>
        /// Gets the amount of particles.
        /// </summary>
        public virtual int Count => this.ValuesVector.Count;

        /// <summary>
        /// Gets the maximum value the particles can have.
        /// </summary>
        public float MaxValue { get; }

        /// <summary>
        /// Gets the minimum value the particles can have.
        /// </summary>
        public float MinValue { get; }

        /// <summary>
        /// Gets or sets the particle values.
        /// </summary>
        public virtual float[] Values
        {
            get
            {
                return this.ValuesVector.ToArray<float>();
            }

            set
            {
                this.CheckArrayLength(value);
                if ((value.Max() > this.MaxValue) || (value.Min() < this.MinValue))
                {
                    throw new ArgumentOutOfRangeException($"All have value have to be bigger than the minimum and smaller than the maximum value. " + $"Min was {value.Min()}, max was {value.Max()}");
                }

                float[] newvalues = new float[value.Length];
                value.CopyTo(newvalues, 0);
                this.ValuesVector.SetValues(newvalues);
            }
        }

        /// <summary>
        /// Gets or sets the weights of the particles.
        /// </summary>
        public virtual float[] Weights
        {
            get
            {
                return this.WeightsVector.ToArray<float>();
            }

            set
            {
                float[] newweights = new float[value.Length];
                value.CopyTo(newweights, 0);
                this.WeightsVector.SetValues(newweights);
            }
        }

        /// <summary>
        /// Gets the vector containing the values of all the particles.
        /// </summary>
        protected Vector<float> ValuesVector { get; }

        /// <summary>
        /// Gets the vector containing the weights of all the particles.
        /// </summary>
        protected Vector<float> WeightsVector { get; }

        /// <summary>
        /// Pointwise adds an array of values to the existing values.
        /// </summary>
        /// <param name="values">List of the values to add.</param>
        public virtual void AddToValues(float[] values)
        {
            this.ValuesVector.Add(new DenseVector(values), this.ValuesVector);
            this.ValuesVector.Map(val => val > this.MaxValue ? this.MaxValue : val, this.ValuesVector);
            this.ValuesVector.Map(val => val < this.MinValue ? this.MinValue : val, this.ValuesVector);
        }

        /// <summary>
        /// Add a certain value to all particle values.
        /// </summary>
        /// <param name="toadd">The value to add to all particle values.</param>
        public void AddToValues(float toadd)
        {
            this.AddToValues(Enumerable.Repeat(toadd, this.Count).ToArray());
        }

        /// <summary>
        /// Calculates the distance between all particle values and a given value.
        /// </summary>
        /// <param name="othervalue">The value to compare the particle values with.</param>
        /// <returns>The values that need to be add to the particle values to get the value given in the parameter.</returns>
        public abstract float[] DistanceToValue(float othervalue);

        /// <summary>
        /// Gets the value of the particle with a certain index.
        /// </summary>
        /// <param name="index">The index of the particle to get the weight of.</param>
        /// <returns>The value of the particle with the given index.</returns>
        public virtual float GetValueAt(int index)
        {
            return this.ValuesVector[index];
        }

        /// <summary>
        /// Gets the weight of the particle with a certain index.
        /// </summary>
        /// <param name="index">The index of the particle to get the weight of.</param>
        /// <returns>The weight of the particle.</returns>
        public virtual float GetWeightAt(int index)
        {
            return this.WeightsVector[index];
        }

        /// <summary>
        /// Multiply the weight of a particle with a certain value.
        /// </summary>
        /// <param name="index">The index of the particle to multiply the particle of.</param>
        /// <param name="scalar">The value to multipy with.</param>
        public void MultiplyWeightAt(int index, float scalar)
        {
            this.WeightsVector[index] *= scalar;
        }

        /// <summary>
        /// Normalize the weights of the particles,
        ///  ie scale the weights of the particles so that the sum of the weights is 1.
        /// </summary>
        public void NormalizeWeights()
        {
            float sum = this.WeightsVector.Sum();
            if (sum < float.Epsilon)
            {
                throw new DivideByZeroException("Cant normalize the weights when the sum of the weights is 0");
            }
            else
            {
                this.WeightsVector.Divide(sum, this.WeightsVector);
            }
        }

        /// <summary>
        /// Sets a value of a particle.
        /// </summary>
        /// <param name="index">The index of the particle to set the value of.</param>
        /// <param name="value">The new value of the particle.</param>
        public virtual void SetValueAt(int index, float value)
        {
            if ((this.MinValue <= value) && (this.MaxValue >= value))
            {
                this.ValuesVector[index] = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Value is not between the minimum and maximum value");
            }
        }

        /// <summary>
        /// Sets a weight of a particle.
        /// </summary>
        /// <param name="index">The index of the particle to set the weight of.</param>
        /// <param name="weight">The new weight of the particle.</param>
        public virtual void SetWeightAt(int index, float weight)
        {
            this.WeightsVector[index] = weight;
        }

        /// <summary>
        /// Change all weights.
        /// </summary>
        /// <param name="weight">The new value to change all weights to.</param>
        public virtual void SetWeights(float weight)
        {
            this.WeightsVector.SetValues(Enumerable.Repeat(weight, this.Count).ToArray());
        }

        /// <summary>
        /// Calculates the weighted average based on the current particle values and corresponding weights.
        /// </summary>
        /// <returns>The weighted average weights of the particles.</returns>
        public abstract float WeightedAverage();

        /// <summary>
        /// Checks if the sum of the weights is 0.
        /// </summary>
        /// <returns>True if the sum of the weights is not zero, false otherwise</returns>
        protected bool CheckSumWeightsNotZero()
        {
            return !(Math.Abs(this.WeightsVector.Sum()) < float.Epsilon);
        }

        /// <summary>
        /// Check the length of the given array and throw exception if it is not the same length as the particle count.
        /// </summary>
        /// <param name="array">The array to check the length of.</param>
        private void CheckArrayLength(float[] array)
        {
            if (array.Length != this.Count)
            {
                throw new ArgumentException("Array length does not equal the amount of particles");
            }
        }
    }
}