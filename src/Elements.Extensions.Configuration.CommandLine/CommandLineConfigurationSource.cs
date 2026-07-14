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

public sealed partial class CommandLineConfigurationSource(RootCommand command) : IConfigurationSource, IFormatProviderSource
{
    private readonly Dictionary<string, OptionHolderBase> commandLineBindings = new();

    public CommandLineConfigurationSource() : this(new())
    {
    }

    public RootCommand Command { get; set; } = command;

    internal IReadOnlyDictionary<string, OptionHolderBase> CommandLineBindings
    {
        get
        {
            return this.commandLineBindings.AsReadOnly();
        }
    }

    public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

    public void Add<T>(KeyValuePair<string, Option<T>> binding)
    {
        this.Add(binding.Key, binding.Value);
    }

    public void Add<T>(string key, Option<T> option)
    {
        option.Recursive = true;
        this.commandLineBindings.Add(key, new OptionHolder<T>(option, this));
    }

    internal void Add(IReadOnlyDictionary<string, OptionHolderBase> options, IFormatProvider formatProvider)
    {
        foreach (KeyValuePair<string, OptionHolderBase> binding in options)
        {
            this.commandLineBindings.Add(binding.Key, binding.Value);
        }

        this.FormatProvider = formatProvider;
    }

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
