using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("Configuration.Extensions.EnvironmentFile.UnitTests")]

namespace Configuration.Extensions.EnvironmentFile
{
    internal class EnvironmentFileConfigurationSource : IConfigurationSource
    {
        private readonly EnvironmentFileConfigurationProvider _provider;

        internal EnvironmentFileConfigurationSource(EnvironmentFileConfigurationProvider provider)
        {
            _provider = provider;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return _provider;
        }
    }
}
