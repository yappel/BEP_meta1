// <copyright file="MeasurementTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace Core.Test
{
    using IRescue.Core.DataTypes;
    using NUnit.Framework;

    /// <summary>
    /// Test the Measurement Class
    /// </summary>
    public class MeasurementTest
    {
        /// <summary>
        /// The pose
        /// </summary>
        private Pose pose;

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
            this.pose = new Pose(this.position, this.orientation);
        }

        /// <summary>
        /// Test the constructor
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Measurement<Pose> measurement = new Measurement<Pose>(this.pose, 20, 1);
            Assert.True(measurement.Data is Pose);
            Assert.True(measurement.Std == 20);
            Assert.True(measurement.TimeStamp == 1);
        }

        /// <summary>
        /// Test SetData for set null
        /// </summary>
        [Test]
        public void TestSetData()
        {
            Measurement<Pose> measurement = new Measurement<Pose>(this.pose, 1.2f, 2441);
            Pose newPose = new Pose(this.position, this.orientation);
            measurement.Data = newPose;
            Assert.AreEqual(measurement.Data, newPose);
            Assert.AreNotEqual(measurement.Data, this.pose);
        }

        /// <summary>
        /// Test SetData for set null
        /// </summary>
        [Test]
        public void TestSetDataNull()
        {
            Measurement<Pose> measurement = new Measurement<Pose>(this.pose, 1.2f, 2441);
            Assert.NotNull(measurement.Data);
            measurement.Data = null;
            Assert.Null(measurement.Data);
        }

        /// <summary>
        /// Test Set standard deviation
        /// </summary>
        [Test]
        public void TestSetStd()
        {
            Measurement<Pose> measurement = new Measurement<Pose>(this.pose, 52, 19);
            measurement.Std = 21;
            Assert.AreEqual(21, measurement.Std);
        }

        /// <summary>
        /// Test SetTimestamp
        /// </summary>
        [Test]
        public void TestSetTimestamp()
        {
            Measurement<Pose> measurement = new Measurement<Pose>(this.pose, 12, 51);
            measurement.TimeStamp = 122;
            Assert.AreEqual(122, measurement.TimeStamp);
        }
    }
}
