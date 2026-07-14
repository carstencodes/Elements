using System;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace HedgeCraft.Elements.CommandLine;

internal sealed class OptionResultConverter<T, U>(Option<T> option, Func<T, U> converter, Func<U> defaultValueFactory) :
    SymbolResultConverter<Option<T>, T, U>(option, converter, defaultValueFactory)
    where U : notnull, IParsable<U>
{
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