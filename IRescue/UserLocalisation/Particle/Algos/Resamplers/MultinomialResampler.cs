﻿// <copyright file="MultinomialResampler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>
namespace IRescue.UserLocalisation.Particle.Algos.Resamplers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Resamples particles using the multinomial algorithm.
    /// </summary>
    public class MultinomialResampler : IResampler
    {
        /// <summary>
        /// Random for random numbers
        /// </summary>
        private static Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultinomialResampler"/> class.
        /// </summary>
        public MultinomialResampler()
        {
            random = new Random();
        }

        /// <inheritdoc/>
        public void Resample(AbstractParticleController parCon)
        {
            parCon.NormalizeWeights();
            int[] indexes = Multinomial(parCon);
            float[] newParticleValues = new float[indexes.Length];
            for (int i = 0; i < newParticleValues.Length; i++)
            {
                newParticleValues[i] = parCon.GetValueAt(indexes[i]);
            }

            parCon.Values = newParticleValues;
        }

        /// <summary>
        /// Calculates the cumulative sum of the _values in a vector.
        /// </summary>
        /// <param name="list">The _values to calculate with.</param>
        /// <returns>List with the cumulative sum values of the input list.</returns>
        private static IList<float> CumSum(IList<float> list)
        {
            float[] output = new float[list.Count];
            list.CopyTo(output, 0);
            for (int i = 1; i < list.Count; i++)
            {
                output[i] = output[i - 1] + output[i];
            }

            return output;
        }

        /// <summary>
        /// Selects a list of indexes of the Particles that survived the resampling.
        /// </summary>
        /// <param name="particles">The Particles in a certain dimension</param>
        /// <returns>The list with indexes of the Particles that are chosen by the resample algorithm</returns>
        private static int[] Multinomial(AbstractParticleController particles)
        {
            int[] listout = new int[particles.Count];
            IList<float> weights = CumSum(particles.Weights);
            weights[weights.Count - 1] = 1;
            int i = 0;
            while (i < weights.Count)
            {
                double rand = random.NextDouble();
                int j = 0;
                while (weights[j] < rand)
                {
                    j++;
                }

                listout[i] = j;
                i++;
            }

            return listout;
        }
    }
}