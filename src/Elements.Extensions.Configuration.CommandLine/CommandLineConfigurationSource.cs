// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Primitives;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Represents a configuration source that binds command-line options to configuration keys.
/// </summary>
/// <param name="command">The root command containing options.</param>
public sealed partial class CommandLineConfigurationSource(RootCommand command) : IConfigurationSource, IFormatProviderSource
{
    private readonly Dictionary<string, OptionHolderBase> commandLineBindings = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineConfigurationSource"/> class.
    /// </summary>
    public CommandLineConfigurationSource() : this(new())
    {
    }

    /// <summary>
    /// Gets or sets the root command for command-line parsing.
    /// </summary>
    public RootCommand Command { get; set; } = command;

    /// <summary>
    /// Gets the registered command-line option bindings.
    /// </summary>
    internal IReadOnlyDictionary<string, OptionHolderBase> CommandLineBindings
    {
        get
        {
            return this.commandLineBindings.AsReadOnly();
        }
    }

    /// <summary>
    /// Gets or sets the format provider used for formatting option values.
    /// </summary>
    public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>
    /// Adds a configuration key to command-line option binding.
    /// </summary>
    /// <typeparam name="T">The option value type.</typeparam>
    /// <param name="binding">The key-value pair binding the configuration key to an option.</param>
    public void Add<T>(KeyValuePair<string, Option<T>> binding)
    {
        this.Add(binding.Key, binding.Value);
    }

    /// <summary>
    /// Adds a command-line option binding for the specified configuration key.
    /// </summary>
    /// <typeparam name="T">The option value type.</typeparam>
    /// <param name="key">The configuration key path.</param>
    /// <param name="option">The option to bind.</param>
    public void Add<T>(string key, Option<T> option)
    {
        option.Recursive = true;
        this.commandLineBindings.Add(key, new OptionHolder<T>(option, this));
    }

    /// <summary>
    /// Adds multiple option bindings and configures the format provider.
    /// </summary>
    /// <param name="options">The dictionary of option holders to register.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    internal void Add(IReadOnlyDictionary<string, OptionHolderBase> options, IFormatProvider formatProvider)
    {
        foreach (KeyValuePair<string, OptionHolderBase> binding in options)
        {
            this.commandLineBindings.Add(binding.Key, binding.Value);
        }

        this.FormatProvider = formatProvider;
    }

    /// <summary>
    /// Builds the configuration provider for this source.
    /// </summary>
    /// <param name="builder">The configuration builder.</param>
    /// <returns>A new <see cref="CommandLineConfigurationProvider"/> instance configured with the option bindings.</returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        CommandLineConfigurationProvider provider = new();
        foreach (KeyValuePair<string, OptionHolderBase> binding in this.commandLineBindings)
        {
            OptionHolderBase option = binding.Value;
            ReconfigureConfigurationValueAction action = new(provider, binding, option.Option.Action);
            option.Option.Action = action;
        }

        return provider;
    }
}
