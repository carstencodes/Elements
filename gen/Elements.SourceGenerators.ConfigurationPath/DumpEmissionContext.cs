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
    private int localIndex;

    internal StringBuilder Source { get; } = source;

    internal string ValuesName { get; } = valuesName;

    internal string VisitedName { get; } = visitedName;

    internal int NextLocalIndex()
    {
        return this.localIndex++;
    }
}
// AI GENERATED END
