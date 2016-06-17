// <copyright file="QuaternionTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test.DataTypes
{
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
            ////Values generated with matlab rotm2quat(rotz(10) * roty(20) *rotx(30))
            Assert.AreEqual(0.9515, this.quaternion.W, 0.001);
            Assert.AreEqual(0.0381, this.quaternion.X, 0.001);
            Assert.AreEqual(0.1893, this.quaternion.Y, 0.001);
            Assert.AreEqual(0.2393, this.quaternion.Z, 0.001);
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
