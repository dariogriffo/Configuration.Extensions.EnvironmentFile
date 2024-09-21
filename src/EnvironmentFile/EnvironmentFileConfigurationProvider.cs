using System.IO;
using Microsoft.Extensions.Configuration;

namespace Configuration.Extensions.EnvironmentFile;

internal class EnvironmentFileConfigurationProvider : FileConfigurationProvider
{
    private readonly bool _trim;
    private readonly bool _removeWrappingQuotes;
    private readonly string? _prefix;

    public override void Load(Stream stream)
    {
        Data = EnvironmentFileConfigurationParser.Parse(
            stream,
            _trim,
            _removeWrappingQuotes,
            _prefix
        );
    }

    internal EnvironmentFileConfigurationProvider(
        FileConfigurationSource source,
        bool trim,
        bool removeWrappingQuotes,
        string? prefix
    )
        : base(source)
    {
        _trim = trim;
        _removeWrappingQuotes = removeWrappingQuotes;
        _prefix = prefix;
    }
}
