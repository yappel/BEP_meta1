using NUnit.Framework;

public class IRDoubleVectorTest
{
    private IRVector3 position;
    private IRVector3 rotation;
    private IRDoubleVector transform;

    [SetUp]
    public void Init()
    {
        position = new IRVector3(1, 2, 3);
        rotation = new IRVector3(4, 5, 6);
    }

    // Test if the constructor assigns variables correctly.
    [Test]
    public void TestConstructor()
    {
        transform = new IRDoubleVector(position, rotation);
        Assert.AreEqual(transform.GetPosition(), position);
        Assert.AreEqual(transform.GetRotation(), rotation);
    }

    // Test if SetPosition works properly.
    [Test]
    public void TestSetPosition()
    {
        transform = new IRDoubleVector(position, rotation);
        transform.SetPosition(new IRVector3(7, 8, 9));
        Assert.AreEqual(transform.GetPosition().GetX(), 7);
        Assert.AreEqual(transform.GetPosition().GetY(), 8);
        Assert.AreEqual(transform.GetPosition().GetZ(), 9);
    }

    // Test if SetRotation works properly
    [Test]
    public void TestSetRotation()
    {
        transform = new IRDoubleVector(position, rotation);
        transform.SetRotation(new IRVector3(10, 11, 12));
        Assert.AreEqual(transform.GetRotation().GetX(), 10);
        Assert.AreEqual(transform.GetRotation().GetY(), 11);
        Assert.AreEqual(transform.GetRotation().GetZ(), 12);
    }
}
