using NUnit.Framework;

public class IRVectorDeviationTest
{
    private IRVector3 position;
    private IRVector3 rotation;

    private IRVectorDeviation weight;

    [SetUp]
    public void Init()
    {
        position = new IRVector3(1, 2, 3);
        rotation = new IRVector3(4, 5, 6);
    }

    // Test if the constructor assigns the variables correctly.
    [Test]
    public void TestConstructor()
    {
        weight = new IRVectorDeviation(position, rotation, 5);
        Assert.AreEqual(weight.GetPosition(), position);
        Assert.AreEqual(weight.GetRotation(), rotation);
    }

    // Test if SetStandardDeviation works properly.
    [Test]
    public void TestSetStandardDeviation()
    {
        weight = new IRVectorDeviation(position, rotation, 5);
        weight.SetStandardDeviation(0.5f);
        Assert.AreNotEqual(weight.GetStandardDeviation(), 5);
        Assert.AreEqual(weight.GetStandardDeviation(), 0.5f);
    }
}
