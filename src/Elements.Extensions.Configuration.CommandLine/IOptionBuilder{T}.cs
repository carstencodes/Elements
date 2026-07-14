using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

public interface IOptionBuilder<T> where T: notnull
{
    IOptionBuilder<T> UsingValidFileNames();

    IOptionBuilder<T> UsingValidFilePaths();

    IOptionBuilder<T> WithAlias(string alias);

    IOptionBuilder<T> WithAliases(string[] aliases);

    IOptionBuilder<T> WithAllowedValues(IEnumerable<string> allowedValues);

    IOptionBuilder<T> WithArity(ArgumentArity arity);

    IOptionBuilder<T> WithCommandLineAction(CommandLineAction action);

    IOptionBuilder<T> WithCustomParser(Func<ArgumentResult, T> parser);

    IOptionBuilder<T> WithDescription(string description);

    IOptionBuilder<T> WithDefaultValue(T defaultValue);

    IOptionBuilder<T> WithDefaultValueFactory(Func<ArgumentResult, T> factory);

    IOptionBuilder<T> WithHelpText(string helpText);

    IOptionBuilder<T> AsHidden();
    
    IOptionBuilder<T> AsNotHidden();

    IOptionBuilder<T> AsRequired();

    IOptionBuilder<T> AsOptional();

    IOptionBuilder<T> AsLocal();

    IOptionBuilder<T> AsRecursive();

    IOptionBuilder<T> AsGlobal();

    IOptionBuilder<T> WithValidator(Action<OptionResult> validator);

    Option<T> Build();
}