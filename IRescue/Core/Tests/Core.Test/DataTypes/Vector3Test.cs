// <copyright file="Vector3Test.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test
{
    using IRescue.Core.DataTypes;
    using MathNet.Numerics.LinearAlgebra.Single;
    using NUnit.Framework;

    /// <summary>
    /// Test for Vector3
    /// </summary>
    public class Vector3Test
    {
        /// <summary>
        /// Test the constructor
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Vector3 vector = new Vector3(1, 2, 3);
            Assert.True(vector is DenseVector);
        }

        /// <summary>
        /// Test SetX
        /// </summary>
        [Test]
        public void TestSetX()
        {
            Vector3 vector = new Vector3(1, 2, 3);
            vector.X = 12;
            Assert.AreEqual(12, vector.X);
        }

        /// <summary>
        /// Test SetY
        /// </summary>
        [Test]
        public void TestSetY()
        {
            Vector3 vector = new Vector3(1, 2, 3);
            vector.Y = 112;
            Assert.AreEqual(112, vector.Y);
        }

        /// <summary>
        /// Test SetZ
        /// </summary>
        [Test]
        public void TestSetZ()
        {
            Vector3 vector = new Vector3(1, 2, 3);
            vector.Z = 122;
            Assert.AreEqual(122, vector.Z);
        }
    }
}
