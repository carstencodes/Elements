// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;
using Microsoft.Extensions.Configuration;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Provides extension methods for adding command-line configuration to an <see cref="IConfigurationBuilder"/>.
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds a command-line configuration source to the configuration builder using specified options.
    /// </summary>
    /// <param name="builder">The configuration builder to add to.</param>
    /// <param name="command">The root command containing command-line options.</param>
    /// <param name="configureOptions">The delegate used to configure command-line options mapping.</param>
    /// <returns>The same <see cref="IConfigurationBuilder"/> instance.</returns>
    public static IConfigurationBuilder AddCommandLine(this IConfigurationBuilder builder, RootCommand command, Action<CommandLineConfigurationOptions> configureOptions)
    {
        CommandLineConfigurationOptions options = new();
        configureOptions(options);
        CommandLineConfigurationSource source = new CommandLineConfigurationSource(command);
        source.Add(options.Build(), options.FormatProvider);
        return builder.Add(source);
    }

    /// <summary>
    /// Adds a command-line configuration source to the configuration builder using a configuration source action.
    /// </summary>
    /// <param name="builder">The configuration builder to add to.</param>
    /// <param name="configureSource">The optional delegate used to configure the configuration source.</param>
    /// <returns>The same <see cref="IConfigurationBuilder"/> instance.</returns>
    public static IConfigurationBuilder AddCommandLine(this IConfigurationBuilder builder, Action<CommandLineConfigurationSource>? configureSource)
    {
        return builder.Add(configureSource);
    }
}
