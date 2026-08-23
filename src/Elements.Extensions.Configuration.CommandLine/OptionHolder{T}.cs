// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Holds a typed <see cref="Option{T}"/> and formats its parsed value into a configuration string.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
/// <param name="option">The typed command-line option.</param>
/// <param name="formatProviderSource">The format provider source for value formatting.</param>
internal sealed class OptionHolder<T>(Option<T> option, IFormatProviderSource formatProviderSource) : OptionHolderBase(option)
{
    /// <inheritdoc />
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
