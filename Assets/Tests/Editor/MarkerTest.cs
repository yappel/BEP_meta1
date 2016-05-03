using NUnit.Framework;

public class MarkerTest
{
    private IRVector3 position;
    private IRVector3 rotation;
    private Marker marker;

    [SetUp]
    public void Init()
    {
        position = new IRVector3(1, 2, 3);
        rotation = new IRVector3(4, 5, 6);
    }

    // Test if the constructor works properly.
    [Test]
    public void TestConstructor()
    {
        marker = new Marker(61, position, rotation);
        Assert.AreEqual(marker.GetId(), 61);
    }

    // Test if SetPosition works properly.
    [Test]
    public void TestSetPosition()
    {
        marker = new Marker(61, position, rotation);
        marker.SetPosition(new IRVector3(7, 8, 9));
        Assert.AreEqual(marker.GetPosition().GetX(), 7);
        Assert.AreEqual(marker.GetPosition().GetY(), 8);
        Assert.AreEqual(marker.GetPosition().GetZ(), 9);
    }

    // Test if SetRotation works properly.
    [Test]
    public void TestSetRotation()
    {
        marker = new Marker(61, position, rotation);
        marker.SetRotation(new IRVector3(10, 11, 12));
        Assert.AreEqual(marker.GetRotation().GetX(), 10);
        Assert.AreEqual(marker.GetRotation().GetY(), 11);
        Assert.AreEqual(marker.GetRotation().GetZ(), 12);
    }
}
