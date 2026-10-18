// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Defines a provider that exposes an <see cref="IFormatProvider"/>.
/// </summary>
internal interface IFormatProviderSource
{
    /// <summary>
    /// Gets the format provider.
    /// </summary>
    IFormatProvider FormatProvider { get; }
}
