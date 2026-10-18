// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Generic;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests;

// AI GENERATED START - model: Claude Sonnet 5.5
/// <summary>
/// Result of comparing the generated dump with the configuration that was bound by the binder.
/// </summary>
/// <param name="CompilationErrors">Compiler errors of the generated assembly, empty on success.</param>
/// <param name="Missing">Configuration keys the generated dump did not report.</param>
/// <param name="Extra">Dump keys with non-null values that were never part of the configuration.</param>
/// <param name="Mismatched">Keys reported by the dump with a different value than configured.</param>
/// <param name="UncoveredByPaths">Configured keys absent from the generated writable path set.</param>
internal sealed record ParityOutcome(
    string CompilationErrors,
    IReadOnlyList<string> Missing,
    IReadOnlyList<string> Extra,
    IReadOnlyList<string> Mismatched,
    IReadOnlyList<string> UncoveredByPaths)
{
    /// <summary>
    /// Creates an outcome for a source that failed to compile.
    /// </summary>
    /// <param name="compilationErrors">The compiler errors.</param>
    /// <returns>The failed outcome.</returns>
    public static ParityOutcome Failed(string compilationErrors)
    {
        return new ParityOutcome(compilationErrors, [], [], [], []);
    }
}
// AI GENERATED END
