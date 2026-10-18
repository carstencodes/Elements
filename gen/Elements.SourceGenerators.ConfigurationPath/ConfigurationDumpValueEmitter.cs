// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: Copilot
/// <summary>Emits the recursive configuration values included in a generated dump.</summary>
internal static class ConfigurationDumpValueEmitter
{
    /// <summary>Emits a dump body that applies the writable-only option before reading simple properties.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="type">The model whose bindable properties are visited.</param>
    /// <param name="indent">The generated block indentation level.</param>
    internal static void AppendBody(StringBuilder source, INamedTypeSymbol type, int indent)
    {
        AppendLine(
            source,
            indent,
            "global::System.Collections.Generic.Dictionary<string, object?> values = new(" +
                "global::System.StringComparer.Ordinal);");
        DumpValueParameters parameters = new(source, "values", "visited");
        int localIndex = 0;
        foreach (IPropertySymbol property in ConfigurationPathGenerator.GetBindableProperties(type))
        {
            // Only simple read-only values are omitted: retaining models and collections preserves their child paths.
            if (IsSimpleProperty(property.Type) &&
                property.SetMethod?.DeclaredAccessibility != Accessibility.Public)
            {
                AppendLine(source, indent, "if (!considerWritablePropertiesOnly)");
                AppendLine(source, indent, "{");
                AppendDumpProperty(parameters, property, indent + 1, ref localIndex);
                AppendLine(source, indent, "}");
                continue;
            }

            AppendDumpProperty(parameters, property, indent, ref localIndex);
        }

        AppendLine(source, indent, "return values;");
    }

