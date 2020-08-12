using AutoFixture;
using AutoFixture.Xunit2;

namespace DotNet.Testing.AutoMoqFixture
{
    public class AutoMoqFixtureAttribute : AutoDataAttribute
    {
        public AutoMoqFixtureAttribute(int repeatCount = 3, bool configureMembers = false)
            : base(() => GetFixture(repeatCount, configureMembers))
        { }

        public AutoMoqFixtureAttribute(int repeatCount)
            : base(() => GetFixture(repeatCount, false))
        { }

        public AutoMoqFixtureAttribute(bool configureMembers)
            : base(() => GetFixture(3, configureMembers))
        { }

        private static Fixture GetFixture(int repeatCount, bool configureMembers)
        {
            var fixture = new FixtureBuilder()
                .CustomizeAutoMoq(configureMembers)
                .CustomizeServiceProvider()
                .GetFixture();

            fixture.RepeatCount = repeatCount;

            return fixture;
        }
    }
}
