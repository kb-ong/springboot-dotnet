using com.example.demo;

namespace HelloWorldNTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        new Service().logging1();
        new Service().logging2();
        Assert.Pass();
    }
}