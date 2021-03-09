using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Configuration.Extensions.EnvironmentFile.UnitTests
{
    public class EnvironmentFileConfigurationProviderTests
    {
        [Fact]
        public void Constructor_Initializes_Data_Correctly()
        {
            var data = new[] { new KeyValuePair<string, string>("key", "value"), };
            var source = new EnvironmentFileConfigurationSource(data);
            var provider = new EnvironmentFileConfigurationProvider(source);
            provider.TryGet("key", out var value).Should().BeTrue();
            value.Should().Be("value");
        }
    }
}
