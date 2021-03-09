using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Configuration.Extensions.EnvironmentFile
{
    internal class EnvironmentFileConfigurationProvider : ConfigurationProvider
    {
        internal EnvironmentFileConfigurationProvider(EnvironmentFileConfigurationSource envFileConfigurationSource)
        {
            Data = envFileConfigurationSource.InitialData.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
