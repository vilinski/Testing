using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestImplementation.UnderTestClasses
{
    public class SecondClass
    {
        private readonly MyInterface _myInterface;
        private ILogger<SecondClass> _logger;
        private IServiceProvider _provider;

        public SecondClass(ILogger<SecondClass> logger, MyInterface myInterface, IServiceProvider provider)
        {
            _logger = logger;
            _myInterface = myInterface;
            _provider = provider;
        }

        public string Property { get; set; }

        public MyInterface GetMyInterface => _myInterface;

        public void TriggerMethod(MyModel m)
        {
            _logger.LogDebug("SecondClass Write a debug message with property {Property}!", Property);

            _myInterface.TestMethod(_myInterface.Model);

            var i = _provider.GetService<MyInterface>();
            i.TestMethod(m);

            _logger.LogInformation("SecondClass Write a info message with Interface Model {@Model}!", _myInterface.Model);
            
            var d = _provider.GetRequiredService<SecondClass>();
            d.GetMyInterface.TestMethod(new MyModel(){MyString = "Test"});
        }

    }
}
