// <copyright file="QuaternionTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test.DataTypes
{
    using System;

    using IRescue.Core.DataTypes;

    using MathNet.Numerics;

    using NUnit.Framework;

    /// <summary>
    /// Tests for <see cref="Quaternion"/>
    /// </summary>
    public class QuaternionTest
    {
        /// <summary>
        /// Test subject.
        /// </summary>
        private Quaternion quaternion;

        /// <summary>
        /// Setup fields.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
        }

        /// <summary>
        /// Test if right quaternion is created.
        /// </summary>
        [Test]
        public void CreationTest()
        {
            Vector3 euler = new Vector3(10, 20, 30);
            this.quaternion = new Quaternion(new RotationMatrix(euler.X, euler.Y, euler.Z));
            ////Values generated with matlab rotm2quat(rotz(-10) * roty(-20) *rotx(-30))
            Assert.AreEqual(0.9437, this.quaternion.W, 0.001);
            Assert.AreEqual(-0.2685, this.quaternion.X, 0.001);
            Assert.AreEqual(-0.1449, this.quaternion.Y, 0.001);
            Assert.AreEqual(-0.1277, this.quaternion.Z, 0.001);
        }

        [Test]
        public void CreateTestCase3()
        {
            Vector3 euler = new Vector3(170, 20, 10);
            this.quaternion = new Quaternion(new RotationMatrix(euler.X, euler.Y, euler.Z));
            Quaternion expected = new Quaternion(0.0704f, -0.1798f, 0.0704f, -0.9786f);
            ////Values generated with matlab rotm2quat(rotz(-170) * roty(-20) *rotx(-10))
            AreEqual(expected, this.quaternion, 0.001f);
        }

        private static bool AreEqual(Quaternion expected, Quaternion actual, float epsilon)
        {
            bool equal = expected.Equals(actual, epsilon);
            if (equal)
            {
                return true;
            }

            Console.WriteLine($"Expected: {expected}");
            Console.WriteLine($"But was: {actual}");
            return false;
        }

        /// <summary>
        /// Test if the convertion to euler angles is correct.
        /// </summary>
        [Test]
        public void ToEulerAngleTest()
        {
            Vector3 euler = new Vector3(10, 20, 30);
            this.quaternion = new Quaternion(new RotationMatrix(euler.X, euler.Y, euler.Z));
            Vector3 actual = this.quaternion.EulerAnglesDegree;
            Assert.AreEqual(euler[0], actual[0], 0.0001f);
            Assert.AreEqual(euler[1], actual[1], 0.0001f);
            Assert.AreEqual(euler[2], actual[2], 0.0001f);
        }
    }
}
