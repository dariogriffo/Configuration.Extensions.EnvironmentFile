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
            string prefix = null)
        {
            if (!File.Exists(fileName))
            {
                return builder;
            }

            var nonCommentLinesWithPropertyValues =
                File
                    .ReadAllLines(fileName)
                    .Select(x => x.TrimStart())
                    .Select(x => string.IsNullOrWhiteSpace(prefix) ? x : x.Replace(prefix, string.Empty))
                    .Where(x => !x.StartsWith("#") && x.Contains("="));

            string ParseQuotes(string line)
            {
                if (!removeWrappingQuotes)
                {
                    return line;
                }

                var parts = line.Split('=');
                line = string.Join("=", parts.Skip(1));
                return $"{parts[0]}={line.Trim('"')}";
            }

            string RemoveCommentsAtTheEndAndTrimIfNecessary(string line)
            {
                return trim ? line.Trim() : line;
            }

            var configuration =
                nonCommentLinesWithPropertyValues
                .Select(ParseQuotes)
                .Select(RemoveCommentsAtTheEndAndTrimIfNecessary)
                .Select(x => x.Split('='))
                .Select(x => new KeyValuePair<string, string>(x[0].Replace("__", ":"), string.Join("=", x.Skip(1))));

            return builder.Add(new EnvironmentFileConfigurationSource(configuration));
        }
    }
}
