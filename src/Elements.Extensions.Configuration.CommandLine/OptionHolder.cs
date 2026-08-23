// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.CommandLine;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Provides a base class for holding command-line option instances and extracting string values from parse results.
/// </summary>
/// <param name="option">The option being held.</param>
internal abstract class OptionHolderBase(Option option)
{
    /// <summary>
    /// Gets the command-line option.
    /// </summary>
    public Option Option { get; } = option;

    /// <summary>
    /// Extracts the formatted string result for the option from the specified parse result.
    /// </summary>
    /// <param name="parseResult">The command-line parse result.</param>
    /// <returns>The string representation of the option value, or <see langword="null"/> if not present.</returns>
    internal abstract string? GetResult(ParseResult parseResult);
}
