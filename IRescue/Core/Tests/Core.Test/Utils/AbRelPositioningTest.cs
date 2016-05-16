// <copyright file="AbRelPositioningTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test
{
    using IRescue.Core.DataTypes;
    using IRescue.Core.Utils;
    using NUnit.Framework;
    using System;
    /// <summary>
    /// Test for Vector3
    /// </summary>
    public class AbRelPositioningTest
    {
        /// <summary>
        /// Absolute position
        /// </summary>
        private Pose abPosition;

        /// <summary>
        /// Relative position
        /// </summary>
        private Pose relPosition;

        /// <summary>
        /// Allowed margin
        /// </summary>
        private float epsilon = 0.00005f;

        /// <summary>
        /// Test GetLocation2D
        /// </summary>
        [Test]
        public void Test2D()
        {
            this.abPosition = new Pose(new Vector3(5, 0, 5), new Vector3(0, 0, 0));
            this.relPosition = new Pose(new Vector3(5, 0, 0), new Vector3(0, 0, 0));
            Pose result = AbRelPositioning.GetLocation2D(this.abPosition, this.relPosition);
            Assert.AreEqual(10, result.Position.X, this.epsilon);
            Assert.AreEqual(0, result.Position.Y, this.epsilon);
            Assert.AreEqual(5, result.Position.Z, this.epsilon);
        }

        /// <summary>
        /// Expected at location 3,0,3
        /// </summary>
        [Test]
        public void TestSimple3()
        {
            this.abPosition = new Pose(new Vector3(5, 0, 5), new Vector3(0, 0, 0));
            this.relPosition = new Pose(new Vector3(2, 0, 2), new Vector3(0, 45, 0));
            Pose result = AbRelPositioning.GetLocation(this.abPosition, this.relPosition);
            Assert.AreEqual(7, result.Position.X, this.epsilon);
            Assert.AreEqual(0, result.Position.Y, this.epsilon);
            Assert.AreEqual(3, result.Position.Z, this.epsilon);
        }
    }
}
