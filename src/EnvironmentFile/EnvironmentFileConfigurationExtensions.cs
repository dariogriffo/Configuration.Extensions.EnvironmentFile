using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace Configuration.Extensions.EnvironmentFile;

public static class EnvironmentFileConfigurationExtensions
{
    /// <summary>
    /// Adds Unix style Environment files to IConfigurationBuilder for .NET applications
    /// </summary>
    /// <param name="builder">the <see cref="IConfigurationBuilder"/></param>
    /// <param name="fileName">The name of the file, defaults to .env</param>
    /// <param name="trim">If true the spaces are trimmed to left and right of values. Defaults to true</param>
    /// <param name="removeWrappingQuotes">If true remove wrapping quotes from values. Defaults to true</param>
    /// <param name="prefix">If specified, the prefix is removed from the key name when loaded into the configuration</param>
    /// <param name="reloadOnChange">If true, the file is watched for changes to reload the configuration. Defaults to false</param>
    /// <returns></returns>
    public static IConfigurationBuilder AddEnvironmentFile(
        this IConfigurationBuilder builder,
        string fileName = ".env",
        bool trim = true,
        bool removeWrappingQuotes = true,
        string? prefix = null,
        bool reloadOnChange = false
    )
    {
        string? directory;
        if (!Path.IsPathRooted(fileName))
        {
            directory =
                builder.Properties.TryGetValue("FileProvider", out object? p)
                && p is FileConfigurationProvider configurationProvider
                    ? Path.GetDirectoryName(configurationProvider.Source.Path)
                    : Directory.GetCurrentDirectory();
        }
        else
        {
            directory = EnsureTrailingSlash(Path.GetFullPath(fileName));
        }

        Action<EnvironmentFileConfigurationSource> configureSource = s =>
        {
            s.Prefix = prefix;
            s.RemoveWrappingQuotes = removeWrappingQuotes;
            s.Trim = trim;
            s.Path = fileName;
            s.Optional = true;
            s.ReloadOnChange = reloadOnChange;
            s.FileProvider = new PhysicalFileProvider(directory!, ExclusionFilters.None);
        };
        return builder.Add(configureSource);
    }

    private static string EnsureTrailingSlash(string path)
    {
        return !string.IsNullOrEmpty(path) && path[path.Length - 1] != Path.DirectorySeparatorChar
            ? path + Path.DirectorySeparatorChar
            : path;
    }
}
