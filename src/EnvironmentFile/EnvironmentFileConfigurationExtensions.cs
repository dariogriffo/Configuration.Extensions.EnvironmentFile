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
            bool removeWrappingQuotes = true)
        {
            if (!File.Exists(fileName))
            {
                return builder;
            }

            var nonCommentLinesWithPropertyValues =
                File
                    .ReadAllLines(fileName)
                    .Select(x => x.TrimStart())
                    .Where(x => !x.StartsWith("#") && x.Contains("="));

            string ParseQuotes(string s)
            {
                if (!removeWrappingQuotes)
                {
                    return s;
                }

                var parts = s.Split('=');
                s = string.Join("=", parts.Skip(1));
                return $"{parts[0]}={s.Trim('"')}";
            }

            string RemoveCommentsAtTheEndAndTrimIfNecessary(string s1)
            {
                var value = s1.Split('#')[0];
                return trim ? value.Trim() : value;
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
