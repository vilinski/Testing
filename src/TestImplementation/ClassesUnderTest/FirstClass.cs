
using System;
using Microsoft.Extensions.Logging;

namespace TestImplementation.UnderTestClasses
{
    public class FirstClass
    {
        private ILogger<FirstClass> _logger;
        private SecondClass secondClass;

        public FirstClass(ILogger<FirstClass> logger, SecondClass classUnderTest, string[] strArray)
        {
            _logger = logger;
            secondClass = classUnderTest;
            StrArray = strArray;
        }

        public string[] StrArray { get; set; }

        public bool TriggerMethod()
        {
            _logger.LogDebug("FirstClass Write a debug message");
            secondClass.TriggerMethod(new MyModel(){MyString = "from first class"});

            return true;
        }

        public SecondClass GetSecondClass => secondClass;
    }
}
