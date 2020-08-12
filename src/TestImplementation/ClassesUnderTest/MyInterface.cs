

namespace TestImplementation.UnderTestClasses
{
    public interface MyInterface
    {
        MyModel Model { get; set; }

        bool TestMethod(MyModel m);
    }
}
