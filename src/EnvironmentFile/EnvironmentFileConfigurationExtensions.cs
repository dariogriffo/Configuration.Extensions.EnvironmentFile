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
            string fileName = ".env")
        {
            if (!File.Exists(fileName))
            {
                return builder;
            }

            var configuration = File
                .ReadAllLines(fileName)
                .Select(x => x.TrimStart())
                .Where(x => !x.StartsWith("#") && x.Contains("="))
                .Select(x => x.Split('='))
                .Select(x => new KeyValuePair<string, string>(x[0].Replace("__", ":"), string.Join("=", x.Skip(1))));

            return builder.Add(new EnvironmentFileConfigurationSource(configuration));
        }
    }
}
