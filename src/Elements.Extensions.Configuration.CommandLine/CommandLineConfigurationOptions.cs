using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

public sealed class CommandLineConfigurationOptions : IFormatProviderSource
{
    private readonly Dictionary<string, OptionHolderBase> options = new();

    public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

    public CommandLineConfigurationOptions Add<T>(IEnumerable<string> keys, Action<IOptionBuilder<T>> configureOption, string optionName) where T : notnull
    {
        string key = ConfigurationPath.Combine(keys);
        return this.Add(key, configureOption, optionName);
    }

    public CommandLineConfigurationOptions Add<T>(IEnumerable<string> keys, Action<Option<T>> configureOption, string optionName) where T : notnull
    {
        string key = ConfigurationPath.Combine(keys);
        return this.Add(key, configureOption, optionName);
    }

    public CommandLineConfigurationOptions Add<T>(string key, Action<IOptionBuilder<T>> configureOption, string optionName) where T : notnull
    {
        OptionBuilder<T> builder = new(optionName);
        configureOption(builder);
        return this.Add(key, builder.Build());
    }

    public CommandLineConfigurationOptions Add<T>(string key, Action<Option<T>> configureOption, string optionName) where T : notnull
    {
        Option<T> option = new Option<T>(optionName);
        configureOption(option);
        return this.Add(key, option);
    }

    public CommandLineConfigurationOptions Add<T>(IEnumerable<string> keys, Option<T> option) where T : notnull
    {
        string key = ConfigurationPath.Combine(keys);
        return this.Add(key, option);
    }

    public CommandLineConfigurationOptions Add<T>(string key, Option<T> option) where T : notnull
    {
        option.Recursive = true;
        this.options.Add(key, new OptionHolder<T>(option, this));
        return this;
    }

    internal IReadOnlyDictionary<string, OptionHolderBase> Build()
    {
        return this.options.AsReadOnly();
    }
}
