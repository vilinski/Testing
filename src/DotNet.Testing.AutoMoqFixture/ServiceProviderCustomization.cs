using System;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;

namespace DotNet.Testing.AutoMoqFixture
{
    internal class ServiceProviderCustomization : ICustomization
    {

        public void Customize(IFixture fixture)
        {
            var serviceProviderMock = fixture.Freeze<Mock<IServiceProvider>>();

            // GetService
            serviceProviderMock
                .Setup(m => m.GetService(It.IsAny<Type>()))
                .Returns((Type type) => fixture.Create(type, new SpecimenContext(fixture)));
        }

    }

}
