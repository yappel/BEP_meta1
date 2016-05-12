// <copyright file="Feeder.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;

namespace IRescue.UserLocalisation.Particle.Algos
{
    /// <summary>
    /// Adds weight to particles
    /// </summary>
    public class Feeder
    {

        //public static void AddWeightsAll(double margin, params Tuple<float, float>[][] particles, params List<Tuple<float, float>>[] measurments)
        //{
        //    for (int i = 0; i < values.Length; i = i + 2)
        //    {
        //        Feeder.AddWeights(margin, values[i], values[i + 1]);
        //    }
        //}


        //TODO look at these calculations
        public static void AddWeights(double margin, Matrix<float> particles, Matrix<float> measurements, Matrix<float> weights)
        {
            for (int i = 0; i < particles.ColumnCount; i++)
            {
                for (int index = 0; index < particles.Column(i).Count; index++)
                {
                    float particle = particles.Column(i)[index];
                    var p = 1d;
                    for (int j = 0; j < measurements.Column(i).Count; j++)
                    {
                        float std = measurements[j, 4];
                        float meas = measurements[i, j];
                        p = p * (Normal.CDF(particle, std, meas + margin) -
                               Normal.CDF(particle, std, meas - margin));
                    }
                    weights[index, i] = (float)p;
                }
            }
        }

    }
}
