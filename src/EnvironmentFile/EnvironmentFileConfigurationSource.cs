using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("Configuration.Extensions.EnvironmentFile.UnitTests")]

namespace Configuration.Extensions.EnvironmentFile
{
    internal class EnvironmentFileConfigurationSource : IConfigurationSource
    {
        internal EnvironmentFileConfigurationSource(IEnumerable<KeyValuePair<string, string>> initialData)
        {
            InitialData = initialData;
        }

        internal IEnumerable<KeyValuePair<string, string>> InitialData { get; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EnvironmentFileConfigurationProvider(this);
        }
    }
}
