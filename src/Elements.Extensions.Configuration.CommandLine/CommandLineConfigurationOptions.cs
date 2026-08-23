// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Provides configuration options for mapping command-line options to configuration keys.
/// </summary>
public sealed class CommandLineConfigurationOptions : IFormatProviderSource
{
    private readonly Dictionary<string, OptionHolderBase> options = new();

    /// <summary>
    /// Gets or sets the format provider used when converting command-line option values.
    /// </summary>
    public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>
    /// Adds a command-line option mapping for the specified composite configuration keys using an option builder.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="keys">The sequence of configuration key parts.</param>
    /// <param name="configureOption">The action to configure the option builder.</param>
    /// <param name="optionName">The primary name of the option.</param>
    /// <returns>The current configuration options instance.</returns>
    public CommandLineConfigurationOptions Add<T>(IEnumerable<string> keys, Action<IOptionBuilder<T>> configureOption, string optionName) where T : notnull
    {
        string key = ConfigurationPath.Combine(keys);
        return this.Add(key, configureOption, optionName);
    }

    /// <summary>
    /// Adds a command-line option mapping for the specified composite configuration keys using a direct option configurator.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="keys">The sequence of configuration key parts.</param>
    /// <param name="configureOption">The action to configure the option.</param>
    /// <param name="optionName">The primary name of the option.</param>
    /// <returns>The current configuration options instance.</returns>
    public CommandLineConfigurationOptions Add<T>(IEnumerable<string> keys, Action<Option<T>> configureOption, string optionName) where T : notnull
    {
        string key = ConfigurationPath.Combine(keys);
        return this.Add(key, configureOption, optionName);
    }

    /// <summary>
    /// Adds a command-line option mapping for the specified configuration key using an option builder.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="key">The configuration key path.</param>
    /// <param name="configureOption">The action to configure the option builder.</param>
    /// <param name="optionName">The primary name of the option.</param>
    /// <returns>The current configuration options instance.</returns>
    public CommandLineConfigurationOptions Add<T>(string key, Action<IOptionBuilder<T>> configureOption, string optionName) where T : notnull
    {
        OptionBuilder<T> builder = new(optionName);
        configureOption(builder);
        return this.Add(key, builder.Build());
    }

    /// <summary>
    /// Adds a command-line option mapping for the specified configuration key using a direct option configurator.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="key">The configuration key path.</param>
    /// <param name="configureOption">The action to configure the option.</param>
    /// <param name="optionName">The primary name of the option.</param>
    /// <returns>The current configuration options instance.</returns>
    public CommandLineConfigurationOptions Add<T>(string key, Action<Option<T>> configureOption, string optionName) where T : notnull
    {
        Option<T> option = new Option<T>(optionName);
        configureOption(option);
        return this.Add(key, option);
    }

    /// <summary>
    /// Adds a command-line option mapping for the specified composite configuration keys with an existing option.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="keys">The sequence of configuration key parts.</param>
    /// <param name="option">The option instance.</param>
    /// <returns>The current configuration options instance.</returns>
    public CommandLineConfigurationOptions Add<T>(IEnumerable<string> keys, Option<T> option) where T : notnull
    {
        string key = ConfigurationPath.Combine(keys);
        return this.Add(key, option);
    }

    /// <summary>
    /// Adds a command-line option mapping for the specified configuration key with an existing option.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="key">The configuration key path.</param>
    /// <param name="option">The option instance.</param>
    /// <returns>The current configuration options instance.</returns>
    public CommandLineConfigurationOptions Add<T>(string key, Option<T> option) where T : notnull
    {
        option.Recursive = true;
        this.options.Add(key, new OptionHolder<T>(option, this));
        return this;
    }

    /// <summary>
    /// Builds and returns the configured dictionary of option holders.
    /// </summary>
    /// <returns>A read-only dictionary mapping configuration keys to option holders.</returns>
    internal IReadOnlyDictionary<string, OptionHolderBase> Build()
    {
        return this.options.AsReadOnly();
    }
}
