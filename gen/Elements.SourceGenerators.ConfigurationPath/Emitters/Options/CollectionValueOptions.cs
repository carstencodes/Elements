// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters.Options;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Options describing how a collection value is enumerated and keyed.</summary>
internal readonly record struct CollectionValueOptions(
    ITypeSymbol ElementType,
    string CollectionExpression,
    string KeysExpression,
    int Indent,
    bool CanBeNull,
    bool IsDictionaryValue);
// AI GENERATED END
