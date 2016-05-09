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
        Assert.AreEqual(10, vector.GetZ());
    }

    // Test if adding another vector to this vector works
    [Test]
    public void TestAdd()
    {
        vector = new IRVector3(1, 2, 3);
        IRVector3 vectorToadd = new IRVector3(1, 1, 1);
        vector.Add(vectorToadd);
        IRVector3 expected = new IRVector3(2, 3, 4);
        Assert.AreEqual(expected,vector);
    }

    // Test the hashcode generation
    [Test]
    public void TestGetHashCode()
    {
        vector = new IRVector3(1, 2, 3);
        int expectedHascode = 1069547520;
        Assert.AreEqual(vector.GetHashCode(), expectedHascode);
    }

    //Test the tostring method
    [Test]
    public void TestToString1()
    {
        vector = new IRVector3(1, 2, 3);
        string expected = "[1 2 3]";
        Assert.AreEqual(vector.ToString(), expected);
    }

    //Test the tostring method to check if the returned string is not always the same
    [Test]
    public void TestToString2()
    {
        vector = new IRVector3(1, 1, 3);
        string notexpected = "[1 2 3]";
        Assert.AreNotEqual(vector.ToString(), notexpected);
    }
}
