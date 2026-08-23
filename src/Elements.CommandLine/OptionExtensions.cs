// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;

namespace HedgeCraft.Elements.CommandLine;

/// <summary>
/// Provides extension methods for <see cref="Option{T}"/>.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Creates a new derived option with a specified name, conversion function, and default value.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="option">The source option to derive from.</param>
    /// <param name="name">The name of the new derived option.</param>
    /// <param name="converter">The function to convert the option value.</param>
    /// <param name="defaultValue">The default value for the option.</param>
    /// <returns>A new derived option.</returns>
    public static Option<T> Derive<T>(this Option<T> option, string name, Func<T, T> converter, T defaultValue) where T : notnull, IParsable<T>
    {
        return option.Derive(name, converter, () => defaultValue);
    }

    /// <summary>
    /// Creates a new derived option with a specified name, conversion function, and default value factory.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="option">The source option to derive from.</param>
    /// <param name="name">The name of the new derived option.</param>
    /// <param name="converter">The function to convert the option value.</param>
    /// <param name="defaultValueFactory">The factory function to produce the default value.</param>
    /// <returns>A new derived option.</returns>
    public static Option<T> Derive<T>(this Option<T> option, string name, Func<T, T> converter, Func<T> defaultValueFactory) where T : notnull, IParsable<T>
    {
        return new DerivedOption<T, T>(name, option, converter, defaultValueFactory);
    }

    /// <summary>
    /// Adds the option to the specified command.
    /// </summary>
    /// <typeparam name="T">The type of the option value.</typeparam>
    /// <param name="option">The option to add.</param>
    /// <param name="command">The command to which the option should be added.</param>
    /// <returns>The option that was added.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="command"/> is <see langword="null"/>.</exception>
    public static Option<T> AddToCommand<T>(this Option<T> option, Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        command.Add(option);
        return option;
    }
}
