// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

internal interface IFormatProviderSource
{
    IFormatProvider FormatProvider { get; }
}
