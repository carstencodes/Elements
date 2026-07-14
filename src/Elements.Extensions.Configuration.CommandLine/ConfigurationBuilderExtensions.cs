// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;
using Microsoft.Extensions.Configuration;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddCommandLine(this IConfigurationBuilder builder, RootCommand command, Action<CommandLineConfigurationOptions> configureOptions)
    {
        CommandLineConfigurationOptions options = new();
        configureOptions(options);
        CommandLineConfigurationSource source = new CommandLineConfigurationSource(command);
        source.Add(options.Build(), options.FormatProvider);
        return builder.Add(source);
    }

    public static IConfigurationBuilder AddCommandLine(this IConfigurationBuilder builder, Action<CommandLineConfigurationSource>? configureSource)
    {
        return builder.Add(configureSource);
    }
}