    /// <summary>Reads a property once and routes its value through the type-specific traversal emitter.</summary>
    /// <param name="parameters">Shared output and traversal-state names for the current dump body.</param>
    /// <param name="property">The property to read and emit.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    private static void AppendDumpProperty(
        DumpValueParameters parameters,
        IPropertySymbol property,
        int indent,
        ref int localIndex)
    {
        StringBuilder source = parameters.Source;
        string localName = string.Concat("__propertyValue", localIndex++);
        AppendLine(
            source,
            indent,
            string.Concat("var ", localName, " = instance.")
                + ConfigurationPathGenerator.EscapeIdentifier(property.Name)
                + ";");
        string keysExpression = string.Concat(
            "AppendKeys(parentKeys, ",
            SymbolDisplay.FormatLiteral(ConfigurationPathGenerator.GetKeyName(property), true),
            ")");
        AppendDumpValue(
            parameters,
            property.Type,
            localName,
            keysExpression,
            indent,
            ref localIndex);
    }

    /// <summary>Creates traversal parameters and dispatches an arbitrary value to its matching emitter.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="type">The static type of the value expression.</param>
    /// <param name="valueExpression">The generated expression that evaluates to the value.</param>
    /// <param name="keysExpression">The configuration path expression for this value.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    internal static void AppendDumpValue(
        StringBuilder source,
        ITypeSymbol type,
        string valueExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex)
    {
        AppendDumpValue(
            new DumpValueParameters(source, valuesName, visitedName),
            type,
            valueExpression,
            keysExpression,
            indent,
            ref localIndex);
    }

    /// <summary>Selects scalar, array, named-type, and collection emission without repeating traversal state.</summary>
    /// <param name="parameters">Shared output and traversal-state names for the current dump body.</param>
    /// <param name="type">The static type of the value expression.</param>
    /// <param name="valueExpression">The generated expression that evaluates to the value.</param>
    /// <param name="keysExpression">The configuration path expression for this value.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    private static void AppendDumpValue(
        DumpValueParameters parameters,
        ITypeSymbol type,
        string valueExpression,
        string keysExpression,
        int indent,
        ref int localIndex)
    {
        StringBuilder source = parameters.Source;
        string valuesName = parameters.ValuesName;
        string visitedName = parameters.VisitedName;
        if (type is IArrayTypeSymbol arrayType)
        {
            if (ConfigurationPathGenerator.IsByteArray(arrayType))
            {
                AppendScalar(source, valueExpression, keysExpression, indent, valuesName);
            }
            else
            {
                ConfigurationDumpCollectionEmitter.AppendCollectionValue(
                    source,
                    arrayType.ElementType,
                    valueExpression,
                    keysExpression,
                    indent,
                    valuesName,
                    visitedName,
                    ref localIndex,
                    true,
                    false);
            }

            return;
        }

        if (type is INamedTypeSymbol namedType)
        {
            AppendNamedValue(
                source,
                namedType,
                valueExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                ref localIndex);
        }
        else
        {
            AppendScalar(source, valueExpression, keysExpression, indent, valuesName);
        }
    }

    /// <summary>Dispatches a named type to nullable, scalar, collection, or nested-model handling.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="type">The named value type.</param>
    /// <param name="valueExpression">The generated expression that evaluates to the value.</param>
    /// <param name="keysExpression">The configuration path expression for this value.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    private static void AppendNamedValue(
        StringBuilder source,
        INamedTypeSymbol type,
        string valueExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex)
    {
        if (type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
        {
            ConfigurationDumpCollectionEmitter.AppendNullableValue(
                source,
                type,
                valueExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                ref localIndex);
        }
        else if (ConfigurationPathGenerator.IsSimpleType(type))
        {
            AppendScalar(source, valueExpression, keysExpression, indent, valuesName);
        }
        else if (!AppendNamedCollection(
                      source,
                      type,
                      valueExpression,
                      keysExpression,
                      indent,
                      valuesName,
                      visitedName,
                      ref localIndex))
        {
            AppendModelValue(
                source,
                type,
                valueExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                ref localIndex);
        }
    }

    /// <summary>Emits dictionary or enumerable handling when the named type is a supported collection.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="type">The named type to inspect.</param>
    /// <param name="valueExpression">The generated expression that evaluates to the collection.</param>
    /// <param name="keysExpression">The parent configuration path expression.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    /// <returns><see langword="true"/> when collection handling was emitted.</returns>
    private static bool AppendNamedCollection(
        StringBuilder source,
        INamedTypeSymbol type,
        string valueExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex)
    {
        INamedTypeSymbol? dictionary = ConfigurationPathGenerator.GetDictionaryContract(type);
        if (dictionary is not null)
        {
            ConfigurationDumpCollectionEmitter.AppendCollectionValue(
                source,
                dictionary.TypeArguments[1],
                valueExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                ref localIndex,
                type.IsReferenceType,
                dictionaryValue: true);
            return true;
        }

        if (ConfigurationPathGenerator.TryGetCollectionValues(type, out ImmutableArray<ITypeSymbol> values))
        {
            ConfigurationDumpCollectionEmitter.AppendGenericCollectionValues(
                source,
                values,
                valueExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                ref localIndex,
                type.IsReferenceType);
            return true;
        }

        return false;
    }

    /// <summary>Chooses inline expansion for closed generic models or delegates to a generated context.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="type">The model type to expand.</param>
    /// <param name="valueExpression">The generated expression that evaluates to the model.</param>
    /// <param name="keysExpression">The parent configuration path expression.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    private static void AppendModelValue(
        StringBuilder source,
        INamedTypeSymbol type,
        string valueExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex)
    {
        if (IsClosedGenericObject(type) &&
            ConfigurationPathGenerator.FindExistingContext(type.OriginalDefinition) is null)
        {
            AppendInlineGenericModel(
                source,
                type,
                valueExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                ref localIndex);
            return;
        }

        AppendContextDump(
            source,
            type,
            valueExpression,
            keysExpression,
            indent,
            valuesName,
            visitedName);
    }

    /// <summary>
    /// Expands a closed generic model inline because its open definition cannot have a concrete context.
    /// </summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="type">The closed generic model type.</param>
    /// <param name="valueExpression">The generated expression that evaluates to the model.</param>
    /// <param name="keysExpression">The parent configuration path expression.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    private static void AppendInlineGenericModel(
        StringBuilder source,
        INamedTypeSymbol type,
        string valueExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex)
    {
        bool trackReference = type.IsReferenceType;
        if (trackReference)
        {
            string condition = string.Concat(
                "if (",
                valueExpression,
                " is not null && ",
                visitedName,
                ".Add(",
                valueExpression,
                "))");
            AppendLine(source, indent, condition);
            AppendLine(source, indent, "{");
            AppendLine(source, indent + 1, "try");
            AppendLine(source, indent + 1, "{");
            indent += 2;
        }

        AppendInlineGenericProperties(
            source,
            type,
            valueExpression,
            keysExpression,
            indent,
            valuesName,
            visitedName,
            ref localIndex);

        if (trackReference)
        {
            indent -= 2;
            AppendLine(source, indent + 1, "}");
            AppendLine(source, indent + 1, "finally");
            AppendLine(source, indent + 1, "{");
            AppendLine(source, indent + 2, string.Concat(visitedName, ".Remove(", valueExpression, ");"));
            AppendLine(source, indent + 1, "}");
            AppendLine(source, indent, "}");
        }
    }

    /// <summary>
    /// Emits inline generic properties with the same simple read-only filter as root models.
    /// </summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="type">The closed generic model whose properties are visited.</param>
    /// <param name="valueExpression">The expression that evaluates to the model instance.</param>
    /// <param name="keysExpression">The parent configuration path expression.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    private static void AppendInlineGenericProperties(
        StringBuilder source,
        INamedTypeSymbol type,
        string valueExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex)
    {
        foreach (IPropertySymbol property in ConfigurationPathGenerator.GetBindableProperties(type))
        {
            bool filterSimpleReadOnlyProperty = IsSimpleProperty(property.Type) &&
                property.SetMethod?.DeclaredAccessibility != Accessibility.Public;
            if (filterSimpleReadOnlyProperty)
            {
                AppendLine(source, indent, "if (!considerWritablePropertiesOnly)");
                AppendLine(source, indent, "{");
                indent++;
            }

            AppendInlineGenericProperty(
                source,
                property,
                valueExpression,
                keysExpression,
                indent,
                valuesName,
                visitedName,
                ref localIndex);

            if (filterSimpleReadOnlyProperty)
            {
                indent--;
                AppendLine(source, indent, "}");
            }
        }
    }

    /// <summary>
    /// Reads one inline generic property and delegates to its scalar, collection, or context emitter.
    /// </summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="property">The property to read and emit.</param>
    /// <param name="instanceExpression">The expression for the containing model instance.</param>
    /// <param name="parentKeysExpression">The containing model's configuration path expression.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    /// <param name="localIndex">The counter used to keep generated local names unique.</param>
    private static void AppendInlineGenericProperty(
        StringBuilder source,
        IPropertySymbol property,
        string instanceExpression,
        string parentKeysExpression,
        int indent,
        string valuesName,
        string visitedName,
        ref int localIndex)
    {
        string valueName = string.Concat("__genericProperty", localIndex++);
        AppendLine(
            source,
            indent,
            string.Concat(
                "var ",
                valueName,
                " = ",
                instanceExpression,
                ".",
                ConfigurationPathGenerator.EscapeIdentifier(property.Name),
                ";"));
        string keysExpression = string.Concat(
            "AppendKeys(",
            parentKeysExpression,
            ", ",
            SymbolDisplay.FormatLiteral(ConfigurationPathGenerator.GetKeyName(property), true),
            ")");

        if (property.Type is INamedTypeSymbol nestedType &&
            IsClosedGenericObject(nestedType) &&
            ConfigurationPathGenerator.FindExistingContext(nestedType.OriginalDefinition) is null)
        {
            AppendContextDump(
                source,
                nestedType,
                valueName,
                keysExpression,
                indent,
                valuesName,
                visitedName);
            return;
        }

        AppendDumpValue(
            source,
            property.Type,
            valueName,
            keysExpression,
            indent,
            valuesName,
            visitedName,
            ref localIndex);
    }

    /// <summary>
    /// Checks whether a generic model can be expanded with concrete type arguments at this call site.
    /// </summary>
    /// <param name="type">The candidate model type.</param>
    /// <returns><see langword="true"/> when it is a non-collection model with closed generic arguments.</returns>
    private static bool IsClosedGenericObject(INamedTypeSymbol type)
    {
        if (type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T ||
            ConfigurationPathGenerator.IsSimpleType(type) ||
            ConfigurationPathGenerator.GetDictionaryContract(type) is not null ||
            ConfigurationPathGenerator.TryGetCollectionValues(type, out _))
        {
            return false;
        }

        ImmutableArray<ITypeSymbol> typeArguments = ConfigurationPathGenerator.GetTypeArguments(type);
        return typeArguments.Length > 0 && typeArguments.All(static argument => !ContainsTypeParameter(argument));

        static bool ContainsTypeParameter(ITypeSymbol type)
        {
            // Open nested arguments are not closed just because their outer generic type is constructed.
            if (type.TypeKind == TypeKind.TypeParameter)
            {
                return true;
            }

            if (type is IArrayTypeSymbol arrayType)
            {
                return ContainsTypeParameter(arrayType.ElementType);
            }

            return type is INamedTypeSymbol namedType &&
                namedType.TypeArguments.Any(ContainsTypeParameter);
        }
    }

    /// <summary>
    /// Invokes the cached child context and merges its flattened values under the child path.
    /// </summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="modelType">The nested model handled by the cached context.</param>
    /// <param name="valueExpression">The expression that evaluates to the nested model.</param>
    /// <param name="keysExpression">The nested model's configuration path expression.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The parent result dictionary variable name.</param>
    /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
    private static void AppendContextDump(
        StringBuilder source,
        INamedTypeSymbol modelType,
        string valueExpression,
        string keysExpression,
        int indent,
        string valuesName,
        string visitedName)
    {
        string contextVariable = ConfigurationDumpMethodEmitter.CreateContextFieldName(modelType);
        bool existingContext = ConfigurationPathGenerator.FindExistingContext(modelType.OriginalDefinition) is not null;
        // User-provided contexts expose only the public API; generated contexts share the parent's cycle set.
        string methodName = existingContext ? "DumpConfigurationObject" : "DumpConfigurationObjectCore";
        string arguments = existingContext
            ? string.Concat(valueExpression, ", considerWritablePropertiesOnly, ", keysExpression)
            : string.Concat(
                valueExpression,
                ", considerWritablePropertiesOnly, ",
                keysExpression,
                ", ",
                visitedName);

        if (modelType.IsReferenceType)
        {
            AppendLine(source, indent, string.Concat("if (", valueExpression, " is not null)"));
            AppendLine(source, indent, "{");
            indent++;
        }

        AppendContextCall(
            source,
            contextVariable,
            methodName,
            arguments,
            valuesName,
            indent);

        if (modelType.IsReferenceType)
        {
            indent--;
            AppendLine(source, indent, "}");
        }
    }

    /// <summary>Emits the child-context loop that copies its entries into the current result dictionary.</summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="contextVariable">The static field name of the cached nested context.</param>
    /// <param name="methodName">The public or internal dump method to invoke.</param>
    /// <param name="arguments">The already formatted invocation arguments.</param>
    /// <param name="valuesName">The parent result dictionary variable name.</param>
    /// <param name="indent">The generated block indentation level.</param>
    private static void AppendContextCall(
        StringBuilder source,
        string contextVariable,
        string methodName,
        string arguments,
        string valuesName,
        int indent)
    {
        string entryVariable = string.Concat(contextVariable, "Entry");
        AppendLine(
            source,
            indent,
            string.Concat(
                "foreach (global::System.Collections.Generic.KeyValuePair<string, object?> ",
                entryVariable,
                " in ",
                contextVariable,
                ".",
                methodName,
                "(",
                arguments,
                "))"));
        AppendLine(source, indent, "{");
        AppendLine(
            source,
            indent + 1,
            string.Concat(valuesName, "[", entryVariable, ".Key] = ", entryVariable, ".Value;"));
        AppendLine(source, indent, "}");
    }

    /// <summary>
    /// Emits assignment at the colon-joined configuration key, including a null scalar value.
    /// </summary>
    /// <param name="source">The source buffer receiving generated statements.</param>
    /// <param name="valueExpression">The expression whose value is stored.</param>
    /// <param name="keysExpression">The array expression containing the full configuration path.</param>
    /// <param name="indent">The generated block indentation level.</param>
    /// <param name="valuesName">The result dictionary variable name.</param>
    internal static void AppendScalar(
        StringBuilder source,
        string valueExpression,
        string keysExpression,
        int indent,
        string valuesName)
    {
        AppendLine(
            source,
            indent,
            string.Concat(
                valuesName,
                "[global::System.String.Join(\":\", ",
                keysExpression,
                ")] = ",
                valueExpression,
                ";"));
    }

    /// <summary>Classifies nullable scalar and named scalar types for writable-only property filtering.</summary>
    /// <param name="type">The property type to classify.</param>
    /// <returns><see langword="true"/> when the type is a scalar rather than a nested configuration model.</returns>
    private static bool IsSimpleProperty(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol namedType &&
            namedType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
        {
            return ConfigurationPathGenerator.IsSimpleType((INamedTypeSymbol)namedType.TypeArguments[0]);
        }

        return type is INamedTypeSymbol simpleType && ConfigurationPathGenerator.IsSimpleType(simpleType);
    }

    /// <summary>Appends one source line using the emitter's four-space indentation convention.</summary>
    /// <param name="source">The source buffer receiving the line.</param>
    /// <param name="indent">The indentation depth.</param>
    /// <param name="value">The generated C# line without leading indentation.</param>
    private static void AppendLine(StringBuilder source, int indent, string value)
    {
        source.Append(' ', indent * 4).AppendLine(value);
    }

    /// <summary>Groups values shared by recursive emission without repeatedly threading unrelated arguments.</summary>
    private readonly ref struct DumpValueParameters
    {
        /// <summary>Initializes the source output buffer and generated traversal-state local names.</summary>
        /// <param name="source">The source buffer receiving generated statements.</param>
        /// <param name="valuesName">The result dictionary variable name.</param>
        /// <param name="visitedName">The shared reference-cycle tracking variable name.</param>
        internal DumpValueParameters(StringBuilder source, string valuesName, string visitedName)
        {
            Source = source;
            ValuesName = valuesName;
            VisitedName = visitedName;
        }

        /// <summary>Gets the source buffer receiving generated statements.</summary>
        internal StringBuilder Source { get; }

        /// <summary>Gets the generated identifier for the flattened result dictionary.</summary>
        internal string ValuesName { get; }

        /// <summary>Gets the generated identifier for reference-cycle tracking state.</summary>
        internal string VisitedName { get; }
    }
}
// AI GENERATED END
