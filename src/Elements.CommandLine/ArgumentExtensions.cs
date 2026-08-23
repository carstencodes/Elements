// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;

namespace HedgeCraft.Elements.CommandLine;

/// <summary>
/// Provides extension methods for <see cref="Argument{T}"/>.
/// </summary>
public static class ArgumentExtensions
{
    /// <summary>
    /// Creates a new derived argument with a specified name, conversion function, and default value.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="argument">The source argument to derive from.</param>
    /// <param name="name">The name of the new derived argument.</param>
    /// <param name="converter">The function to convert the argument value.</param>
    /// <param name="defaultValue">The default value for the argument.</param>
    /// <returns>A new derived argument.</returns>
    public static Argument<T> Derive<T>(this Argument<T> argument, string name, Func<T, T> converter, T defaultValue) where T : notnull, IParsable<T>
    {
        return argument.Derive(name, converter, () => defaultValue);
    }

    /// <summary>
    /// Creates a new derived argument with a specified name, conversion function, and default value factory.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="argument">The source argument to derive from.</param>
    /// <param name="name">The name of the new derived argument.</param>
    /// <param name="converter">The function to convert the argument value.</param>
    /// <param name="defaultValueFactory">The factory function to produce the default value.</param>
    /// <returns>A new derived argument.</returns>
    public static Argument<T> Derive<T>(this Argument<T> argument, string name, Func<T, T> converter, Func<T> defaultValueFactory) where T : notnull, IParsable<T>
    {
        return new DerivedArgument<T, T>(name, argument, converter, defaultValueFactory);
    }

    /// <summary>
    /// Adds the argument to the specified command.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="argument">The argument to add.</param>
    /// <param name="command">The command to which the argument should be added.</param>
    /// <returns>The argument that was added.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="command"/> is <see langword="null"/>.</exception>
    public static Argument<T> AddToCommand<T>(this Argument<T> argument, Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        command.Add(argument);
        return argument;
    }
}
