// <copyright file="MarkerSensorTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace UserLocalisation.Test.Sensors.Marker
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
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
        /// Path to the xml
        /// </summary>
        private string savePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\MarkerMap01.xml";

        /// <summary>
        /// Setup the map
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.sensor = new MarkerSensor(this.std, this.savePath);
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
            Assert.AreEqual(pose2.Orientation, this.sensor.GetLastOrientation().Data);
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
            Assert.AreEqual(pose2.Orientation, this.sensor.GetLastOrientation().Data);
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
            Assert.AreEqual(25f, this.sensor.GetLastPosition().Data.X);
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
        /// test the get orientation measurement.
        /// </summary>
        [Test]
        public void TestGetOrientationMeasurement()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(910, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(90, this.sensor.GetOrientation(0).Data.X);
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
        /// test the get position measurements.
        /// </summary>
        [Test]
        public void TestGetPositionMeasurement()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(dic);
            Assert.AreEqual(12f, this.sensor.GetPosition(0).Data.X);
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
    }
}
