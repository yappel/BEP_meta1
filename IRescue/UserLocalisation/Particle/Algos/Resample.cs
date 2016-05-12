// <copyright file="Resample.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System;
using MathNet.Numerics.LinearAlgebra;


namespace IRescue.UserLocalisation.Particle.Algos
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Resample
    {

        public static void Multinomial(Matrix<float> particles, Matrix<float> weights)
        {
            int[] indexes = new int[particles.RowCount];
            int j = 0;
            foreach (Vector<float> column in weights.EnumerateColumns())
            {
                Multinomial(column, indexes);
                float[] newparticles = new float[particles.RowCount];
                float[] newweights = new float[particles.RowCount];
                for (int i = 0; i < particles.RowCount; i++)
                {
                    newparticles[i] = particles[i, j];
                    newweights[i] = weights[i, j];
                }
                particles.SetColumn(j, newparticles);
                weights.SetColumn(j, newweights);
                j++;
            }
        }

        private static void Multinomial(Vector<float> weights, int[] listout)
        {
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
        }


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
