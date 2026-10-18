// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters.Options;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Options describing the element contracts exposed by one enumerable value.</summary>
/// <param name="ElementTypes">The element type contracts supported by the collection.</param>
/// <param name="CollectionExpression">The C# expression accessing the collection instance.</param>
/// <param name="KeysExpression">The C# expression representing the ancestor key path.</param>
/// <param name="Indent">The indentation level for emitted statements.</param>
/// <param name="CanBeNull">Whether the collection reference may be null.</param>
internal readonly record struct GenericCollectionValuesOptions(
    ImmutableArray<ITypeSymbol> ElementTypes,
    string CollectionExpression,
    string KeysExpression,
    int Indent,
    bool CanBeNull);
// AI GENERATED END
