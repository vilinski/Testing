using System;
using TestImplementation.UnderTestClasses;
using Moq;
using Xunit;
using Xunit.Abstractions;
using DotNet.Testing.AutoMoqFixture;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace TestImplementation
{
    public class AutoMoqFixtureTests
    {
        private ITestOutputHelper _logger;


        public AutoMoqFixtureTests(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        [Fact]
        public void Resolve_ByDefault_FirstClass()
        {
            var moq = new AutoMoqFixture();
            var firstClass = moq.Create<FirstClass>();

            var result = firstClass.TriggerMethod();

            Assert.True(result);
            Assert.NotNull(firstClass.GetSecondClass);
            Assert.NotNull(firstClass.GetSecondClass.Property);
            Assert.NotNull(firstClass.GetSecondClass.GetMyInterface.Model);
            Assert.Equal(0, firstClass.GetSecondClass.GetMyInterface.Model.MyInteger);
            Assert.Null(firstClass.GetSecondClass.GetMyInterface.Model.MyString);
        }

        [Fact]
        public void Resolve_ConfigureMember_FirstClass()
        {
            var moq = new AutoMoqFixture(true);
            var firstClass = moq.Create<FirstClass>();

            var result = firstClass.TriggerMethod();

            Assert.True(result);
            Assert.NotNull(firstClass.GetSecondClass);
            Assert.NotNull(firstClass.GetSecondClass.Property);
            Assert.NotNull(firstClass.GetSecondClass.GetMyInterface.Model);
            Assert.NotEqual(0, firstClass.GetSecondClass.GetMyInterface.Model.MyInteger);
            Assert.NotNull(firstClass.GetSecondClass.GetMyInterface.Model.MyString);
        }

        [Fact]
        public void Resolve_WithRepeatCount_FirstClass()
        {
            var moq = new AutoMoqFixture();
            moq.RepeatCount = 17;

            var firstClass = moq.Create<FirstClass>();

            Assert.Equal(moq.RepeatCount, firstClass.StrArray.Length);
        }

        [Fact]
        public void Resolve_WithLogging_FirstClass()
        {
            var moq = new AutoMoqFixture(_logger, true);
            
            moq.InjectLogger<FirstClass>();
            moq.InjectLogger<SecondClass>();

            var firstClass = moq.Create<FirstClass>();

            var result = firstClass.TriggerMethod();

            Assert.True(result);
            Assert.NotNull(firstClass.GetSecondClass);
            Assert.NotNull(firstClass.GetSecondClass.Property);
            Assert.NotNull(firstClass.GetSecondClass.GetMyInterface.Model);
            Assert.NotEqual(firstClass.GetSecondClass.GetMyInterface.Model.MyInteger, 0);
            Assert.NotNull(firstClass.GetSecondClass.GetMyInterface.Model.MyString);
        }


        [Fact]
        public void Resolve_ByTransient_SecondClass()
        {
            var moq = new AutoMoqFixture(_logger, true);

            moq.InjectLogger<FirstClass>();
            moq.InjectLogger<SecondClass>();

            var firstClass = moq.Create<FirstClass>();
            var secondClass = moq.Create<SecondClass>();

            Assert.NotEqual(firstClass.GetSecondClass.GetMyInterface.Model.MyString, secondClass.GetMyInterface.Model.MyString);
            Assert.NotEqual(firstClass.GetSecondClass.GetMyInterface.Model.MyInteger, secondClass.GetMyInterface.Model.MyInteger);
            Assert.NotEqual(firstClass.GetSecondClass.Property, secondClass.Property);
        }


        [Fact]
        public void Resolve_BySingle_SecondClass()
        {
            var moq = new AutoMoqFixture(_logger, true);

            moq.InjectLogger<FirstClass>();
            moq.InjectLogger<SecondClass>();
            moq.Freeze<SecondClass>();

            var firstClass = moq.Create<FirstClass>();
            var secondClass = moq.Create<SecondClass>();

            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyString, secondClass.GetMyInterface.Model.MyString);
            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyInteger, secondClass.GetMyInterface.Model.MyInteger);
            Assert.Equal(firstClass.GetSecondClass.Property, secondClass.Property);
        }

        [Fact]
        public void Resolve_ByTransient_IServiceProvider()
        {
            var moq = new AutoMoqFixture(_logger, true);

            moq.InjectLogger<FirstClass>();
            moq.InjectLogger<SecondClass>();

            var provider = moq.Create<IServiceProvider>();

            var firstClass = provider.GetService<FirstClass>();
            var secondClass = provider.GetService<SecondClass>();

            Assert.NotEqual(firstClass.GetSecondClass.GetMyInterface.Model.MyString, secondClass.GetMyInterface.Model.MyString);
            Assert.NotEqual(firstClass.GetSecondClass.GetMyInterface.Model.MyInteger, secondClass.GetMyInterface.Model.MyInteger);
            Assert.NotEqual(firstClass.GetSecondClass.Property, secondClass.Property);
        }

        [Fact]
        public void Resolve_BySingle_IServiceProviders()
        {
            var moq = new AutoMoqFixture(_logger, false);

            moq.InjectLogger<FirstClass>();
            moq.InjectLogger<SecondClass>();

            moq.Freeze<MyInterface>();
            moq.Freeze<SecondClass>();

            var provider = moq.Create<IServiceProvider>();

            var myInterface11 = provider.GetService<MyInterface>();
            var myInterface22 = provider.GetService<MyInterface>();
            Assert.Null(myInterface11.Model.MyString);
            Assert.Equal(myInterface11.Model.MyString, myInterface22.Model.MyString);

            var myInterface111 = provider.GetRequiredService<MyInterface>();
            var myInterface222 = provider.GetRequiredService<MyInterface>();
            Assert.Null(myInterface111.Model.MyString);
            Assert.Equal(myInterface111.Model.MyString, myInterface222.Model.MyString);
            
            var myInterface1111 = provider.GetRequiredService<MyInterface>();
            var myInterface2222 = provider.GetRequiredService<Mock<MyInterface>>();
            Assert.False(Object.ReferenceEquals(myInterface1111, myInterface2222.Object));
            Assert.Null(myInterface1111.Model.MyString);
            Assert.Equal(myInterface1111.Model.MyString, myInterface2222.Object.Model.MyString);

            var myInterface1 = moq.Create<MyInterface>();
            var myInterface2 = moq.Create<MyInterface>();
            Assert.Null(myInterface1.Model.MyString);
            Assert.Equal(myInterface1.Model.MyString, myInterface2.Model.MyString);

            var myInterface = provider.GetService<MyInterface>();
            var secondClass = provider.GetService<SecondClass>();
            var firstClass = provider.GetService<FirstClass>();

            Assert.Null(firstClass.GetSecondClass.GetMyInterface.Model.MyString);
            Assert.NotNull(firstClass.GetSecondClass.Property);
            Assert.True(firstClass.GetSecondClass.GetMyInterface.Model.MyInteger == 0);

            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyString, myInterface.Model.MyString);
            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyString, secondClass.GetMyInterface.Model.MyString);
            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyInteger, secondClass.GetMyInterface.Model.MyInteger);
            Assert.Equal(firstClass.GetSecondClass.Property, secondClass.Property);
            
        }


        [Fact]
        public void Resolve_CreateScope_IServiceProviders()
        {
            var moq = new AutoMoqFixture(_logger, true);

            moq.InjectLogger<FirstClass>();
            moq.InjectLogger<SecondClass>();

            moq.Freeze<MyInterface>();
            moq.Freeze<SecondClass>();

            var provider = moq.Create<IServiceProvider>();
            var serviceScope = provider.CreateScope();
            provider = serviceScope.ServiceProvider;

            var myInterface11 = provider.GetService<MyInterface>();
            var myInterface22 = provider.GetService<MyInterface>();
            Assert.Equal(myInterface11.Model.MyString, myInterface22.Model.MyString);

            var myInterface111 = provider.GetRequiredService<MyInterface>();
            var myInterface222 = provider.GetRequiredService<MyInterface>();
            Assert.Equal(myInterface111.Model.MyString, myInterface222.Model.MyString);

            var myInterface1111 = provider.GetRequiredService<MyInterface>();
            var myInterface2222 = provider.GetRequiredService<IMock<MyInterface>>();
            Assert.Equal(myInterface1111.Model.MyString, myInterface2222.Object.Model.MyString);

            var myInterface1 = moq.Create<MyInterface>();
            var myInterface2 = moq.Create<MyInterface>();
            Assert.Equal(myInterface1.Model.MyString, myInterface2.Model.MyString);

            var myInterface = provider.GetService<MyInterface>();
            var secondClass = provider.GetService<SecondClass>();
            var firstClass = provider.GetService<FirstClass>();

            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyString, myInterface.Model.MyString);

            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyString, secondClass.GetMyInterface.Model.MyString);
            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyInteger, secondClass.GetMyInterface.Model.MyInteger);
            Assert.Equal(firstClass.GetSecondClass.Property, secondClass.Property);

        }
    }
}
