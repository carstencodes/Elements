// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;

namespace HedgeCraft.Elements.CommandLine;

public class DerivedOption<T, U> : Option<U> where U : notnull, IParsable<U>
{
    private readonly OptionResultConverter<T, U> optionResultConverter;

    public DerivedOption(string name, Option<T> option, Func<T, U> converter, U defaultValue) : this(name, option, converter, () => defaultValue)
    {
    }

    public DerivedOption(string name, Option<T> option, Func<T, U> converter, Func<U> defaultValueFactory) : base(name)
    {
        this.optionResultConverter = new(option, converter, defaultValueFactory);

        base.Required = false;
        base.CustomParser = this.optionResultConverter.ParseFromArgument;
        base.DefaultValueFactory = this.optionResultConverter.ParseFromArgumentOrDefault;
        base.Arity = ArgumentArity.ZeroOrOne;
    }

    public IFormatProvider FormatProvider
    {
        get
        {
            return this.optionResultConverter.FormatProvider;
        }
        set
        {
            this.optionResultConverter.FormatProvider = value;
        }
    }
}
