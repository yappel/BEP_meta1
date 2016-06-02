// <copyright file="MarkerLocationsTest.cs" company="Delft University of Technology">
// Copyright (c) Delft University of Technology. All rights reserved.
// </copyright>

namespace UserLocalisation.Test.Sensors.Marker
{
    using System;
    using System.Xml;
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
            Assert.That(() => this.markerLocations.GetMarker(1), Throws.TypeOf<UnallocatedMarkerException>());
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
            Assert.AreEqual(this.markerLocations.GetMarker(0).Position.Y, 13.52f);
        }

        /// <summary>
        /// Test the constructor exception with xml
        /// </summary>
        [Test]
        public void TestConstructorException()
        {
            this.markerLocations = new MarkerLocations(TestContext.CurrentContext.TestDirectory + "\\MarkerMap01.xml");
            Assert.That(() => this.markerLocations.GetMarker(2), Throws.TypeOf<UnallocatedMarkerException>());
        }

        /// <summary>
        /// Test the constructor for wrong xml
        /// </summary>
        [Test]
        public void TestConstructorLoadException()
        {
            Assert.That(() => new MarkerLocations(TestContext.CurrentContext.TestDirectory + "\\MarkerMapFail.xml"), Throws.TypeOf<NullReferenceException>());
        }

        /// <summary>
        /// Test the constructor for wrongly parsed xml
        /// </summary>
        [Test]
        public void TestConstructorFailLoadException()
        {
            Assert.That(() => new MarkerLocations(TestContext.CurrentContext.TestDirectory + "\\MarkerMapFailFormat.xml"), Throws.TypeOf<XmlException>());
        }

        /// <summary>
        /// Test the constructor with xml containing floating point numbers with . separator.
        /// </summary>
        [Test]
        public void TestFloatingPointRead()
        {
            this.markerLocations = new MarkerLocations(TestContext.CurrentContext.TestDirectory + "\\MarkerMapFloat.xml");
            Assert.AreEqual(this.markerLocations.GetMarker(0).Position.X, 25.12f);
            Assert.AreEqual(this.markerLocations.GetMarker(0).Position.Y, 13.52f);
            Assert.AreEqual(this.markerLocations.GetMarker(0).Position.Z, 5.00f);
            Assert.AreEqual(this.markerLocations.GetMarker(0).Orientation.X, 0.15f);
            Assert.AreEqual(this.markerLocations.GetMarker(0).Orientation.Y, 0.23f);
            Assert.AreEqual(this.markerLocations.GetMarker(0).Orientation.Z, 0.00f);
        }
    }
}