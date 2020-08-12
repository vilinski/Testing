using AutoFixture;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace DotNet.Testing.AutoMoqFixture
{
    public class AutoMoqFixture : Fixture
    {
        private ITestOutputHelper _logger;


        public AutoMoqFixture(bool configureMembers=false)
        {
            AddCustomitation(configureMembers);
        }

        public AutoMoqFixture(ITestOutputHelper logger, bool configureMembers = false) : this(configureMembers)
        {
            _logger = logger;
        }

        private void AddCustomitation(bool configureMembers)
        {
            new FixtureBuilder(this)
                .CustomizeAutoMoq(configureMembers)
                .CustomizeServiceProvider();
        }
        
        public bool InjectLogger<T>()
        {
            if (_logger != null)
            {
                var l = (ILogger<T>) _logger.BuildLoggerFor<T>();
                this.Register(() => l);
                return true;
            }
            return false;
        }

    }
}
