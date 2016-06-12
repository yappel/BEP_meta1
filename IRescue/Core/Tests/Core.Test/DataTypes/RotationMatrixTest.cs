// <copyright file="RotationMatrixTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test.DataTypes
{
    using System;

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

        [Test]
        public void VectorBasedRotationTest()
        {
            this.rotation = new RotationMatrix(new Vector3(0.7f, -0.7f, 0), 90);
            ////Generated with matlab vrrotvec2mat([0.7 -0.7 0 0.5*pi])
            float[] expected = new float[]
                                   {
                                       0.5f, -0.5f, -0.7071f,
                                       -0.5f, 0.5f, -0.7071f,
                                       0.7071f, 0.7071f, 0
                                   };
            Console.WriteLine(this.rotation);
            Assert.AreEqual(expected[0], this.rotation[0, 0]);
            Assert.AreEqual(expected[1], this.rotation[0, 1]);
            Assert.AreEqual(expected[2], this.rotation[0, 2]);
            Assert.AreEqual(expected[3], this.rotation[1, 0]);
            Assert.AreEqual(expected[4], this.rotation[1, 1]);
            Assert.AreEqual(expected[5], this.rotation[1, 2]);
            Assert.AreEqual(expected[6], this.rotation[2, 0]);
            Assert.AreEqual(expected[7], this.rotation[2, 1]);
            Assert.AreEqual(expected[8], this.rotation[2, 2]);
        }

        /// <summary>
        /// Test that a simple rotation returns the desired output vector.
        /// </summary>
        [Test]
        public void SimpleRotationTest2()
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
