using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Configuration.Extensions.EnvironmentFile
{
    public static class EnvironmentFileConfigurationExtensions
    {
        public static IConfigurationBuilder AddEnvironmentFile(
            this IConfigurationBuilder builder,
            string fileName = ".env",
            bool trim = true,
            bool removeWrappingQuotes = true,
            string prefix = null,
            bool reloadOnChange = false)
        {
            
            var filePath = fileName;
            if (!Path.IsPathRooted(fileName))
            {
                var directory = builder.Properties.TryGetValue("FileProvider", out var p) && p is FileConfigurationProvider configurationProvider
                    ? Path.GetDirectoryName(configurationProvider.Source.Path)
                    : Directory.GetCurrentDirectory();

                filePath = Path.Combine(directory, fileName);
            }
            
            if (!File.Exists(filePath))
            {
                return builder;
            }
            var provider =
                new EnvironmentFileConfigurationProvider(filePath, trim, removeWrappingQuotes, prefix, reloadOnChange);

            return builder.Add(new EnvironmentFileConfigurationSource(provider));
            
        }
    }
}
