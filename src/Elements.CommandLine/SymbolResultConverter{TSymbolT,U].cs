// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Globalization;

namespace HedgeCraft.Elements.CommandLine;

internal abstract class SymbolResultConverter<TSymbol, T, U>(TSymbol symbol, Func<T, U> converter, Func<U> defaultValueFactory)
    where U : notnull, IParsable<U>
    where TSymbol : notnull, Symbol
{
    private readonly Func<T, U> converter = converter;
    private readonly Func<U> defaultValueFactory = defaultValueFactory;

    protected TSymbol Symbol { get; } = symbol;

    public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

    public U ParseFromArgumentOrDefault(ArgumentResult result)
    {
        return this.ParseFromArgument(result) ?? this.GetDefaultValue();
    }

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

    protected abstract U? ParseFromParentSymbol(TSymbol passedSymbol, ArgumentResult argumentResult);

    protected U ConvertValue(T parentValue)
    {
        return this.converter(parentValue);
    }

    protected U GetDefaultValue()
    {
        return this.defaultValueFactory();
    }
}
