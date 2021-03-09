using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Configuration.Extensions.EnvironmentFile.UnitTests
{
    public class EnvironmentFileConfigurationExtensionsTests
    {
        [Fact]
        public void AddEnvironmentFile_With_No_Parameters_Loads_Default_File()
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentFile().Build();
            configuration.GetConnectionString("Test").Should().Be("Value");
        }

        [Fact]
        public void AddEnvironmentFile_When_File_Does_Not_Exist_Nothing_IsLoaded()
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentFile("missing-file").Build();
            configuration.GetConnectionString("Test").Should().BeNullOrEmpty();
        }

        [Fact]
        public void AddEnvironmentFile_Only_Works_On_Dev()
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentFile().Build();
            configuration.GetConnectionString("Test").Should().Be("Value");
        }

        [Fact]
        public void AddEnvironmentFile_With_Sections_They_Are_Loaded_Correctly()
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentFile(".env-with-sections").Build();
            configuration.GetSection("Section1").Get<Section1>().Should().NotBeNull();
        }

        [Fact]
        public void AddEnvironmentFile_When_File_Has_Comments_They_Are_Skipped()
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentFile(".env-with-comments").Build();
            configuration.GetConnectionString("Commented").Should().BeNullOrEmpty();
        }

        [Fact]
        public void AddEnvironmentFile_When_File_Has_Invalid_Lines_They_Are_Skipped()
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentFile(".env-with-invalid-lines").Build();
            configuration.GetConnectionString("Test").Should().Be("Value");
        }

        [Fact]
        public void AddEnvironmentFile_When_Configuration_Values_Have_An_Equals_Sign_Value_Is_Correctly_Parsed()
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentFile(".env-with-equals-in-value").Build();
            configuration.GetConnectionString("Test").Should().Be("Value=1");
        }
    }
}
