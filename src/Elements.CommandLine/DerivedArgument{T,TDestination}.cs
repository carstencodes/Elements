// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;

namespace HedgeCraft.Elements.CommandLine;

/// <summary>
/// Represents a command-line argument whose value is derived from another argument.
/// </summary>
/// <typeparam name="T">The source argument type.</typeparam>
/// <typeparam name="TDestination">The destination argument type.</typeparam>
public class DerivedArgument<T, TDestination> : Argument<TDestination> where TDestination : notnull, IParsable<TDestination>
{
    private readonly ArgumentResultConverter<T, TDestination> argumentResultConverter;

    /// <summary>
    /// Initializes a new instance of the <see cref="DerivedArgument{T, TDestination}"/> class with a constant default value.
    /// </summary>
    /// <param name="name">The name of the argument.</param>
    /// <param name="argument">The source argument to derive values from.</param>
    /// <param name="converter">The conversion function from <typeparamref name="T"/> to <typeparamref name="TDestination"/>.</param>
    /// <param name="defaultValue">The default value of type <typeparamref name="TDestination"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="argument"/> or <paramref name="converter"/> is <see langword="null"/>.</exception>
    public DerivedArgument(string name, Argument<T> argument, Func<T, TDestination> converter, TDestination defaultValue) : this(name, argument, converter, () => defaultValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DerivedArgument{T, TDestination}"/> class with a default value factory.
    /// </summary>
    /// <param name="name">The name of the argument.</param>
    /// <param name="argument">The source argument to derive values from.</param>
    /// <param name="converter">The conversion function from <typeparamref name="T"/> to <typeparamref name="TDestination"/>.</param>
    /// <param name="defaultValueFactory">The factory function providing the default value of type <typeparamref name="TDestination"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="argument"/>, <paramref name="converter"/>, or <paramref name="defaultValueFactory"/> is <see langword="null"/>.</exception>
    public DerivedArgument(string name, Argument<T> argument, Func<T, TDestination> converter, Func<TDestination> defaultValueFactory) : base(name)
    {
        this.argumentResultConverter = new(argument, converter, defaultValueFactory);

        base.CustomParser = this.argumentResultConverter.ParseFromArgument;
        base.DefaultValueFactory = this.argumentResultConverter.ParseFromArgumentOrDefault;
        base.Arity = ArgumentArity.ZeroOrOne;
    }

    /// <summary>
    /// Gets or sets the format provider used during conversion and parsing.
    /// </summary>
    public IFormatProvider FormatProvider
    {
        get
        {
            return this.argumentResultConverter.FormatProvider;
        }
        set
        {
            this.argumentResultConverter.FormatProvider = value;
        }
    }
}
