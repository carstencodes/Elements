// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace HedgeCraft.Elements.CommandLine;

/// <summary>
/// Converts an option symbol result of type <typeparamref name="T"/> to a result of type <typeparamref name="U"/>.
/// </summary>
/// <typeparam name="T">The source option type.</typeparam>
/// <typeparam name="U">The destination option type.</typeparam>
/// <param name="option">The parent option symbol.</param>
/// <param name="converter">The conversion function from <typeparamref name="T"/> to <typeparamref name="U"/>.</param>
/// <param name="defaultValueFactory">The factory function to supply the default value of type <typeparamref name="U"/>.</param>
internal sealed class OptionResultConverter<T, U>(Option<T> option, Func<T, U> converter, Func<U> defaultValueFactory) :
    SymbolResultConverter<Option<T>, T, U>(option, converter, defaultValueFactory)
    where U : notnull, IParsable<U>
{
    /// <summary>
    /// Parses and converts the value from the parent option symbol result.
    /// </summary>
    /// <param name="passedSymbol">The parent option symbol.</param>
    /// <param name="argumentResult">The argument result from command-line parsing.</param>
    /// <returns>The converted value of type <typeparamref name="U"/>, or the default value if unavailable.</returns>
    protected override U? ParseFromParentSymbol(Option<T> passedSymbol, ArgumentResult argumentResult)
    {
        T? parentValue = argumentResult.GetValue<T>(passedSymbol);
        if (parentValue is null)
        {
            if (!passedSymbol.HasDefaultValue || passedSymbol.GetDefaultValue() is not T parentDefaultValue)
            {
                return this.GetDefaultValue();
            }

            parentValue = parentDefaultValue;
        }

        return this.ConvertValue(parentValue);
    }
}
