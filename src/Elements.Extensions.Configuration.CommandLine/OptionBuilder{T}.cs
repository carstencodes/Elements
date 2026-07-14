using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

internal sealed class OptionBuilder<T>(string name) : IOptionBuilder<T> where T : notnull
{
    private readonly List<Action<Option<T>>> configureOptions = new();

    public IOptionBuilder<T> AsGlobal()
    {
        void ConfigureOption(Option<T> option)
        {
            option.Recursive = true;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> AsHidden()
    {
        void ConfigureOption(Option<T> option)
        {
            option.Hidden = true;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> AsLocal()
    {
        void ConfigureOption(Option<T> option)
        {
            option.Recursive = false;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> AsNotHidden()
    {
        void ConfigureOption(Option<T> option)
        {
            option.Hidden = false;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> AsOptional()
    {
        void ConfigureOption(Option<T> option)
        {
            option.Required = false;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> AsRecursive()
    {
        void ConfigureOption(Option<T> option)
        {
            option.Recursive = true;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> AsRequired()
    {
        void ConfigureOption(Option<T> option)
        {
            option.Required = true;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public Option<T> Build()
    {
        Option<T> option = new(name);
        foreach (Action<Option<T>> configureOption in this.configureOptions)
        {
            configureOption(option);
        }

        return option;
    }

    public IOptionBuilder<T> UsingValidFileNames()
    {
        void ConfigureOption(Option<T> option)
        {
            option.AcceptLegalFileNamesOnly();
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> UsingValidFilePaths()
    {
        void ConfigureOption(Option<T> option)
        {
            option.AcceptLegalFilePathsOnly();
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithAlias(string alias)
    {
        void ConfigureOption(Option<T> option)
        {
            option.Aliases.Add(alias);
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithAliases(string[] aliases)
    {
        void ConfigureOption(Option<T> option)
        {
            foreach (string alias in aliases)
            {
                option.Aliases.Add(alias);
            }
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithAllowedValues(IEnumerable<string> allowedValues)
    {
        void ConfigureOption(Option<T> option)
        {
            option.AcceptOnlyFromAmong(allowedValues.ToArray());
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithArity(ArgumentArity arity)
    {
        void ConfigureOption(Option<T> option)
        {
            option.Arity = arity;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithCommandLineAction(CommandLineAction action)
    {
        void ConfigureOption(Option<T> option)
        {
            option.Action = action;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithCustomParser(Func<ArgumentResult, T> parser)
    {
        void ConfigureOption(Option<T> option)
        {
            option.CustomParser = parser;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithDefaultValue(T defaultValue)
    {
        return this.WithDefaultValueFactory(_ => defaultValue);
    }

    public IOptionBuilder<T> WithDefaultValueFactory(Func<ArgumentResult, T> factory)
    {
        void ConfigureOption(Option<T> option)
        {
            option.DefaultValueFactory = factory;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithDescription(string description)
    {
        void ConfigureOption(Option<T> option)
        {
            option.Description = description;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithHelpText(string helpText)
    {
        void ConfigureOption(Option<T> option)
        {
            option.HelpName = helpText;
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }

    public IOptionBuilder<T> WithValidator(Action<OptionResult> validator)
    {
        void ConfigureOption(Option<T> option)
        {
            option.Validators.Add(validator);
        }

        this.configureOptions.Add(ConfigureOption);
        return this;
    }
}
