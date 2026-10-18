// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: Copilot
/// <summary>Emits indexed and dictionary loops used to flatten collection values.</summary>
internal static class ConfigurationDumpCollectionEmitter
{
    /// <summary>Emits a collection traversal loop, choosing indexed keys or dictionary-entry keys.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="elementType">The collection's element or dictionary value type.</param>
    /// <param name="collectionExpression">The expression that yields the collection instance.</param>
    /// <param name="keysExpression">The parent path expression to extend for each item.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    /// <param name="canBeNull">Whether the collection needs a null guard before enumeration.</param>
    /// <param name="dictionaryValue">Whether item keys come from dictionary keys rather than numeric indexes.</param>
    internal static void AppendCollectionValue(
        StringBuilder source,
        ITypeSymbol elementType,
        string collectionExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex,
        bool canBeNull,
        bool dictionaryValue)
    {
        AppendCollectionLoop(
            source,
            elementType,
            collectionExpression,
            keysExpression,
            indent,
            valuesName,
            visitedName,
            ref localIndex,
            canBeNull,
            dictionaryValue);
    }

    /// <summary>Emits nullable-scalar assignment or a guarded traversal for a nullable complex value.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="nullableType">The nullable value type being visited.</param>
    /// <param name="valueExpression">The expression that evaluates to the nullable value.</param>
    /// <param name="keysExpression">The configuration path used for a scalar value.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    internal static void AppendNullableValue(
        StringBuilder source,
        INamedTypeSymbol nullableType,
        string valueExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex)
    {
        ITypeSymbol underlyingType = nullableType.TypeArguments[0];
        if (underlyingType.TypeKind == TypeKind.TypeParameter ||
            underlyingType is INamedTypeSymbol namedType &&
            ConfigurationPathGenerator.IsSimpleType(namedType))
        {
            // Nullable scalars still occupy their property key when empty; nullable models have no child keys then.
            ConfigurationDumpValueEmitter.AppendScalar(source, valueExpression, keysExpression, indent, valuesName);
            return;
        }

        int nullableIndex = localIndex++;
        string nullableValue = string.Concat("__nullableValue", nullableIndex);
        AppendLine(source, indent, string.Concat("if (", valueExpression, " is { } ", nullableValue, ")"));
        AppendLine(source, indent, "{");
        ConfigurationDumpValueEmitter.AppendDumpValue(
            source,
            nullableType.TypeArguments[0],
            nullableValue,
            keysExpression,
            indent + 1,
            valuesName,
            visitedName,
            ref localIndex);
        AppendLine(source, indent, "}");
    }

    /// <summary>Emits one traversal loop for every distinct supported enumerable element contract.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="values">The element types exposed by the collection.</param>
    /// <param name="collectionExpression">The expression that yields the collection instance.</param>
    /// <param name="keysExpression">The parent path expression to extend for each item.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    /// <param name="canBeNull">Whether the collection needs a null guard before enumeration.</param>
    internal static void AppendGenericCollectionValues(
        StringBuilder source,
        ImmutableArray<ITypeSymbol> values,
        string collectionExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex,
        bool canBeNull)
    {
        foreach (ITypeSymbol valueType in values)
        {
            AppendCollectionLoop(
                source,
                valueType,
                collectionExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                ref localIndex,
                canBeNull,
                dictionaryValue: false);
        }
    }

    /// <summary>Wraps a collection loop in a null guard when the collection is a reference type.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="elementType">The collection's element or dictionary value type.</param>
    /// <param name="collectionExpression">The expression that yields the collection instance.</param>
    /// <param name="keysExpression">The parent path expression to extend for each item.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    /// <param name="canBeNull">Whether to guard against a null collection reference.</param>
    /// <param name="dictionaryValue">Whether item keys come from dictionary keys rather than numeric indexes.</param>
    private static void AppendCollectionLoop(
        StringBuilder source,
        ITypeSymbol elementType,
        string collectionExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex,
        bool canBeNull,
        bool dictionaryValue)
    {
        int loopIndex = localIndex++;
        if (canBeNull)
        {
            // Enumerating a null collection would throw; the guard treats it as an empty branch of the model.
            AppendLine(source, indent, string.Concat("if (", collectionExpression, " is not null)"));
            AppendLine(source, indent, "{");
            indent++;
        }

        if (dictionaryValue)
        {
            AppendDictionaryLoop(
                source,
                elementType,
                collectionExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                ref localIndex);
        }
        else
        {
            AppendIndexedLoop(
                source,
                elementType,
                collectionExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                loopIndex,
                ref localIndex);
        }

        if (canBeNull)
        {
            indent--;
            AppendLine(source, indent, "}");
        }
    }

    /// <summary>
    /// Emits dictionary enumeration and appends each invariant stringified dictionary key to the path.
    /// </summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="valueType">The dictionary's value type.</param>
    /// <param name="dictionaryExpression">The expression that yields the dictionary instance.</param>
    /// <param name="keysExpression">The parent path expression to extend with each dictionary key.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    private static void AppendDictionaryLoop(
        StringBuilder source,
        ITypeSymbol valueType,
        string dictionaryExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex)
    {
        string entryName = string.Concat("__entry", localIndex++);
        string itemKeysName = string.Concat("__itemKeys", localIndex++);
        AppendLine(source, indent, string.Concat("foreach (var ", entryName, " in ", dictionaryExpression, ")"));
        AppendLine(source, indent, "{");
        AppendLine(
            source,
            indent + 1,
            string.Concat(
                "string[] ",
                itemKeysName,
                " = AppendKeys(",
                keysExpression,
                ", global::System.Convert.ToString(",
                entryName,
                ".Key, global::System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty);"));
        ConfigurationDumpValueEmitter.AppendDumpValue(
            source,
            valueType,
            string.Concat(entryName, ".Value"),
            itemKeysName,
            indent + 1,
            valuesName,
            visitedName,
            ref localIndex);
        AppendLine(source, indent, "}");
    }

    /// <summary>Emits sequence enumeration and uses each zero-based index as the next path segment.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="elementType">The sequence element type.</param>
    /// <param name="collectionExpression">The expression that yields the sequence instance.</param>
    /// <param name="keysExpression">The parent path expression to extend with each index.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="loopIndex">The unique suffix assigned to this loop's local variables.</param>
    /// <param name="localIndex">The counter used to keep nested generated local names unique.</param>
    private static void AppendIndexedLoop(
        StringBuilder source,
        ITypeSymbol elementType,
        string collectionExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        int loopIndex,
        ref int localIndex)
    {
        string itemName = string.Concat("__item", loopIndex);
        string indexName = string.Concat("__index", loopIndex);
        string itemKeysName = string.Concat("__itemKeys", loopIndex);
        AppendLine(source, indent, string.Concat("int ", indexName, " = 0;"));
        AppendLine(source, indent, string.Concat("foreach (var ", itemName, " in ", collectionExpression, ")"));
        AppendLine(source, indent, "{");
        AppendLine(
            source,
            indent + 1,
            string.Concat(
                "string[] ",
                itemKeysName,
                " = AppendKeys(",
                keysExpression,
                ", ",
                indexName,
                ".ToString(global::System.Globalization.CultureInfo.InvariantCulture));"));
        ConfigurationDumpValueEmitter.AppendDumpValue(
            source,
            elementType,
            itemName,
            itemKeysName,
            indent + 1,
            valuesName,
            visitedName,
            ref localIndex);
        AppendLine(source, indent + 1, string.Concat(indexName, "++;"));
        AppendLine(source, indent, "}");
    }

    /// <summary>Appends one source line using the emitter's four-space indentation convention.</summary>
    /// <param name="source">The source buffer receiving the line.</param>
    /// <param name="indent">The indentation depth.</param>
    /// <param name="value">The generated C# line without leading indentation.</param>
    private static void AppendLine(StringBuilder source, int indent, string value)
    {
        source.Append(' ', indent * 4).AppendLine(value);
    }
}
// AI GENERATED END
