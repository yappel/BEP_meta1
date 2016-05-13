// <copyright file="Resample.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle.Algos
{
    using System;
    using MathNet.Numerics.LinearAlgebra;

    /// <summary>
    /// Class to resample Particles.
    /// </summary>
    public class Resample
    {
        /// <summary>
        /// Resamples Particles using a Multinomial algorithm
        /// </summary>
        /// <param name="particles">The Particles to resample</param>
        /// <param name="weights">The Weights of the Particles</param>
        public static void Multinomial(Matrix<float> particles, Matrix<float> weights)
        {
            int j = 0;
            foreach (Vector<float> column in weights.EnumerateColumns())
            {
                int[] indexes = Multinomial(column);
                float[] newparticles = new float[particles.RowCount];
                float[] newweights = new float[particles.RowCount];

                for (int i = 0; i < particles.RowCount; i++)
                {
                    newparticles[i] = particles[indexes[i], j];
                    newweights[i] = weights[indexes[i], j];
                }

                particles.SetColumn(j, newparticles);
                weights.SetColumn(j, newweights);
                j++;
            }
        }

        /// <summary>
        /// Selects a list of indexes of the Particles that will survive the resampling.
        /// </summary>
        /// <param name="weights">The Weights of the Particles in a certain dimension</param>
        /// <returns>The list with indexes of the Particles that are chosen by the resample algorithm</returns>
        public static int[] Multinomial(Vector<float> weights)
        {
            int[] listout = new int[weights.Count];
            CumSum(weights);
            Random random = new Random();
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

        /// <summary>
        /// Calculates the cumulative sum of the values in a vector.
        /// </summary>
        /// <param name="list">The values to calculate with.</param>
        public static void CumSum(Vector<float> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    list[i] = list[i - 1] + list[i];
                }
            }
        }
    }
}
