// <copyright file="MarkerSensorTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace UserLocalisation.Test.Sensors.Marker
{
    using System.Collections.Generic;
    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Sensors.Marker;
    using NUnit.Framework;

    /// <summary>
    /// Test for MarkerSensor
    /// </summary>
    public class MarkerSensorTest
    {
        /// <summary>
        /// The tested sensor
        /// </summary>
        private MarkerSensor sensor;

        /// <summary>
        /// The standard deviation
        /// </summary>
        private float std = 20.5f;

        /// <summary>
        /// Error margin for the rotation
        /// </summary>
        private float epsilon = 2;

        /// <summary>
        /// Error margin for position
        /// </summary>
        private float epsilonP = 0.25f;

        /// <summary>
        /// Path to the xml
        /// </summary>
        private string savePath = TestContext.CurrentContext.TestDirectory + "\\MarkerMap01.xml";

        /// <summary>
        /// The default type of probability distribution belonging to the measurements of the position.
        /// </summary>
        private DistributionType posDistType = DistributionType.gaussian;

        /// <summary>
        /// The default type of probability distribution belonging to the measurements of the orientation.
        /// </summary>
        private DistributionType oriDistType = DistributionType.gaussian;

        /// <summary>
        /// Setup the map
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.sensor = new MarkerSensor(this.std, this.savePath, this.posDistType, this.oriDistType);
        }

        /// <summary>
        /// Test regular update
        /// </summary>
        [Test]
        public void TestUpdate()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(2, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(180f, this.sensor.GetLastOrientation().Data.X, this.epsilon);
        }

        /// <summary>
        /// Test if the loop continues when an exception will be thrown.
        /// </summary>
        [Test]
        public void TestUpdateNothingToSeeHere()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(-1337, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(180f, this.sensor.GetLastOrientation().Data.X, this.epsilon);
        }

        /// <summary>
        /// Test if the calculus class was correct.
        /// </summary>
        [Test]
        public void TestUpdateCalculus()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(18f, this.sensor.GetLastPosition().Data.X, this.epsilonP);
        }

        /// <summary>
        /// test the get position measurements.
        /// </summary>
        [Test]
        public void TestGetPositionMeasurements()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(2, this.sensor.GetPositions(0, long.MaxValue).Count);
        }

        /// <summary>
        /// test the get all position measurements.
        /// </summary>
        [Test]
        public void TestGetAllPositions()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(2, this.sensor.GetAllPositions().Count);
        }

        /// <summary>
        /// test the get orientation measurement for null.
        /// </summary>
        [Test]
        public void TestGetOrientationMeasurementNull()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(910, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.Null(this.sensor.GetOrientation(-50));
        }

        /// <summary>
        /// test the get orientation measurements.
        /// </summary>
        [Test]
        public void TestGetOrientationMeasurements()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(2, this.sensor.GetOrientations(0, long.MaxValue).Count);
        }

        /// <summary>
        /// test the get all orientation measurements.
        /// </summary>
        [Test]
        public void TestGetAllOrientation()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(2, this.sensor.GetAllOrientations().Count);
        }

        /// <summary>
        /// test the get position measurements for null.
        /// </summary>
        [Test]
        public void TestGetPositionMeasurementNull()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.Null(this.sensor.GetPosition(-50));
        }

        /// <summary>
        /// Test for when the buffer size gets exceeded.
        /// </summary>
        [Test]
        public void TestOverflow()
        {
            this.sensor = new MarkerSensor(this.std, this.savePath, 6, DistributionType.uniform, DistributionType.uniform);
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            this.sensor.UpdateLocations(dic);
            this.sensor.UpdateLocations(dic);
            this.sensor.UpdateLocations(dic);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(6, this.sensor.GetAllOrientations().Count);
        }

        /// <summary>
        /// Test for when one iteration had no data.
        /// </summary>
        [Test]
        public void TestNoData()
        {
            this.sensor = new MarkerSensor(this.std, this.savePath, 6, DistributionType.uniform, DistributionType.uniform);
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            this.sensor.UpdateLocations(dic);
            this.sensor.UpdateLocations(dic);
            this.sensor.UpdateLocations(dic);
            this.sensor.UpdateLocations(dic);
            this.sensor.UpdateLocations(new Dictionary<int, Pose>());
            Assert.AreEqual(6, this.sensor.GetAllOrientations().Count);
        }
    }
}
