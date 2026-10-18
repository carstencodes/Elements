// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: gpt-luna 5.6
/// <summary>Options describing the element contracts exposed by one enumerable value.</summary>
internal readonly record struct GenericCollectionValuesOptions(
    ImmutableArray<ITypeSymbol> ElementTypes,
    string CollectionExpression,
    string KeysExpression,
    int Indent,
    bool CanBeNull);
// AI GENERATED END
