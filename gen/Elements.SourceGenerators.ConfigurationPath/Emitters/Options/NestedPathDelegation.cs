// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters.Options;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Represents a delegation to a nested configuration context path method.</summary>
/// <param name="KeyPath">The parent property key path to prefix to nested paths.</param>
/// <param name="ContextFieldName">The field name of the cached nested context instance.</param>
/// <param name="IsUserContext">Whether the target context is an external user-provided context.</param>
internal sealed record NestedPathDelegation(
    string[] KeyPath,
    string ContextFieldName,
    bool IsUserContext);
// AI GENERATED END
