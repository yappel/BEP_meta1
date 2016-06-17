// <copyright file="Vector3Test.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test.DataTypes
{
    using System;
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
        /// Test the constructor
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            Vector3 vector = new Vector3(new float[] { 1, 2, 3 });
            Assert.True(vector is DenseVector);
            Assert.True(vector.X == 1);
        }

        /// <summary>
        /// Test the constructor exception
        /// </summary>
        [Test]
        public void TestConstructorException()
        {
            Assert.That(() => new Vector3(new float[] { 1, 2, 3, 4 }), Throws.TypeOf<ArgumentException>());
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

        /// <summary>
        /// Test that the empty constructor creates a vector with all zero values.
        /// </summary>
        [Test]
        public void EmptyConstructorTest()
        {
            Vector3 vec = new Vector3();
            Assert.AreEqual(0, vec.X);
            Assert.AreEqual(0, vec.Y);
            Assert.AreEqual(0, vec.Z);
        }
    }
}
