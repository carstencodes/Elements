// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

internal abstract class OptionHolderBase(Option option)
{
    public Option Option { get; } = option;

    internal abstract string? GetResult(ParseResult parseResult);
}
