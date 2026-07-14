using System;
using System.CommandLine;

namespace HedgeCraft.Elements.CommandLine;

public static class ArgumentExtensions
{
    public static Argument<T> Derive<T>(this Argument<T> argument, string name, Func<T, T> converter, T defaultValue) where T : notnull, IParsable<T>
    {
        return argument.Derive(name, converter, () => defaultValue);
    }

    public static Argument<T> Derive<T>(this Argument<T> argument, string name, Func<T, T> converter, Func<T> defaultValueFactory) where T : notnull, IParsable<T>
    {
        return new DerivedArgument<T, T>(name, argument, converter, defaultValueFactory);
    }

    public static Argument<T> AddToCommand<T>(this Argument<T> argument, Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        command.Add(argument);
        return argument;
    }
}
