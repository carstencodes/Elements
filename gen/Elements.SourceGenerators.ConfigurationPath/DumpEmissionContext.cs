// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Text;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: Copilot
/// <summary>Shared mutable state for one generated configuration dump.</summary>
internal sealed class DumpEmissionContext(StringBuilder source, string valuesName, string visitedName)
{
    /// <summary>
    /// Tracks generated local names for this one dump so recursive emitters allocate unique identifiers.
    /// </summary>
    // This counter is intentionally scoped to a single emission, not shared between generator runs.
    private int localIndex;

    /// <summary>Gets the buffer receiving generated source statements.</summary>
    internal StringBuilder Source { get; } = source;

    /// <summary>Gets the generated identifier for the flattened result dictionary.</summary>
    internal string ValuesName { get; } = valuesName;

    /// <summary>Gets the generated identifier for reference-cycle tracking.</summary>
    internal string VisitedName { get; } = visitedName;

    /// <summary>Allocates the next unique suffix for a generated local variable.</summary>
    /// <returns>The allocated local-name suffix.</returns>
    internal int NextLocalIndex()
    {
        return this.localIndex++;
    }
}
// AI GENERATED END
