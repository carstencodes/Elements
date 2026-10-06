// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Text;
using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: Copilot
/// <summary>Emits indexed and dictionary loops used to flatten collection values.</summary>
internal static class ConfigurationDumpCollectionEmitter
{
    /// <summary>Emits a collection traversal loop, choosing indexed keys or dictionary-entry keys.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The collection shape and generated expressions.</param>
    internal static void AppendCollectionValue(
        DumpEmissionContext context,
        CollectionValueOptions options)
    {
        AppendCollectionLoop(context, options);
    }

    /// <summary>Emits nullable-scalar assignment or a guarded traversal for a nullable complex value.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The nullable value and its generated expressions.</param>
    internal static void AppendNullableValue(DumpEmissionContext context, DumpValueOptions options)
    {
        INamedTypeSymbol nullableType = (INamedTypeSymbol)options.Type;
        ITypeSymbol underlyingType = nullableType.TypeArguments[0];
        if (IsNullableScalar(underlyingType))
        {
            // Nullable scalars still occupy their property key when empty; nullable models have no child keys then.
            ConfigurationDumpValueEmitter.AppendScalar(context, options);
            return;
        }

        string nullableValue = string.Concat("__nullableValue", context.NextLocalIndex());
        AppendLine(
            context.Source,
            options.Indent,
            string.Concat("if (", options.ValueExpression, " is { } ", nullableValue, ")"));
        AppendLine(context.Source, options.Indent, "{");
        ConfigurationDumpValueEmitter.AppendDumpValue(
            context,
            options with
            {
                Type = underlyingType,
                ValueExpression = nullableValue,
                Indent = options.Indent + 1,
            });
        AppendLine(context.Source, options.Indent, "}");
    }

    /// <summary>Emits one traversal loop for every distinct supported enumerable element contract.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The collection's element contracts and generated expressions.</param>
    internal static void AppendGenericCollectionValues(
        DumpEmissionContext context,
        GenericCollectionValuesOptions options)
    {
        foreach (ITypeSymbol elementType in options.ElementTypes)
        {
            AppendCollectionLoop(
                context,
                new CollectionValueOptions(
                    elementType,
                    options.CollectionExpression,
                    options.KeysExpression,
                    options.Indent,
                    options.CanBeNull,
                    IsDictionaryValue: false));
        }
    }

    /// <summary>Wraps a collection loop in a null guard when the collection is a reference type.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The element type, collection expressions, and enumeration mode.</param>
    private static void AppendCollectionLoop(
        DumpEmissionContext context,
        CollectionValueOptions options)
    {
        int loopIndex = context.NextLocalIndex();
        int bodyIndent = options.Indent;
        if (options.CanBeNull)
        {
            AppendLine(
                context.Source,
                bodyIndent,
                string.Concat("if (", options.CollectionExpression, " is not null)"));
            AppendLine(context.Source, bodyIndent, "{");
            bodyIndent++;
        }

        CollectionValueOptions loopOptions = options with { Indent = bodyIndent };
        if (options.IsDictionaryValue)
        {
            AppendDictionaryLoop(context, loopOptions);
        }
        else
        {
            AppendIndexedLoop(context, loopOptions, loopIndex);
        }

        if (options.CanBeNull)
        {
            AppendLine(context.Source, options.Indent, "}");
        }
    }

    /// <summary>
    /// Emits dictionary enumeration and appends each invariant stringified dictionary key to the path.
    /// </summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The dictionary value type and generated expressions.</param>
    private static void AppendDictionaryLoop(
        DumpEmissionContext context,
        CollectionValueOptions options)
    {
        string entryName = string.Concat("__entry", context.NextLocalIndex());
        string itemKeysName = string.Concat("__itemKeys", context.NextLocalIndex());
        AppendLine(
            context.Source,
            options.Indent,
            string.Concat("foreach (var ", entryName, " in ", options.CollectionExpression, ")"));
        AppendLine(context.Source, options.Indent, "{");
        AppendLine(
            context.Source,
            options.Indent + 1,
            string.Concat(
                "string[] ",
                itemKeysName,
                " = AppendKeys(",
                options.KeysExpression,
                ", global::System.Convert.ToString(",
                entryName,
                ".Key, global::System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty);"));
        ConfigurationDumpValueEmitter.AppendDumpValue(
            context,
            new DumpValueOptions(
                options.ElementType,
                string.Concat(entryName, ".Value"),
                itemKeysName,
                options.Indent + 1));
        AppendLine(context.Source, options.Indent, "}");
    }

    /// <summary>Emits sequence enumeration and uses each zero-based index as the next path segment.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The sequence element type and generated expressions.</param>
    /// <param name="loopIndex">The unique suffix assigned to this loop's local variables.</param>
    private static void AppendIndexedLoop(
        DumpEmissionContext context,
        CollectionValueOptions options,
        int loopIndex)
    {
        string itemName = string.Concat("__item", loopIndex);
        string indexName = string.Concat("__index", loopIndex);
        string itemKeysName = string.Concat("__itemKeys", loopIndex);
        AppendLine(context.Source, options.Indent, string.Concat("int ", indexName, " = 0;"));
        AppendLine(
            context.Source,
            options.Indent,
            string.Concat("foreach (var ", itemName, " in ", options.CollectionExpression, ")"));
        AppendLine(context.Source, options.Indent, "{");
        AppendLine(
            context.Source,
            options.Indent + 1,
            string.Concat(
                "string[] ",
                itemKeysName,
                " = AppendKeys(",
                options.KeysExpression,
                ", ",
                indexName,
                ".ToString(global::System.Globalization.CultureInfo.InvariantCulture));"));
        ConfigurationDumpValueEmitter.AppendDumpValue(
            context,
            new DumpValueOptions(
                options.ElementType,
                itemName,
                itemKeysName,
                options.Indent + 1));
        AppendLine(
            context.Source,
            options.Indent + 1,
            string.Concat(indexName, "++;"));
        AppendLine(context.Source, options.Indent, "}");
    }

    private static bool IsNullableScalar(ITypeSymbol underlyingType)
    {
        return underlyingType.TypeKind == TypeKind.TypeParameter ||
            underlyingType is INamedTypeSymbol namedType &&
            ConfigurationPathGenerator.IsSimpleType(namedType);
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
