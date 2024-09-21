using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Configuration.Extensions.EnvironmentFile;

public class EnvironmentFileConfigurationSource : FileConfigurationSource
{
    public EnvironmentFileConfigurationSource()
    {
        Path = ".env";
    }

    internal string? Prefix { get; set; }
    internal bool RemoveWrappingQuotes { get; set; } = true;
    internal bool Trim { get; set; } = true;

    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new EnvironmentFileConfigurationProvider(this, Trim, RemoveWrappingQuotes, Prefix);
    }
}
