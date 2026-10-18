// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;

namespace HedgeCraft.Elements.CommandLine;

/// <summary>
/// Represents a command-line option whose value is derived from another option.
/// </summary>
/// <typeparam name="T">The source option type.</typeparam>
/// <typeparam name="U">The destination option type.</typeparam>
public class DerivedOption<T, U> : Option<U> where U : notnull, IParsable<U>
{
    private readonly OptionResultConverter<T, U> optionResultConverter;

    /// <summary>
    /// Initializes a new instance of the <see cref="DerivedOption{T, U}"/> class with a constant default value.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    /// <param name="option">The source option to derive values from.</param>
    /// <param name="converter">The conversion function from <typeparamref name="T"/> to <typeparamref name="U"/>.</param>
    /// <param name="defaultValue">The default value of type <typeparamref name="U"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="option"/> or <paramref name="converter"/> is <see langword="null"/>.</exception>
    public DerivedOption(string name, Option<T> option, Func<T, U> converter, U defaultValue) : this(name, option, converter, () => defaultValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DerivedOption{T, U}"/> class with a default value factory.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    /// <param name="option">The source option to derive values from.</param>
    /// <param name="converter">The conversion function from <typeparamref name="T"/> to <typeparamref name="U"/>.</param>
    /// <param name="defaultValueFactory">The factory function providing the default value of type <typeparamref name="U"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="option"/>, <paramref name="converter"/>, or <paramref name="defaultValueFactory"/> is <see langword="null"/>.</exception>
    public DerivedOption(string name, Option<T> option, Func<T, U> converter, Func<U> defaultValueFactory) : base(name)
    {
        this.optionResultConverter = new(option, converter, defaultValueFactory);

        base.Required = false;
        base.CustomParser = this.optionResultConverter.ParseFromArgument;
        base.DefaultValueFactory = this.optionResultConverter.ParseFromArgumentOrDefault;
        base.Arity = ArgumentArity.ZeroOrOne;
    }

    /// <summary>
    /// Gets or sets the format provider used during conversion and parsing.
    /// </summary>
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
