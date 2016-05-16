// <copyright file="PoseTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test
{
    using IRescue.Core.DataTypes;
    using NUnit.Framework;

    /// <summary>
    /// Test the Pose Class
    /// </summary>
    public class PoseTest
    {
        /// <summary>
        /// The position
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// The orientation
        /// </summary>
        private Vector3 orientation;

        /// <summary>
        /// Setup for the test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.position = new Vector3(1, 2, 3);
            this.orientation = new Vector3(120, 80, 360);
        }

        /// <summary>
        /// Test the constructor
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Pose pose = new Pose(this.position, this.orientation);
            Assert.AreEqual(this.position, pose.Position);
        }

        /// <summary>
        /// Test the constructor with no arguments
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            Pose pose = new Pose();
            Assert.AreEqual(new Vector3(0, 0, 0), pose.Position);
            Assert.AreEqual(new Vector3(0, 0, 0), pose.Orientation);
        }

        /// <summary>
        /// Test SetPosition
        /// </summary>
        [Test]
        public void TestSetPosition()
        {
            Pose pose = new Pose(this.position, this.orientation);
            Vector3 newPosition = new Vector3(16, 23, 12);
            pose.Position = newPosition;
            Assert.AreEqual(pose.Position, newPosition);
        }

        /// <summary>
        /// Test SetOrientation
        /// </summary>
        [Test]
        public void TestSetOrientation()
        {
            Pose pose = new Pose(this.position, this.orientation);
            Vector3 newPosition = new Vector3(16, 23, 12);
            pose.Position = newPosition;
            Assert.AreEqual(pose.Position, newPosition);
        }
    }
}
