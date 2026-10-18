// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Defines a fluent builder for configuring and creating an <see cref="Option{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
public interface IOptionBuilder<T> where T : notnull
{
    /// <summary>
    /// Configures the option to only accept valid file names.
    /// </summary>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> UsingValidFileNames();

    /// <summary>
    /// Configures the option to only accept valid file paths.
    /// </summary>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> UsingValidFilePaths();

    /// <summary>
    /// Adds an alias to the option.
    /// </summary>
    /// <param name="alias">The alias name.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithAlias(string alias);

    /// <summary>
    /// Adds multiple aliases to the option.
    /// </summary>
    /// <param name="aliases">The array of alias names.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithAliases(string[] aliases);

    /// <summary>
    /// Sets the allowed values for the option.
    /// </summary>
    /// <param name="allowedValues">The sequence of permissible string values.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithAllowedValues(IEnumerable<string> allowedValues);

    /// <summary>
    /// Sets the arity of the option.
    /// </summary>
    /// <param name="arity">The argument arity.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithArity(ArgumentArity arity);

    /// <summary>
    /// Sets the command-line action executed when this option is matched.
    /// </summary>
    /// <param name="action">The command-line action.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithCommandLineAction(CommandLineAction action);

    /// <summary>
    /// Configures a custom parser function for parsing the option value.
    /// </summary>
    /// <param name="parser">The custom parser delegate.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithCustomParser(Func<ArgumentResult, T> parser);

    /// <summary>
    /// Sets the description text for the option.
    /// </summary>
    /// <param name="description">The option description.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithDescription(string description);

    /// <summary>
    /// Sets a constant default value for the option.
    /// </summary>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithDefaultValue(T defaultValue);

    /// <summary>
    /// Sets a factory function that produces the default value for the option.
    /// </summary>
    /// <param name="factory">The factory delegate.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithDefaultValueFactory(Func<ArgumentResult, T> factory);

    /// <summary>
    /// Sets the help text for the option.
    /// </summary>
    /// <param name="helpText">The help text.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithHelpText(string helpText);

    /// <summary>
    /// Configures the option to be hidden from help output.
    /// </summary>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> AsHidden();

    /// <summary>
    /// Configures the option to be visible in help output.
    /// </summary>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> AsNotHidden();

    /// <summary>
    /// Marks the option as required.
    /// </summary>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> AsRequired();

    /// <summary>
    /// Marks the option as optional.
    /// </summary>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> AsOptional();

    /// <summary>
    /// Configures the option as local to its command.
    /// </summary>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> AsLocal();

    /// <summary>
    /// Configures the option to apply recursively to subcommands.
    /// </summary>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> AsRecursive();

    /// <summary>
    /// Configures the option to apply globally across the command tree.
    /// </summary>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> AsGlobal();

    /// <summary>
    /// Adds a validation delegate to the option.
    /// </summary>
    /// <param name="validator">The validation delegate.</param>
    /// <returns>The option builder instance.</returns>
    IOptionBuilder<T> WithValidator(Action<OptionResult> validator);

    /// <summary>
    /// Constructs and returns the configured <see cref="Option{T}"/>.
    /// </summary>
    /// <returns>The configured option instance.</returns>
    Option<T> Build();
}
