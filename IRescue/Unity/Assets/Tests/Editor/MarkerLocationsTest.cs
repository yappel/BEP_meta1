using NUnit.Framework;

public class MarkerLocationsTest
{
    private IRVector3 position;
    private IRVector3 rotation;
    private MarkerLocations markerLocations;

    [SetUp]
    public void Init()
    {
        position = new IRVector3(1, 2, 3);
        rotation = new IRVector3(4, 5, 6);
    }

    // Test the empty constructor with an empty exception.
    [Test]
    [ExpectedException(typeof(UnallocatedMarkerException))]
    public void TestConstructorEmpty()
    {
        markerLocations = new MarkerLocations();
        markerLocations.GetMarker(1);
    }

    // Test add marker for empty constructor.
    [Test]
    public void TestConstructorEmptyAddMarker()
    {
        markerLocations = new MarkerLocations();
        Marker newMarker = new Marker(1, position, rotation);
        markerLocations.AddMarker(newMarker);
        Assert.AreEqual(markerLocations.GetMarker(1), newMarker);
    }

    // Test add marker exception for empty constructor.
    [Test]
    [ExpectedException(typeof(UnallocatedMarkerException))]
    public void TestConstructorEmptyAddMarkerException()
    {
        markerLocations = new MarkerLocations();
        Marker newMarker = new Marker(1, position, rotation);
        markerLocations.AddMarker(newMarker);
        Assert.AreEqual(markerLocations.GetMarker(2), newMarker);
    }

    // Test the constructor with an XML file.
    [Test]
    public void TestConstructorLoad()
    {
        markerLocations = new MarkerLocations("./Assets/Tests/Resources/MarkerMap01.xml");
        Assert.AreEqual(markerLocations.GetMarker(0).GetPosition().GetX(), 25);
        Assert.AreEqual(markerLocations.GetMarker(0).GetPosition().GetY(), 13);
    }

    // Test the exception when an XML was loaded.
    [Test]
    [ExpectedException(typeof(UnallocatedMarkerException))]
    public void TestConstructorException()
    {
        markerLocations = new MarkerLocations("./Assets/Tests/Resources/MarkerMap01.xml");
        markerLocations.GetMarker(2);
    }

    // Test the exception the XML was wrong.
    [Test]
    [ExpectedException(typeof(UnallocatedMarkerException))]
    public void TestConstructorLoadException()
    {
        markerLocations = new MarkerLocations("./Assets/Tests/Resources/MarkerMapFail.xml");
        markerLocations.GetMarker(1);
    }
}
