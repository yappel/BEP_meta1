// <copyright file="MarkerSensorTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace UserLocalisation.Test.Sensors.Marker
{
    using System.Collections.Generic;
    using IRescue.Core.DataTypes;
    using IRescue.Core.Distributions;
    using IRescue.UserLocalisation.Sensors.Marker;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// Test class for testing <see cref="MarkerSensor"/> class.
    /// Tests all basic functionality and has a test case to verify that the correct
    /// user position and orientation is returned from relative marker measurements.
    /// </summary>
    public class MarkerSensorTest
    {
        /// <summary>
        /// The tested sensor
        /// </summary>
        private MarkerSensor sensor;

        /// <summary>
        /// The default time stamp to use for measurements.
        /// </summary>
        private long defaultTimeStamp = 1;

        /// <summary>
        /// Error margin for the rotation
        /// </summary>
        private float epsilon = 2;

        /// <summary>
        /// Field for the marker locations corresponding to MarkerMap01.xml. Mocking not possible due to limitation of Moq.
        /// </summary>
        private MarkerLocations mlocmoq;

        /// <summary>
        /// Default epsilon for vector comparison.
        /// </summary>
        private double vectorEpsilon = 0.0001;

        /// <summary>
        /// Large set of relative measurements from markers, their world position/orientation and the user
        /// position/orientation which should be returned.
        /// </summary>
        private float[][] bulkData = new float[][]
        {
                new float[] { 1, 0, 1, 0, 0, 0, 2, 0, 1, 0, 180, 0, 3, 0, 2, 0, 180, 0 },
                new float[] { 2.98f, 0, 2.06f, 0, 290.99f, 0, 0.15f, 0, 2.06f, 0, 230.52f, 0, 1.11f, 0, 1.18f, 0, 60.47f, 0 },
                new float[] { 2.98f, 0, 2.06f, 0, 1.15f, 0, 1.99f, 0, 0.57f,  0, 10.43f, 0, 1.11f, 0, 1.18f, 0, 350.72f, 0 },
                new float[] { -1.4f, 0, -1.72f, 0, 154.32f, 0, 2.3f, 0, 2.01f, 0, 121.44f, 0, -4.42f, 0, -2.15f, 0, 32.88f, 0 },
                new float[] { -1.4f, -1.72f, 0, 0, 0, 205.68f, 1.02f, 5.29f, 0, 0, 0, 354.05f, -3.3f, 3.32f, 0, 0, 0, 211.62f },
                new float[] { 2, 0, 2, 0, 225, 0, 0, 0, 1.4142f, 0, 180, 0, 1, 0, 1, 0, 45, 0 },
                new float[] { 2, 0, 2, 0, -90, 0, 1, 0, 1, 0, 180, 0, 1, 0, 3, 0, 90, 0 },
                new float[] { 2, 0, 2, 0, 90, 0, 1, 0, 1, 0, 180, 0, 3, 0, 1, 0, -90, 0 },
        };

        /// <summary>
        /// The default type of probability distribution belonging to the measurements of the position.
        /// </summary>
        private Mock<IDistribution> posDistType;

        /// <summary>
        /// The default type of probability distribution belonging to the measurements of the orientation.
        /// </summary>
        private Mock<IDistribution> oriDistType;

        /// <summary>
        /// Setup the map
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.mlocmoq = new MarkerLocations();
            this.mlocmoq.AddMarker(
                0,
                new Pose(
                    new Vector3(25, 13.52f, 5),
                    new Vector3(0, 0, 0)));
            this.mlocmoq.AddMarker(
                1,
                new Pose(
                    new Vector3(12, 21.12f, 13),
                    new Vector3(0, 0, 0)));
            this.posDistType = new Mock<IDistribution>();
            this.oriDistType = new Mock<IDistribution>();
            this.sensor = new MarkerSensor(this.mlocmoq, this.posDistType.Object, this.oriDistType.Object);
        }

        /// <summary>
        /// Test regular update
        /// </summary>
        [Test]
        public void TestUpdate()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(0, 180, 0)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(0, 180, 0));
            dic.Add(2, pose2);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            Assert.AreEqual(13, this.sensor.GetLastPosition().Data.X, this.epsilon);
        }

        /// <summary>
        /// Test if the loop continues when an exception will be thrown.
        /// </summary>
        [Test]
        public void TestUpdateUnknownMarker()
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(0, 180, 0)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(0, 180, 0));
            dic.Add(-1337, pose2);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            Assert.AreEqual(13, this.sensor.GetLastPosition().Data.X, this.epsilon);
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
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
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
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
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
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
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
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
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
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
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
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            Assert.Null(this.sensor.GetPosition(-50));
        }

        /// <summary>
        /// Test for when the buffer size gets exceeded.
        /// </summary>
        [Test]
        public void TestOverflow()
        {
            this.sensor = new MarkerSensor(this.mlocmoq, 6, this.posDistType.Object, this.oriDistType.Object);
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            Assert.AreEqual(6, this.sensor.GetAllOrientations().Count);
        }

        /// <summary>
        /// Test for when one iteration had no data.
        /// </summary>
        [Test]
        public void TestNoData()
        {
            this.sensor = new MarkerSensor(this.mlocmoq, 6, this.posDistType.Object, this.oriDistType.Object);
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, new Pose(new Vector3(1, 2, 3), new Vector3(90, 180, 270)));
            Pose pose2 = new Pose(new Vector3(4, 5, 6), new Vector3(90, 180, 270));
            dic.Add(0, pose2);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            this.sensor.UpdateLocations(this.defaultTimeStamp, dic);
            this.sensor.UpdateLocations(this.defaultTimeStamp, new Dictionary<int, Pose>());
            Assert.AreEqual(6, this.sensor.GetAllOrientations().Count);
        }

        /// <summary>
        /// Test that a simple relative and relative rotation of a marker return the correct
        /// position of the user in the world.
        /// </summary>
        [Test]
        public void SimpleUserPositionTest()
        {
            Pose marker = new Pose(new Vector3(3, 0, 1), new Vector3(0, 0, 0));
            Pose rel = new Pose(new Vector3(1, 0, 1), new Vector3(0, -90, 0));
            MarkerLocations mloc = new MarkerLocations();
            mloc.AddMarker(1, marker);
            this.sensor = new MarkerSensor(mloc, 5, this.posDistType.Object, this.oriDistType.Object);
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, rel);
            this.sensor.UpdateLocations(1, dic);
            this.AssertVectorAreEqual(new Vector3(2, 0, 2), this.sensor.GetLastPosition().Data, this.vectorEpsilon);
        }

        /// <summary>
        /// Test that getting an orientation and position with the time stamp from when
        /// the time the measurement was added return the correct output.
        /// </summary>
        [Test]
        public void GetMeasurementAtTimeStamp()
        {
            Pose marker = new Pose(new Vector3(2, 0, 1), new Vector3(0, 0, 0));
            Pose rel = new Pose(new Vector3(1, 0, 1), new Vector3(0, 180, 0));
            MarkerLocations mloc = new MarkerLocations();
            mloc.AddMarker(1, marker);
            this.sensor = new MarkerSensor(mloc, 5, this.posDistType.Object, this.oriDistType.Object);
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, rel);
            this.sensor.UpdateLocations(1, dic);
            this.AssertVectorAreEqual(new Vector3(3, 0, 2), this.sensor.GetPosition(1).Data, this.vectorEpsilon);
            this.AssertRotationAreEqual(new Vector3(0, 180, 0), this.sensor.GetOrientation(1).Data, this.vectorEpsilon);
        }

        /// <summary>
        /// Test that a simple relative measurement gives the correct user position in the world.
        /// </summary>
        [Test]
        public void SimpleUserPositionTest2()
        {
            Pose marker = new Pose(new Vector3(1, 0, 1), new Vector3(0, 0, 0));
            Pose rel = new Pose(new Vector3(2, 0, 1), new Vector3(0, 180, 0));
            MarkerLocations mloc = new MarkerLocations();
            mloc.AddMarker(1, marker);
            this.sensor = new MarkerSensor(mloc, 5, this.posDistType.Object, this.oriDistType.Object);
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(1, rel);
            this.sensor.UpdateLocations(1, dic);
            this.AssertVectorAreEqual(new Vector3(3, 0, 2), this.sensor.GetLastPosition().Data, this.vectorEpsilon);
        }

        /// <summary>
        /// Test testing on several inputs to assure that the correct world position and orientation are computed
        /// from the relative marker position and orientation. These values are more complex.
        /// </summary>
        [Test]
        public void CombinedPositionAndOrientationInOutTest()
        {
            this.mlocmoq = new MarkerLocations();
            this.sensor = new MarkerSensor(this.mlocmoq, 5, this.posDistType.Object, this.oriDistType.Object);
            for (int i = 0; i < this.bulkData.Length; i++)
            {
                Pose marker = new Pose(
                    new Vector3(
                        this.bulkData[i][0],
                        this.bulkData[i][1],
                        this.bulkData[i][2]),
                    new Vector3(
                        this.bulkData[i][3],
                        this.bulkData[i][4],
                        this.bulkData[i][5]));
                Pose measurement = new Pose(
                    new Vector3(
                        this.bulkData[i][6],
                        this.bulkData[i][7],
                        this.bulkData[i][8]),
                    new Vector3(
                        this.bulkData[i][9],
                        this.bulkData[i][10],
                        this.bulkData[i][11]));
                Pose output = new Pose(
                    new Vector3(
                        this.bulkData[i][12],
                        this.bulkData[i][13],
                        this.bulkData[i][14]),
                    new Vector3(
                        this.bulkData[i][15],
                        this.bulkData[i][16],
                        this.bulkData[i][17]));
                this.mlocmoq.AddMarker(i, marker);
                this.TestComplexInOut(measurement, output, i);
            }
        }

        /// <summary>
        /// Test the output for a measurement and an expected output for the
        /// <see cref="CombinedPositionAndOrientationInOutTest"/> test case.
        /// </summary>
        /// <param name="measurement">The measurement to add to the sensor.</param>
        /// <param name="output">The expected output world relative.</param>
        /// <param name="markerID">The id of the seen marker.</param>
        private void TestComplexInOut(Pose measurement, Pose output, int markerID)
        {
            Dictionary<int, Pose> dic = new Dictionary<int, Pose>();
            dic.Add(markerID, measurement);
            this.sensor.UpdateLocations(markerID, dic);
            this.AssertVectorAreEqual(output.Position, this.sensor.GetLastPosition().Data, 0.01);
            this.AssertRotationAreEqual(output.Orientation, this.sensor.GetLastOrientation().Data, 0.01);
        }

        /// <summary>
        /// Test that rotations are equal. As rotations can be achieved by different rotations in
        /// different orders around different axes the method creates a rotation matrix for the expected
        /// and actual values and compares the result of a multiplication with a reference vector. When
        /// the output of the multiplication is the same the orientation is also similar.
        /// </summary>
        /// <param name="expected">The expected rotation.</param>
        /// <param name="actual">The actual rotation.</param>
        /// <param name="epsilon">The epsilon to use in floating point comparison.</param>
        private void AssertRotationAreEqual(Vector3 expected, Vector3 actual, double epsilon)
        {
            RotationMatrix rotExpected = new RotationMatrix(expected.X, expected.Y, expected.Z);
            RotationMatrix rotActual = new RotationMatrix(actual.X, actual.Y, actual.Z);
            Vector3 vecExpected = new Vector3(1, 1, 1);
            Vector3 vecActual = new Vector3(1, 1, 1);
            rotExpected.Multiply(vecExpected, vecExpected);
            rotActual.Multiply(vecActual, vecActual);
            this.AssertVectorAreEqual(vecExpected, vecActual, epsilon);
        }

        /// <summary>
        /// Assert that all elements in the vectors match with possible deviation set by the epsilon.
        /// </summary>
        /// <param name="expected">The expected vector.</param>
        /// <param name="actual">The actual vector.</param>
        /// <param name="epsilon">The epsilon for the floating point value comparison.</param>
        private void AssertVectorAreEqual(Vector3 expected, Vector3 actual, double epsilon)
        {
            Assert.AreEqual(expected.X, actual.X, epsilon);
            Assert.AreEqual(expected.Y, actual.Y, epsilon);
            Assert.AreEqual(expected.Z, actual.Z, epsilon);
        }
    }
}
