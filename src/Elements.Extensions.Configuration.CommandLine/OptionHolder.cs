using System;
using System.CommandLine;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

internal abstract class OptionHolderBase(Option option)
{
    public Option Option { get; } = option;

    internal abstract string? GetResult(ParseResult parseResult);
}