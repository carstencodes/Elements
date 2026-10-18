// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters.Options;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Options describing one value emitted into a flattened configuration dump.</summary>
internal readonly record struct DumpValueOptions(
    ITypeSymbol Type,
    string ValueExpression,
    string KeysExpression,
    int Indent);
// AI GENERATED END
