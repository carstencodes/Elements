using System;
using System.CommandLine;

namespace HedgeCraft.Elements.CommandLine;

public static class OptionExtensions
{
    public static Option<T> Derive<T>(this Option<T> option, string name, Func<T, T> converter, T defaultValue) where T : notnull, IParsable<T>
    {
        return option.Derive(name, converter, () => defaultValue);
    }

    public static Option<T> Derive<T>(this Option<T> option, string name, Func<T, T> converter, Func<T> defaultValueFactory) where T : notnull, IParsable<T>
    {
        return new DerivedOption<T, T>(name, option, converter, defaultValueFactory);
    }

    public static Option<T> AddToCommand<T>(this Option<T> option, Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        command.Add(option);
        return option;
    }
}
