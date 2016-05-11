// <copyright file="AbRelPositioningTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test
{
    using IRescue.Core.DataTypes;
    using IRescue.Core.Utils;
    using MathNet.Numerics.LinearAlgebra.Single;
    using NUnit.Framework;

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
        /// Test for simple position.
        /// </summary>
        [Test]
        public void TestSimple()
        {
            abPosition = new Pose(new Vector3(5, 0, 5), new Vector3(0, 0, 0));
            relPosition = new Pose(new Vector3(5, 0, 0), new Vector3(0, 90, 0));
            Pose result = AbRelPositioning.GetLocation(abPosition, relPosition);
            Assert.AreEqual(5, result.Position.X, epsilon);
            Assert.AreEqual(0, result.Position.Y, epsilon);
            Assert.AreEqual(10, result.Position.Z, epsilon);
        }

        /// <summary>
        /// Expected at location 3,0,5
        /// </summary>
        [Test]
        public void TestSimple2()
        {
            abPosition = new Pose(new Vector3(5, 0, 5), new Vector3(0, 0, 0));
            relPosition = new Pose(new Vector3(2, 0, 0), new Vector3(0, 0, 0));
            Pose result = AbRelPositioning.GetLocation(abPosition, relPosition);
            Assert.AreEqual(3, result.Position.X, epsilon);
            //Assert.AreEqual(0, result.Position.Y, epsilon);
            //Assert.AreEqual(5, result.Position.Z, epsilon);
        }

        /// <summary>
        /// Expected at location 3,0,3
        /// </summary>
        [Test]
        public void TestSimple3()
        {
            abPosition = new Pose(new Vector3(5, 0, 5), new Vector3(0, 0, 0));
            relPosition = new Pose(new Vector3(2, 0, 2), new Vector3(0, 45, 0));
            Pose result = AbRelPositioning.GetLocation(abPosition, relPosition);
            Assert.AreEqual(3, result.Position.X, epsilon);
            Assert.AreEqual(0, result.Position.Y, epsilon);
            Assert.AreEqual(3, result.Position.Z, epsilon);
        }

        [Test]
        public void TestGetDistance()
        {
            Assert.AreEqual(1, AbRelPositioning.CalculateDistance(new Vector3(1, 0, 0)));
        }
    }
}
