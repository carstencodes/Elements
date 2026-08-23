// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Globalization;

namespace HedgeCraft.Elements.CommandLine;

/// <summary>
/// Provides a base class for converting command-line symbol results from type <typeparamref name="T"/> to type <typeparamref name="U"/>.
/// </summary>
/// <typeparam name="TSymbol">The symbol type.</typeparam>
/// <typeparam name="T">The source value type.</typeparam>
/// <typeparam name="U">The destination value type.</typeparam>
/// <param name="symbol">The source symbol.</param>
/// <param name="converter">The conversion function from <typeparamref name="T"/> to <typeparamref name="U"/>.</param>
/// <param name="defaultValueFactory">The factory function to produce default values of type <typeparamref name="U"/>.</param>
internal abstract class SymbolResultConverter<TSymbol, T, U>(TSymbol symbol, Func<T, U> converter, Func<U> defaultValueFactory)
    where U : notnull, IParsable<U>
    where TSymbol : notnull, Symbol
{
    private readonly Func<T, U> converter = converter;
    private readonly Func<U> defaultValueFactory = defaultValueFactory;

    /// <summary>
    /// Gets the parent symbol.
    /// </summary>
    protected TSymbol Symbol { get; } = symbol;

    /// <summary>
    /// Gets or sets the format provider used for parsing string values.
    /// </summary>
    public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>
    /// Parses the value from the argument result, or returns the default value if the result is null.
    /// </summary>
    /// <param name="result">The argument result.</param>
    /// <returns>The parsed or default value.</returns>
    public U ParseFromArgumentOrDefault(ArgumentResult result)
    {
        return this.ParseFromArgument(result) ?? this.GetDefaultValue();
    }

    /// <summary>
    /// Parses the value from the argument result tokens, or resolves it from the parent symbol if not present.
    /// </summary>
    /// <param name="argumentResult">The argument result.</param>
    /// <returns>The parsed or converted value, or default value if parsing fails.</returns>
    public U? ParseFromArgument(ArgumentResult argumentResult)
    {
        if (argumentResult.Tokens.Count == 1)
        {
            try
            {
                return U.Parse(argumentResult.Tokens[0].Value, this.FormatProvider);
            }
            catch (Exception ex) when (ex is ArgumentNullException or FormatException or OverflowException)
            {
                argumentResult.AddError(ex.Message);
                return this.GetDefaultValue();
            }
        }

        return this.ParseFromParentSymbol(this.Symbol, argumentResult);
    }

    /// <summary>
    /// Parses and converts the value from the specified parent symbol result.
    /// </summary>
    /// <param name="passedSymbol">The parent symbol.</param>
    /// <param name="argumentResult">The argument result.</param>
    /// <returns>The converted value, or default if not available.</returns>
    protected abstract U? ParseFromParentSymbol(TSymbol passedSymbol, ArgumentResult argumentResult);

    /// <summary>
    /// Converts a value of type <typeparamref name="T"/> to type <typeparamref name="U"/>.
    /// </summary>
    /// <param name="parentValue">The source value.</param>
    /// <returns>The converted value.</returns>
    protected U ConvertValue(T parentValue)
    {
        return this.converter(parentValue);
    }

    /// <summary>
    /// Produces a default value of type <typeparamref name="U"/>.
    /// </summary>
    /// <returns>The default value.</returns>
    protected U GetDefaultValue()
    {
        return this.defaultValueFactory();
    }
}
