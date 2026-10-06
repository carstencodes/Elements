// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Reflection;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: Copilot
/// <summary>Metadata shared by source files emitted from this generator assembly.</summary>
internal static class GeneratorMetadata
{
    /// <summary>The assembly informational version, file version, or documented fallback.</summary>
    internal static readonly string Version = GetVersion();

    private static string GetVersion()
    {
        Assembly assembly = typeof(ConfigurationPathGenerator).Assembly;
        string? version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;
        if (version is not null && !string.IsNullOrWhiteSpace(version))
        {
            return version;
        }

        version = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        return version is not null && !string.IsNullOrWhiteSpace(version)
            ? version
            : "1.0.0.0-unknown";
    }
}
// AI GENERATED END
