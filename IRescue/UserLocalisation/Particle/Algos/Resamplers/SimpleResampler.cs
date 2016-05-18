// <copyright file="SimpleResampler.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;

namespace IRescue.UserLocalisation.Particle.Algos.Resamplers
{
    /// <summary>
    /// TODO
    /// </summary>
    public class SimpleResampler : IResampler
    {
        public void Resample(Matrix<float> particles, Matrix<float> weights)
        {
            int j = 0;
            foreach (Vector<float> column in weights.EnumerateColumns())
            {
                int[] indexes = Indexes(column);
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

        private int[] Indexes(Vector<float> column)
        {
            int[] pit = new int[column.Count];
            int count = 0;
            int i = 0;
            RandomSource nrg = new SystemRandomSource();
            while (count < column.Count - 1)
            {
                float random = (float)nrg.NextDouble();
                if (column[i] < 0.000001)
                {
                    i++;
                    i = i % column.Count;
                    continue;
                }
                if (column[i] > random)
                {
                    pit[count] = i;
                    count++;
                }
                i++;
                i = i % column.Count;
            }
            return pit;
        }
    }
}
