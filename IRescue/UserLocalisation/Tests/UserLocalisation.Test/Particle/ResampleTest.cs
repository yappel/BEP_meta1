// <copyright file="ResampleTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace IRescue.UserLocalisation.Particle
{
    using Algos;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Single;
    using NUnit.Framework;

    /// <summary>
    /// Test resampling using the Multinomial algorithm.
    /// </summary>
    public class ResampleTest
    {
        /// <summary>
        /// Test the cumulative sum method.
        /// </summary>
        [Test]
        public void TestCumsum()
        {
            Vector<float> vec = new DenseVector(new float[] { 1, 1, 1, 1, 1, 1 });
            Resample.CumSum(vec);
            float[] expected = new float[] { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(expected, vec.ToArray());
        }
    }
}
