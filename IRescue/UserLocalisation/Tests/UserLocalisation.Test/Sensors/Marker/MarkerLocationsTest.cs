// <copyright file="MarkerLocationsTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace UserLocalisation.Test.Sensors.Marker
{
    using System.IO;
    using System.Reflection;
    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Sensors.Marker;
    using NUnit.Framework;

    /// <summary>
    /// Test for MarkerLocations
    /// </summary>
    public class MarkerLocationsTest
    {
        /// <summary>
        /// Used position
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// Used rotation
        /// </summary>
        private Vector3 rotation;

        /// <summary>
        /// User MarkerLocations
        /// </summary>
        private MarkerLocations markerLocations;

        /// <summary>
        /// Setup the test
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.position = new Vector3(1, 2, 3);
            this.rotation = new Vector3(4, 5, 6);
        }

        /// <summary>
        /// Test the constructor empty
        /// </summary>
        [Test]
        public void TestConstructorEmpty()
        {
            this.markerLocations = new MarkerLocations();
            try
            {
                this.markerLocations.GetMarker(1);
            }
                catch (UnallocatedMarkerException)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        /// <summary>
        /// Test the constructor empty
        /// </summary>
        [Test]
        public void TestConstructorEmptyAddMarker()
        {
            this.markerLocations = new MarkerLocations();
            Pose newMarker = new Pose(this.position, this.rotation);
            this.markerLocations.AddMarker(1, newMarker);
            Assert.AreEqual(this.markerLocations.GetMarker(1), newMarker);
        }

        /// <summary>
        /// Test the constructor empty exception
        /// </summary>
        [Test]
        public void TestConstructorEmptyAddMarkerException()
        {
            this.markerLocations = new MarkerLocations();
            Pose newMarker = new Pose(this.position, this.rotation);
            this.markerLocations.AddMarker(2, newMarker);
            Assert.AreEqual(this.markerLocations.GetMarker(2), newMarker);
        }

        /// <summary>
        /// Test the constructor with xml
        /// </summary>
        [Test]
        public void TestConstructorLoad()
        {
            this.markerLocations = new MarkerLocations(TestContext.CurrentContext.TestDirectory + "\\MarkerMap01.xml");
            Assert.AreEqual(this.markerLocations.GetMarker(0).Position.X, 25);
            Assert.AreEqual(this.markerLocations.GetMarker(0).Position.Y, 13);
        }

        /// <summary>
        /// Test the constructor exception with xml
        /// </summary>
        [Test]
        public void TestConstructorException()
        {
            this.markerLocations = new MarkerLocations(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\MarkerMap01.xml");
            try
            {
                this.markerLocations.GetMarker(2);
            }
            catch (UnallocatedMarkerException)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        /// <summary>
        /// Test the constructor for wrong xml
        /// </summary>
        [Test]
        public void TestConstructorLoadException()
        {
            this.markerLocations = new MarkerLocations(TestContext.CurrentContext.TestDirectory + "\\MarkerMapFail.xml");
            try
            {
                this.markerLocations.GetMarker(1);
            }
            catch (UnallocatedMarkerException)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }
    }
}