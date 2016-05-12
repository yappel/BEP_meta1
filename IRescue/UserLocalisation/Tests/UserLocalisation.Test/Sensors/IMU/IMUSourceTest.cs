// <copyright file="IMUSourceTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace UserLocalisation.Test.Sensors.IMU
{
    using System.Collections.Generic;
    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Sensors.IMU;
    using NUnit.Framework;

    /// <summary>
    /// Test class for the IMUSource class. Tests the getters and the setters.
    /// Also tests that all body and world relative frame conversions are done correctly.
    /// </summary>
    public class IMUSourceTest
    {
        /// <summary>
        /// Source to test.
        /// </summary>
        private IMUSource source;

        /// <summary>
        /// Vector with orientation direction 0,0,0.
        /// </summary>
        private Vector3 zeroOrientation;

        /// <summary>
        /// Vector with a standard acceleration.
        /// </summary>
        private Vector3 standardAcceleration;

        /// <summary>
        /// The default acceleration measurement standard deviation.
        /// </summary>
        private float accelerationStd = 0;

        /// <summary>
        /// The default orientation measurement standard deviation.
        /// </summary>
        private float orientationStd = 0;

        /// <summary>
        /// The default buffer size.
        /// </summary>
        private int bufferSize = 5;

        /// <summary>
        /// Setup the source to test and the default values.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.source = new IMUSource(this.accelerationStd, this.orientationStd, this.bufferSize, new Vector3(new float[] { 0, 0, 0 }));
            this.zeroOrientation = new Vector3(0, 0, 0);
            this.standardAcceleration = new Vector3(1, 2, 3);
        }

        /// <summary>
        /// Test the get acceleration 
        /// </summary>
        [Test]
        public void GetAccelerationTimeStampTest()
        {
            this.source.AddMeasurements(0, this.standardAcceleration, this.zeroOrientation);
            Measurement<Vector3> res = this.source.GetAcceleration(0);
            Assert.AreEqual(this.standardAcceleration.X, res.Data.X);
            Assert.AreEqual(this.standardAcceleration.Y, res.Data.Y);
            Assert.AreEqual(this.standardAcceleration.Z, res.Data.Z);
            Assert.AreEqual(0, res.TimeStamp);
            Assert.AreEqual(this.accelerationStd, res.Std);
        }

        /// <summary>
        /// Test that get accelerations returns the correct measurements.
        /// </summary>
        [Test]
        public void GetAccelerationsPeriodTest()
        {
            Vector3 v1 = new Vector3(1, 1, 1);
            Vector3 v2 = new Vector3(2, 2, 2);
            Vector3 v3 = new Vector3(3, 3, 3);
            this.source.AddMeasurements(0, this.standardAcceleration, this.zeroOrientation);
            this.source.AddMeasurements(1, v1, this.zeroOrientation);
            this.source.AddMeasurements(2, v2, this.zeroOrientation);
            this.source.AddMeasurements(3, v3, this.zeroOrientation);
            List<Measurement<Vector3>> res = this.source.GetAccelerations(1, 3);
            Assert.AreEqual(3, res.Count);
            this.AssertVectorAreEqual(v1, res[0].Data);
            this.AssertVectorAreEqual(v2, res[1].Data);
            this.AssertVectorAreEqual(v3, res[2].Data);
            Assert.AreEqual(1, res[0].TimeStamp);
            Assert.AreEqual(2, res[1].TimeStamp);
            Assert.AreEqual(3, res[2].TimeStamp);
        }

        /// <summary>
        /// Test that get all accelerations returns all the measurements.
        /// </summary>
        [Test]
        public void GetAllAccelerationsTest()
        {
            Vector3 v1 = new Vector3(1, 1, 1);
            Vector3 v2 = new Vector3(2, 2, 2);
            Vector3 v3 = new Vector3(3, 3, 3);
            this.source.AddMeasurements(0, this.standardAcceleration, this.zeroOrientation);
            this.source.AddMeasurements(1, v1, this.zeroOrientation);
            this.source.AddMeasurements(2, v2, this.zeroOrientation);
            this.source.AddMeasurements(3, v3, this.zeroOrientation);
            List<Measurement<Vector3>> res = this.source.GetAllAccelerations();
            Assert.AreEqual(4, res.Count);
            this.AssertVectorAreEqual(this.standardAcceleration, res[0].Data);
            this.AssertVectorAreEqual(v1, res[1].Data);
            this.AssertVectorAreEqual(v2, res[2].Data);
            this.AssertVectorAreEqual(v3, res[3].Data);
            Assert.AreEqual(0, res[0].TimeStamp);
            Assert.AreEqual(1, res[1].TimeStamp);
            Assert.AreEqual(2, res[2].TimeStamp);
            Assert.AreEqual(3, res[3].TimeStamp);
        }

        /// <summary>
        /// Test that get last acceleration returns the correct acceleration.
        /// </summary>
        [Test]
        public void GetLastAccelerationTest()
        {
            Vector3 acc = new Vector3(1, 2, 3);
            this.source.AddMeasurements(0, this.standardAcceleration, this.zeroOrientation);
            this.source.AddMeasurements(1, acc, this.zeroOrientation);
            Measurement<Vector3> res = this.source.GetLastAcceleration();
            Assert.AreEqual(1, res.TimeStamp);
            Assert.AreEqual(this.accelerationStd, res.Std);
            this.AssertVectorAreEqual(acc, res.Data);
        }

        /// <summary>
        /// Test the get orientation. 
        /// </summary>
        [Test]
        public void GetOrientationTimeStampTest()
        {
            Vector3 or = new Vector3(1, 2, 3);
            this.source.AddMeasurements(0, this.standardAcceleration, or);
            Measurement<Vector3> res = this.source.GetOrientation(0);
            this.AssertVectorAreEqual(or, res.Data);
            Assert.AreEqual(0, res.TimeStamp);
            Assert.AreEqual(this.orientationStd, res.Std);
        }

        /// <summary>
        /// Test that get orientations returns the correct measurements.
        /// </summary>
        [Test]
        public void GetOrientationsPeriodTest()
        {
            Vector3 o1 = new Vector3(1, 1, 1);
            Vector3 o2 = new Vector3(2, 2, 2);
            Vector3 o3 = new Vector3(3, 3, 3);
            this.source.AddMeasurements(0, this.standardAcceleration, this.zeroOrientation);
            this.source.AddMeasurements(1, this.standardAcceleration, o1);
            this.source.AddMeasurements(2, this.standardAcceleration, o2);
            this.source.AddMeasurements(3, this.standardAcceleration, o3);
            List<Measurement<Vector3>> res = this.source.GetOrientations(1, 3);
            Assert.AreEqual(3, res.Count);
            this.AssertVectorAreEqual(o1, res[0].Data);
            this.AssertVectorAreEqual(o2, res[1].Data);
            this.AssertVectorAreEqual(o3, res[2].Data);
            Assert.AreEqual(1, res[0].TimeStamp);
            Assert.AreEqual(2, res[1].TimeStamp);
            Assert.AreEqual(3, res[2].TimeStamp);
        }

        /// <summary>
        /// Test that get all accelerations returns all the measurements.
        /// </summary>
        [Test]
        public void GetAllOrientationsTest()
        {
            Vector3 o1 = new Vector3(1, 1, 1);
            Vector3 o2 = new Vector3(2, 2, 2);
            Vector3 o3 = new Vector3(3, 3, 3);
            this.source.AddMeasurements(0, this.standardAcceleration, this.zeroOrientation);
            this.source.AddMeasurements(1, this.standardAcceleration, o1);
            this.source.AddMeasurements(2, this.standardAcceleration, o2);
            this.source.AddMeasurements(3, this.standardAcceleration, o3);
            List<Measurement<Vector3>> res = this.source.GetAllOrientations();
            Assert.AreEqual(4, res.Count);
            this.AssertVectorAreEqual(this.zeroOrientation, res[0].Data);
            this.AssertVectorAreEqual(o1, res[1].Data);
            this.AssertVectorAreEqual(o2, res[2].Data);
            this.AssertVectorAreEqual(o3, res[3].Data);
            Assert.AreEqual(0, res[0].TimeStamp);
            Assert.AreEqual(1, res[1].TimeStamp);
            Assert.AreEqual(2, res[2].TimeStamp);
            Assert.AreEqual(3, res[3].TimeStamp);
        }

        /// <summary>
        /// Test that get last orientation returns the correct orientation.
        /// </summary>
        [Test]
        public void GetLastOrientationTest()
        {
            Vector3 or = new Vector3(1f, 2f, 3f);
            this.source.AddMeasurements(0, this.standardAcceleration, this.zeroOrientation);
            this.source.AddMeasurements(1, this.standardAcceleration, or);
            Measurement<Vector3> res = this.source.GetLastOrientation();
            Assert.AreEqual(1, res.TimeStamp);
            Assert.AreEqual(this.orientationStd, res.Std);
            this.AssertVectorAreEqual(or, res.Data);
        }

        /// <summary>
        /// Test that get last velocity returns the starting velocity when there is no data.
        /// </summary>
        [Test]
        public void GetLastVelocityNoDataTest()
        {
            Vector3 v0 = new Vector3(1, 2, 3);
            this.source = new IMUSource(this.accelerationStd, this.orientationStd, 5, new Vector3(0, 0, 0), v0);
            Measurement<Vector3> res = this.source.GetLastVelocity();
            this.AssertVectorAreEqual(v0, res.Data);
        }

        /// <summary>
        /// Test that get last velocity returns the correct velocity when data is supplied.
        /// </summary>
        [Test]
        public void GetLastVelocityTest()
        {
            Vector3 acc0 = new Vector3(0, 0, 0);
            Vector3 acc1 = new Vector3(1, 2, 3);
            this.source.AddMeasurements(0, acc0, this.zeroOrientation);
            this.source.AddMeasurements(1000, acc1, this.zeroOrientation);
            Measurement<Vector3> res = this.source.GetLastVelocity();
            this.AssertVectorAreEqual(new Vector3(0.5f, 1, 1.5f), res.Data);
            Assert.AreEqual(1000, res.TimeStamp);
        }

        /// <summary>
        /// Test that get velocity at a specified time stamp returns the correct velocity.
        /// </summary>
        [Test]
        public void GetVelocityAtTimeStampTest()
        {
            Vector3 acc0 = new Vector3(0, 0, 0);
            Vector3 acc1 = new Vector3(1, 2, 3);
            this.source.AddMeasurements(0, acc0, this.zeroOrientation);
            this.source.AddMeasurements(1000, acc1, this.zeroOrientation);
            this.source.AddMeasurements(2000, acc0, this.zeroOrientation);
            Measurement<Vector3> res = this.source.GetVelocity(1000);
            this.AssertVectorAreEqual(new Vector3(0.5f, 1, 1.5f), res.Data);
            Assert.AreEqual(1000, res.TimeStamp);
        }

        /// <summary>
        /// Test that the velocities over a period are correctly computed.
        /// </summary>
        [Test]
        public void GetVelocitiesPeriodTest()
        {
            Vector3 acc0 = new Vector3(0, 0, 0);
            Vector3 acc1 = new Vector3(1, 2, 3);
            this.source.AddMeasurements(0, acc0, this.zeroOrientation);
            this.source.AddMeasurements(1000, acc1, this.zeroOrientation);
            this.source.AddMeasurements(2000, acc0, this.zeroOrientation);
            this.source.AddMeasurements(3000, acc1, this.zeroOrientation);
            List<Measurement<Vector3>> res = this.source.GetVelocities(1000, 3000);
            Assert.AreEqual(3, res.Count);
            this.AssertVectorAreEqual(new Vector3(0.5f, 1, 1.5f), res[0].Data);
            this.AssertVectorAreEqual(new Vector3(1, 2, 3), res[1].Data);
            this.AssertVectorAreEqual(new Vector3(1.5f, 3, 4.5f), res[2].Data);
        }

        /// <summary>
        /// Test that all velocities are returned.
        /// </summary>
        [Test]
        public void GetAllVelocitiesTest()
        {
            Vector3 acc0 = new Vector3(0, 0, 0);
            Vector3 acc1 = new Vector3(1, 2, 3);
            this.source.AddMeasurements(0, acc0, this.zeroOrientation);
            this.source.AddMeasurements(1000, acc1, this.zeroOrientation);
            this.source.AddMeasurements(2000, acc0, this.zeroOrientation);
            this.source.AddMeasurements(3000, acc1, this.zeroOrientation);
            List<Measurement<Vector3>> res = this.source.GetAllVelocities();
            Assert.AreEqual(4, res.Count);
            this.AssertVectorAreEqual(new Vector3(0, 0, 0), res[0].Data);
            this.AssertVectorAreEqual(new Vector3(0.5f, 1, 1.5f), res[1].Data);
            this.AssertVectorAreEqual(new Vector3(1, 2, 3), res[2].Data);
            this.AssertVectorAreEqual(new Vector3(1.5f, 3, 4.5f), res[3].Data);
        }

        /// <summary>
        /// Test 90 degrees rotation around Y axis and acceleration in X axis in body frame.
        /// Should result in acceleration in X axis.
        /// </summary>
        [Test]
        public void GetAccelerationTestWorldRelativePositiveAcceleration1Test()
        {
            Vector3 orientation = new Vector3(new float[] { 0, 90, 0 });
            Vector3 acceleration = new Vector3(new float[] { 0, 0, 1 });
            this.source.AddMeasurements(0L, acceleration, orientation);
            Vector3 res = this.source.GetAcceleration(0L).Data;
            Assert.AreEqual(1, res.X, 0.0001);
            Assert.AreEqual(0, res.Y, 0.0001);
            Assert.AreEqual(0, res.Z, 0.0001);
        }

        /// <summary>
        /// Test -90 degrees rotation around Y axis and acceleration in X axis in body frame.
        /// Should result in acceleration in Z axis.
        /// </summary>
        [Test]
        public void GetAccelerationTestWorldRelativePossitiveAcceleration2Test()
        {
            Vector3 orientation = new Vector3(new float[] { 0, -90, 0 });
            Vector3 acceleration = new Vector3(new float[] { 1, 0, 0 });
            this.source.AddMeasurements(0L, acceleration, orientation);
            Vector3 res = this.source.GetAcceleration(0L).Data;
            Assert.AreEqual(0, res.X, 0.0001);
            Assert.AreEqual(0, res.Y, 0.0001);
            Assert.AreEqual(1, res.Z, 0.0001);
        }

        /// <summary>
        /// Test rotation of 90 degrees around Z axis and acceleration in body frame in Y axis.
        /// This must result in a negative acceleration in the X axis.
        /// </summary>
        [Test]
        public void GetAccelerationTestWorldRelativePossitiveAcceleration3Test()
        {
            Vector3 orientation = new Vector3(new float[] { 0, 0, 90 });
            Vector3 acceleration = new Vector3(new float[] { 0, 1, 0 });
            this.source.AddMeasurements(0L, acceleration, orientation);
            Vector3 res = this.source.GetAcceleration(0L).Data;
            Assert.AreEqual(-1, res.X, 0.0001);
            Assert.AreEqual(0, res.Y, 0.0001);
            Assert.AreEqual(0, res.Z, 0.0001);
        }

        /// <summary>
        /// Test -90 degrees rotation around the X axis and body frame acceleration in the Z axis.
        /// Should result in acceleration in Y axis.
        /// </summary>
        [Test]
        public void GetAccelerationTestWorldRelativePossitiveAcceleration4Test()
        {
            Vector3 orientation = new Vector3(new float[] { -90, 0, 0 });
            Vector3 acceleration = new Vector3(new float[] { 0, 0, 1 });
            this.source.AddMeasurements(0L, acceleration, orientation);
            Vector3 res = this.source.GetAcceleration(0L).Data;
            Assert.AreEqual(0, res.X, 0.0001);
            Assert.AreEqual(1, res.Y, 0.0001);
            Assert.AreEqual(0, res.Z, 0.0001);
        }

        /// <summary>
        /// Test the correct displacement when there is no orientation.
        /// </summary>
        [Test]
        public void GetDisplacementNoOrientationTest()
        {
            this.source.AddMeasurements(0, new Vector3(new float[] { 0, 0, 0 }), this.zeroOrientation);
            this.source.AddMeasurements(1000, new Vector3(new float[] { 2, 4, 6 }), this.zeroOrientation);
            this.source.AddMeasurements(2000, new Vector3(new float[] { 0, 0, 0 }), this.zeroOrientation);
            this.source.AddMeasurements(3000, new Vector3(new float[] { 0, 0, 0 }), this.zeroOrientation);
            Vector3 res = this.source.GetDisplacement(0, 3000).Data;
            Assert.AreEqual(4, res.X, 0.0001);
            Assert.AreEqual(8, res.Y, 0.0001);
            Assert.AreEqual(12, res.Z, 0.0001);
        }

        /// <summary>
        /// Test the correct displacement with negative acceleration when there is no orientation.
        /// </summary>
        [Test]
        public void GetDisplacementNegativeAccelerationNoOrientationTest()
        {
            this.source.AddMeasurements(0, new Vector3(new float[] { 0, 0, 0 }), this.zeroOrientation);
            this.source.AddMeasurements(1000, new Vector3(new float[] { -2, -4, -6 }), this.zeroOrientation);
            this.source.AddMeasurements(2000, new Vector3(new float[] { 0, 0, 0 }), this.zeroOrientation);
            this.source.AddMeasurements(3000, new Vector3(new float[] { 0, 0, 0 }), this.zeroOrientation);
            Vector3 res = this.source.GetDisplacement(0, 3000).Data;
            Assert.AreEqual(-4, res.X, 0.0001);
            Assert.AreEqual(-8, res.Y, 0.0001);
            Assert.AreEqual(-12, res.Z, 0.0001);
        }

        /// <summary>
        /// Test the correct displacement when there is an orientation around the Y axis.
        /// </summary>
        [Test]
        public void GetDisplacementYAxisOrientationTest()
        {
            Vector3 orientation = new Vector3(new float[] { 0, 90, 0 });
            this.source.AddMeasurements(0, new Vector3(new float[] { 0, 0, 0 }), orientation);
            this.source.AddMeasurements(1000, new Vector3(new float[] { 2, 4, 6 }), orientation);
            this.source.AddMeasurements(2000, new Vector3(new float[] { 0, 0, 0 }), orientation);
            this.source.AddMeasurements(3000, new Vector3(new float[] { 0, 0, 0 }), orientation);
            Vector3 res = this.source.GetDisplacement(0, 3000).Data;
            Assert.AreEqual(12, res.X, 0.0001);
            Assert.AreEqual(8, res.Y, 0.0001);
            Assert.AreEqual(-4, res.Z, 0.0001);
        }

        /// <summary>
        /// Test the correct displacement when there is an orientation around the X axis.
        /// </summary>
        [Test]
        public void GetDisplacementXAxisOrientationTest()
        {
            Vector3 orientation = new Vector3(new float[] { 90, 0, 0 });
            this.source.AddMeasurements(0, new Vector3(new float[] { 0, 0, 0 }), orientation);
            this.source.AddMeasurements(1000, new Vector3(new float[] { 2, 4, 6 }), orientation);
            this.source.AddMeasurements(2000, new Vector3(new float[] { 0, 0, 0 }), orientation);
            this.source.AddMeasurements(3000, new Vector3(new float[] { 0, 0, 0 }), orientation);
            Vector3 res = this.source.GetDisplacement(0, 3000).Data;
            Assert.AreEqual(4, res.X, 0.0001);
            Assert.AreEqual(-12, res.Y, 0.0001);
            Assert.AreEqual(8, res.Z, 0.0001);
        }

        /// <summary>
        /// Test the correct displacement when there is an orientation around the Z axis.
        /// </summary>
        [Test]
        public void GetDisplacementZAxisOrientationTest()
        {
            Vector3 orientation = new Vector3(new float[] { 0, 0, 90 });
            this.source.AddMeasurements(0, new Vector3(new float[] { 0, 0, 0 }), orientation);
            this.source.AddMeasurements(1000, new Vector3(new float[] { 2, 4, 6 }), orientation);
            this.source.AddMeasurements(2000, new Vector3(new float[] { 0, 0, 0 }), orientation);
            this.source.AddMeasurements(3000, new Vector3(new float[] { 0, 0, 0 }), orientation);
            Vector3 res = this.source.GetDisplacement(0, 3000).Data;
            Assert.AreEqual(-8, res.X, 0.0001);
            Assert.AreEqual(4, res.Y, 0.0001);
            Assert.AreEqual(12, res.Z, 0.0001);
        }

        /// <summary>
        /// Assert that all elements in the vectors match with possible deviation 0.0001.
        /// </summary>
        /// <param name="expected">The expected vector.</param>
        /// <param name="actual">The actual vector.</param>
        private void AssertVectorAreEqual(Vector3 expected, Vector3 actual)
        {
            Assert.AreEqual(expected.X, actual.X, 0.0001);
            Assert.AreEqual(expected.Y, actual.Y, 0.0001);
            Assert.AreEqual(expected.Z, actual.Z, 0.0001);
        }
    }
}
