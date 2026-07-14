using System;
using System.CommandLine;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

internal sealed class OptionHolder<T>(Option<T> option, IFormatProviderSource formatProviderSource) : OptionHolderBase(option)
{
    internal override string? GetResult(ParseResult parseResult)
    {
        T? value = parseResult.GetValue<T>((Option<T>)this.Option);
        if (value is null)
        {
            if (!this.Option.HasDefaultValue || this.Option.GetDefaultValue() is not T defaultValue)
            {
                return null;
            }

            value = defaultValue;
        }

        if (value is IConvertible convertible)
        {
            return Convert.ToString(convertible, formatProviderSource.FormatProvider);
        }

        return value.ToString();
    }
}
