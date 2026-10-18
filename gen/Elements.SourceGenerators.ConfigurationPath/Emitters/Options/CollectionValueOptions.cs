// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters.Options;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Options describing how a collection value is enumerated and keyed.</summary>
/// <param name="ElementType">The type of elements contained in the collection.</param>
/// <param name="CollectionExpression">The C# expression accessing the collection instance.</param>
/// <param name="KeysExpression">The C# expression representing the ancestor key path.</param>
/// <param name="Indent">The indentation level for emitted statements.</param>
/// <param name="CanBeNull">Whether the collection reference may be null.</param>
/// <param name="IsDictionaryValue">Whether the collection represents key-value dictionary pairs.</param>
internal readonly record struct CollectionValueOptions(
    ITypeSymbol ElementType,
    string CollectionExpression,
    string KeysExpression,
    int Indent,
    bool CanBeNull,
    bool IsDictionaryValue);
// AI GENERATED END
