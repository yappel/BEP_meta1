﻿// <copyright file="RotationMatrixTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test.DataTypes
{
    using IRescue.Core.DataTypes;
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
            this.rotation = new RotationMatrix(45, 90, 30);
            Vector3 res = new Vector3(1, 2, 3);
            this.rotation.Multiply(res, res);
            this.AssertVectorAreEqual(new Vector3(3.4154f, 1.1554f, -1.0000f), res);
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
