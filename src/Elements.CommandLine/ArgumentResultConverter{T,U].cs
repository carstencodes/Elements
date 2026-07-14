using System;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace HedgeCraft.Elements.CommandLine;

internal sealed class ArgumentResultConverter<T, U>(Argument<T> argument, Func<T, U> converter, Func<U> defaultValueFactory) :
    SymbolResultConverter<Argument<T>, T, U>(argument, converter, defaultValueFactory)
    where U : notnull, IParsable<U>
{
    protected override U? ParseFromParentSymbol(Argument<T> passedSymbol, ArgumentResult argumentResult)
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
