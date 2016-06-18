// <copyright file="RotationMatrixTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test.DataTypes
{
    using System;

    using IRescue.Core.DataTypes;

    using MathNet.Numerics.LinearAlgebra.Single;

    using NUnit.Framework;

    /// <summary>
    /// Testing class for testing the functionality of the <see cref="RotationMatrix"/> class.
    /// Tests empty constructor and constructor with specified arguments.
    /// </summary>
    public class RotationMatrixTest
    {
        /// <summary>
        /// RotationMatrix to be used in the tests.
        /// </summary>
        private RotationMatrix rotation;

        /// <summary>
        /// Test that the constructor with no rotation returns the same vector again.
        /// </summary>
        [Test]
        public void NoRotationConstructorTest()
        {
            this.rotation = new RotationMatrix();
            Vector3 res = new Vector3(1, 2, 3);
            this.rotation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector3(1, 2, 3), res);
        }

        /// <summary>
        /// Test that a simple rotation returns the desired output vector.
        /// </summary>
        [Test]
        public void SimpleRotationTest()
        {
            this.rotation = new RotationMatrix(180, 0, 0);
            Vector3 up = new Vector3(0, 1, 0);
            Vector3 res = new Vector3();
            Vector3 expected = new Vector3(0, -1, 0);
            this.rotation.Multiply(up, res);
            this.AssertVectorAreEqual(expected, res);

            this.rotation = new RotationMatrix(0, 180, 0);
            Vector3 down = new Vector3(0, -1, 0);
            this.rotation.Multiply(down, res);
            this.AssertVectorAreEqual(expected, res);

            this.rotation = new RotationMatrix(0, 0, 90);
            expected = new Vector3(0, 0, 1);
            this.rotation.Multiply(down, res);
            this.AssertVectorAreEqual(expected, res);

            this.rotation = new RotationMatrix(180, 180, 90);
            res = new Vector3(0, 1, 0);
            this.rotation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector3(0, 0, 1), res);

            this.rotation = new RotationMatrix(45, 90, 30);
            res = new Vector3(1, 2, 3);
            this.rotation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector3(1.1554f, 3.4154f, 1), res);

            this.rotation = new RotationMatrix(0, 90, 0);
            res = new Vector3(1, 0, 1);
            this.rotation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector3(1, 0, -1), res);

        }

        [Test]
        public void GettingEulerAnglesTest()
        {
            this.rotation = new RotationMatrix(53, 23, 77);
            Assert.AreEqual(53, this.rotation.EulerAngles.X, 0.0001f);
            Assert.AreEqual(23, this.rotation.EulerAngles.Y, 0.0001f);
            Assert.AreEqual(77, this.rotation.EulerAngles.Z, 0.0001f);
        }

        /// <summary>
        /// Test if creating a matrix from a quaternion results in the same matrix as creating from euler angles.
        /// </summary>
        [Test]
        public void CreatinMatrixFromQuaterion()
        {
            RotationMatrix expected = new RotationMatrix(45, 90, 30);
            Quaternion q = new Quaternion(expected);
            RotationMatrix actual = new RotationMatrix(q.W, q.X, q.Y, q.Z);
            Assert.AreEqual(expected[0, 0], actual[0, 0], 0.0001);
            Assert.AreEqual(expected[1, 0], actual[1, 0], 0.0001);
            Assert.AreEqual(expected[2, 0], actual[2, 0], 0.0001);
            Assert.AreEqual(expected[0, 1], actual[0, 1], 0.0001);
            Assert.AreEqual(expected[1, 1], actual[1, 1], 0.0001);
            Assert.AreEqual(expected[2, 1], actual[2, 1], 0.0001);
            Assert.AreEqual(expected[0, 2], actual[0, 2], 0.0001);
            Assert.AreEqual(expected[1, 2], actual[1, 2], 0.0001);
            Assert.AreEqual(expected[2, 2], actual[2, 2], 0.0001);
        }

        /// <summary>
        /// Assert that two vectors are equal.
        /// </summary>
        /// <param name="expected">The expected result.</param>
        /// <param name="actual">The actual result.</param>
        private void AssertVectorAreEqual(Vector3 expected, Vector3 actual)
        {
            Assert.AreEqual(expected.X, actual.X, 0.0001);
            Assert.AreEqual(expected.Y, actual.Y, 0.0001);
            Assert.AreEqual(expected.Z, actual.Z, 0.0001);
        }
    }
}
