using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Configuration.Extensions.EnvironmentFile;

internal static class EnvironmentFileConfigurationParser
{
    public static Dictionary<string, string> Parse(
        Stream stream,
        bool trim,
        bool removeWrappingQuotes,
        string? prefix
    )
    {
        StreamReader reader = new(stream);

        IEnumerable<string> nonCommentLinesWithPropertyValues = reader
            .ReadToEnd()
            .Split(["\r\n", "\r", "\n"], StringSplitOptions.None)
            .Select(x => x.TrimStart())
            .Select(x => string.IsNullOrWhiteSpace(prefix) ? x : x.Replace(prefix, string.Empty))
            .Where(x => !x.StartsWith("#") && x.Contains("="));

        IEnumerable<KeyValuePair<string, string>>? configuration = nonCommentLinesWithPropertyValues
            .Select(ParseQuotes)
            .Select(x => RemoveCommentsAtTheEndAndTrimIfNecessary(x, trim))
            .Select(x => x.Split('='))
            .Select(x => new KeyValuePair<string, string>(
                x[0].Replace("__", ":"),
                string.Join("=", x.Skip(1))
            ));

        Dictionary<string, string> result = new();
        foreach (KeyValuePair<string, string> keyValuePair in configuration)
        {
            result[keyValuePair.Key] = keyValuePair.Value;
        }

        return result;

        string ParseQuotes(string line)
        {
            if (!removeWrappingQuotes)
            {
                return line;
            }

            string[] parts = line.Split('=');
            line = string.Join("=", parts.Skip(1));
            return $"{parts[0]}={line.Trim('"')}";
        }

        static string RemoveCommentsAtTheEndAndTrimIfNecessary(string line, bool t)
        {
            return t ? line.Trim() : line;
        }
    }
}
