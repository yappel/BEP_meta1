using NUnit.Framework;

public class IRVector3Test {
    private IRVector3 vector;

    // Test for the constructor.
    [Test]
    public void TestConstructor()
    {
        vector = new IRVector3(6, 1, 7);
        Assert.AreEqual(vector.GetX(), 6);
        Assert.AreEqual(vector.GetY(), 1);
        Assert.AreEqual(vector.GetZ(), 7);
    }

    // Test for SetX.
    [Test]
    public void TestSetX()
    {
        vector = new IRVector3(6, 1, 7);
        vector.SetX(12);
        Assert.AreEqual(vector.GetX(), 12);
    }

    // Test for SetY.
    [Test]
    public void TestSetY()
    {
        vector = new IRVector3(6, 1, 7);
        vector.SetY(11);
        Assert.AreEqual(vector.GetY(), 11);
    }

    // Test for SetZ.
    [Test]
    public void TestSetZ()
    {
        vector = new IRVector3(6, 1, 7);
        vector.SetZ(10);
        Assert.AreEqual(vector.GetZ(), 10);
    }
}
