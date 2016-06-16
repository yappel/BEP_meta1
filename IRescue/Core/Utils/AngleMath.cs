// <copyright file="AngleMath.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.Core.Utils
{
    using System;
    using System.Linq;

    using MathNet.Numerics;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;

    /// <summary>
    /// Math for calculations with angles.
    /// </summary>
    public static class AngleMath
    {
        /// <summary>
        /// Calculates the average angle of a collection of angles.
        /// </summary>
        /// <param name="angles">Collection of angles in degrees to calculate the average of.</param>
        /// <returns>The average angle between 0 and 360 degrees</returns>
        public static float Average(float[] angles)
        {
            return WeightedAverage(angles, Enumerable.Repeat(1f / angles.Length, angles.Length).ToArray());
        }

        /// <summary>
        /// Calculates the amount of degrees closest to zero to add to an angle to get a certain other angle.
        /// </summary>
        /// <param name="source">The angle in degrees to add the result of this method to.</param>
        /// <param name="destination">The target angle in degrees.</param>
        /// <returns>The amount of degrees closest to zero to add to the source angle to get the destination angle.</returns>
        public static double SmallesAngle(float source, float destination)
        {
            double a = destination - source;
            return Euclid.Modulus(a + 180, 360) - 180;
        }

        /// <summary>
        /// Calculates the summation of multiple angles.
        /// </summary>
        /// <param name="angles">The angles in degrees to calculate the summation of.</param>
        /// <returns>The summation between 0 and 360 degrees.</returns>
        public static float Sum(params float[] angles)
        {
            return Euclid.Modulus(angles.Sum(), 360);
        }

        /// <summary>
        /// Calculates the weighted average angle of a collection of angles.
        /// </summary>
        /// <param name="angles">Collection of angles in degrees to calculate the average of.</param>
        /// <param name="weights">The weights to give the angles in the calculation.</param>
        /// <returns>The weighted average angle between 0 and 360 degrees.</returns>
        public static float WeightedAverage(float[] angles, float[] weights)
        {
            if (angles.Length != weights.Length)
            {
                throw new ArgumentException("Input arrays have to be of the same length");
            }

            Vector res = new DenseVector(2);
            for (int i = 0; i < angles.Length; i++)
            {
                Vector<float> vector = VectorMath.AngleToVector(angles[i], weights[i]);
                res.Add(vector, res);
            }

            if ((Math.Abs(res[0]) < 1E-6) && (Math.Abs(res[1]) < 1E-6))
            {
                return float.NaN;
            }

            return VectorMath.Vector2ToAngle(res);
        }
    }
}