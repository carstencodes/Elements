// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Provides an internal builder implementation for configuring and creating an <see cref="Option{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
/// <param name="name">The name of the option.</param>
internal sealed class OptionBuilder<T>(string name) : IOptionBuilder<T> where T : notnull
{
    private readonly List<Action<Option<T>>> configureOptions = [];

    /// <inheritdoc />
    public IOptionBuilder<T> AsGlobal()
    {
        return this.AsRecursive();
    }

    /// <inheritdoc />
    public IOptionBuilder<T> AsHidden()
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Hidden = true;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> AsLocal()
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Recursive = false;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> AsNotHidden()
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Hidden = false;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> AsOptional()
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Required = false;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> AsRecursive()
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Recursive = true;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> AsRequired()
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Required = true;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public Option<T> Build()
    {
        Option<T> option = new(name);
        foreach (Action<Option<T>> configureOption in this.configureOptions)
        {
            configureOption(option);
        }

        return option;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> UsingValidFileNames()
    {
        static void ConfigureOption(Option<T> option)
        {
            option.AcceptLegalFileNamesOnly();
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> UsingValidFilePaths()
    {
        static void ConfigureOption(Option<T> option)
        {
            option.AcceptLegalFilePathsOnly();
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithAlias(string aliasName)
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Aliases.Add(aliasName);
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithAliases(string[] aliases)
    {
        static void ConfigureOption(Option<T> option)
        {
            foreach (string alias in aliases)
            {
                option.Aliases.Add(alias);
            }
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithAllowedValues(IEnumerable<string> allowedValues)
    {
        static void ConfigureOption(Option<T> option)
        {
            option.AcceptOnlyFromAmong(allowedValues.ToArray());
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithArity(ArgumentArity arity)
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Arity = arity;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithCommandLineAction(CommandLineAction action)
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Action = action;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithCustomParser(Func<ArgumentResult, T> parser)
    {
        static void ConfigureOption(Option<T> option)
        {
            option.CustomParser = parser;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithDefaultValue(T defaultValue)
    {
        return this.WithDefaultValueFactory(_ => defaultValue);
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithDefaultValueFactory(Func<ArgumentResult, T> factory)
    {
        static void ConfigureOption(Option<T> option)
        {
            option.DefaultValueFactory = factory;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithDescription(string description)
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Description = description;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithHelpText(string helpText)
    {
        static void ConfigureOption(Option<T> option)
        {
            option.HelpName = helpText;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    /// <inheritdoc />
    public IOptionBuilder<T> WithValidator(Action<OptionResult> validator)
    {
        static void ConfigureOption(Option<T> option)
        {
            option.Validators.Add(validator);
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }
}
