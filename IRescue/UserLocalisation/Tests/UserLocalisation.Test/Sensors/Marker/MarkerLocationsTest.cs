namespace UserLocalisation.Test.Sensors.Marker
{
    using IRescue.Core.DataTypes;
    using IRescue.UserLocalisation.Sensors.Marker;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Reflection;
    public class MarkerLocationsTest
    {
        private Vector3 position;
        private Vector3 rotation;
        private MarkerLocations markerLocations;

        [SetUp]
        public void Init()
        {
            position = new Vector3(1, 2, 3);
            rotation = new Vector3(4, 5, 6);
        }

        // Test the empty constructor with an empty exception.
        [Test]
        public void TestConstructorEmpty()
        {
            markerLocations = new MarkerLocations();
            try
            {
                markerLocations.GetMarker(1);
            }
                catch (UnallocatedMarkerException e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        // Test add marker for empty constructor.
        [Test]
        public void TestConstructorEmptyAddMarker()
        {
            markerLocations = new MarkerLocations();
            Pose newMarker = new Pose(position, rotation);
            markerLocations.AddMarker(1, newMarker);
            Assert.AreEqual(markerLocations.GetMarker(1), newMarker);
        }

        // Test add marker exception for empty constructor.
        [Test]
        public void TestConstructorEmptyAddMarkerException()
        {
            markerLocations = new MarkerLocations();
            Pose newMarker = new Pose(position, rotation);
            markerLocations.AddMarker(2, newMarker);
            Assert.AreEqual(markerLocations.GetMarker(2), newMarker);
        }

        // Test the constructor with an XML file.
        [Test]
        public void TestConstructorLoad()
        {
            markerLocations = new MarkerLocations(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\MarkerMap01.xml");
            Assert.AreEqual(markerLocations.GetMarker(0).Position.X, 25);
            Assert.AreEqual(markerLocations.GetMarker(0).Position.Y, 13);
        }

        // Test the exception when an XML was loaded.
        [Test]
        public void TestConstructorException()
        {
            markerLocations = new MarkerLocations(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\MarkerMap01.xml");
            try
            {
                markerLocations.GetMarker(2);
            }
            catch (UnallocatedMarkerException e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        // Test the exception the XML was wrong.
        [Test]
        public void TestConstructorLoadException()
        {
            markerLocations = new MarkerLocations(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\MarkerMapFail.xml");
            try
            {
                markerLocations.GetMarker(1);
            }
            catch (UnallocatedMarkerException e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
    }
}