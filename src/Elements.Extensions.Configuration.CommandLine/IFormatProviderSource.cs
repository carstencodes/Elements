using System;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

internal interface IFormatProviderSource
{
    IFormatProvider FormatProvider { get; }
}