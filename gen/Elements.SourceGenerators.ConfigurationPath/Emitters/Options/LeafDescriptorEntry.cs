// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters.Options;

// AI GENERATED START - model: Claude Sonnet 5.5
/// <summary>Represents one bindable configuration leaf collected at generation time.</summary>
/// <param name="Path">The configuration key segments that locate the leaf.</param>
/// <param name="TypeName">The fully qualified type name emitted inside a <c>typeof</c> expression.</param>
/// <param name="Description">The single-line description; never empty.</param>
internal sealed record LeafDescriptorEntry(
    string[] Path,
    string TypeName,
    string Description);
// AI GENERATED END
