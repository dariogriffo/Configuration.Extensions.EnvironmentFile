using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Configuration.Extensions.EnvironmentFile.UnitTests
{
    public class EnvironmentFileConfigurationSourceTests
    {
        [Fact]
        public void Constructor_Initializes_Data_Correctly()
        {
            var data = new[] { new KeyValuePair<string, string>("key", "value"), };
            var source = new EnvironmentFileConfigurationSource(data);
            source.InitialData.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void Build_Returns_EnvironmentFileConfigurationProvider()
        {
            var data = new[] { new KeyValuePair<string, string>("key", "value"), };
            var source = new EnvironmentFileConfigurationSource(data);
            var builder = Mock.Of<IConfigurationBuilder>();
            source
                .Build(builder)
                .Should()
                .BeOfType<EnvironmentFileConfigurationProvider>()
                .And
                .Subject.As<EnvironmentFileConfigurationProvider>()
                .TryGet("key", out _).Should().BeTrue();
        }
    }
}