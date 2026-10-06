// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

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
        DumpEmissionContext context = new(source, "values", "visited");
        foreach (IPropertySymbol property in ConfigurationPathGenerator.GetBindableProperties(type))
        {
            PropertyValueOptions options = new(property, "instance", "parentKeys", indent);
            // Only simple read-only values are omitted: retaining models and collections preserves their child paths.
            if (IsSimpleProperty(property.Type) &&
                property.SetMethod?.DeclaredAccessibility != Accessibility.Public)
            {
                AppendLine(source, indent, "if (!considerWritablePropertiesOnly)");
                AppendLine(source, indent, "{");
                AppendDumpProperty(context, options with { Indent = indent + 1 });
                AppendLine(source, indent, "}");
                continue;
            }

            AppendDumpProperty(context, options);
        }

        AppendLine(source, indent, "return values;");
    }

    /// <summary>Reads a property once and routes its value through the type-specific traversal emitter.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The property and its containing model expressions.</param>
    private static void AppendDumpProperty(DumpEmissionContext context, PropertyValueOptions options)
    {
        string localName = string.Concat("__propertyValue", context.NextLocalIndex());
        AppendLine(
            context.Source,
            options.Indent,
            string.Concat("var ", localName, " = ", options.InstanceExpression, ".")
                + ConfigurationPathGenerator.EscapeIdentifier(options.Property.Name)
                + ";");
        string keysExpression = CreateKeysExpression(options.ParentKeysExpression, options.Property);
        AppendDumpValue(
            context,
            new DumpValueOptions(options.Property.Type, localName, keysExpression, options.Indent));
    }

    /// <summary>Selects scalar, array, named-type, and collection emission for one value.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The value's type and generated expressions.</param>
    internal static void AppendDumpValue(DumpEmissionContext context, DumpValueOptions options)
    {
        if (options.Type is IArrayTypeSymbol arrayType)
        {
            AppendArrayValue(context, options, arrayType);
            return;
        }

        if (options.Type is INamedTypeSymbol namedType)
        {
            AppendNamedValue(context, options with { Type = namedType });
            return;
        }

        AppendScalar(context, options);
    }

    private static void AppendArrayValue(
        DumpEmissionContext context,
        DumpValueOptions options,
        IArrayTypeSymbol arrayType)
    {
        if (ConfigurationPathGenerator.IsByteArray(arrayType))
        {
            AppendScalar(context, options);
            return;
        }

        ConfigurationDumpCollectionEmitter.AppendCollectionValue(
            context,
            new CollectionValueOptions(
                arrayType.ElementType,
                options.ValueExpression,
                options.KeysExpression,
                options.Indent,
                CanBeNull: true,
                IsDictionaryValue: false));
    }

    /// <summary>Dispatches named values to nullable, scalar, collection, or nested-model handling.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The named value's type and generated expressions.</param>
    private static void AppendNamedValue(DumpEmissionContext context, DumpValueOptions options)
    {
        INamedTypeSymbol type = (INamedTypeSymbol)options.Type;
        if (IsNullableValueType(type))
        {
            ConfigurationDumpCollectionEmitter.AppendNullableValue(context, options);
            return;
        }

        if (ConfigurationPathGenerator.IsSimpleType(type))
        {
            AppendScalar(context, options);
            return;
        }

        if (AppendNamedCollection(context, options))
        {
            return;
        }

        AppendModelValue(context, options);
    }

    /// <summary>Emits dictionary or enumerable handling when the named type is a supported collection.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The collection type and generated expressions.</param>
    /// <returns><see langword="true"/> when collection handling was emitted.</returns>
    private static bool AppendNamedCollection(DumpEmissionContext context, DumpValueOptions options)
    {
        INamedTypeSymbol type = (INamedTypeSymbol)options.Type;
        INamedTypeSymbol? dictionary = ConfigurationPathGenerator.GetDictionaryContract(type);
        if (dictionary is not null)
        {
            AppendDictionaryCollection(context, options, dictionary);
            return true;
        }

        if (ConfigurationPathGenerator.TryGetCollectionValues(type, out ImmutableArray<ITypeSymbol> values))
        {
            ConfigurationDumpCollectionEmitter.AppendGenericCollectionValues(
                context,
                new GenericCollectionValuesOptions(
                    values,
                    options.ValueExpression,
                    options.KeysExpression,
                    options.Indent,
                    type.IsReferenceType));
            return true;
        }

        return false;
    }

    private static void AppendDictionaryCollection(
        DumpEmissionContext context,
        DumpValueOptions options,
        INamedTypeSymbol dictionary)
    {
        ConfigurationDumpCollectionEmitter.AppendCollectionValue(
            context,
            new CollectionValueOptions(
                dictionary.TypeArguments[1],
                options.ValueExpression,
                options.KeysExpression,
                options.Indent,
                ((INamedTypeSymbol)options.Type).IsReferenceType,
                IsDictionaryValue: true));
    }

    /// <summary>Chooses inline expansion for closed generic models or delegates to a generated context.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The model type and generated expressions.</param>
    private static void AppendModelValue(DumpEmissionContext context, DumpValueOptions options)
    {
        INamedTypeSymbol type = (INamedTypeSymbol)options.Type;
        if (IsClosedGenericObject(type) &&
            ConfigurationPathGenerator.FindExistingContext(type.OriginalDefinition) is null)
        {
            AppendInlineGenericModel(context, options);
            return;
        }

        AppendContextDump(context, options);
    }

    /// <summary>Expands a closed generic model whose open definition cannot have a concrete context.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The model type and generated expressions.</param>
    private static void AppendInlineGenericModel(DumpEmissionContext context, DumpValueOptions options)
    {
        bool trackReference = options.Type.IsReferenceType;
        if (trackReference)
        {
            string condition = string.Concat(
                "if (",
                options.ValueExpression,
                " is not null && ",
                context.VisitedName,
                ".Add(",
                options.ValueExpression,
                "))");
            AppendLine(context.Source, options.Indent, condition);
            AppendLine(context.Source, options.Indent, "{");
            AppendLine(context.Source, options.Indent + 1, "try");
            AppendLine(context.Source, options.Indent + 1, "{");
        }

        int propertyIndent = options.Indent + (trackReference ? 2 : 0);
        AppendInlineGenericProperties(context, options with { Indent = propertyIndent });

        if (trackReference)
        {
            AppendLine(context.Source, options.Indent + 1, "}");
            AppendLine(context.Source, options.Indent + 1, "finally");
            AppendLine(context.Source, options.Indent + 1, "{");
            AppendLine(
                context.Source,
                options.Indent + 2,
                string.Concat(context.VisitedName, ".Remove(", options.ValueExpression, ");"));
            AppendLine(context.Source, options.Indent + 1, "}");
            AppendLine(context.Source, options.Indent, "}");
        }
    }

    /// <summary>Emits inline generic properties using the root model's writable-only filter.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The closed generic model and its expressions.</param>
    private static void AppendInlineGenericProperties(DumpEmissionContext context, DumpValueOptions options)
    {
        foreach (IPropertySymbol property in
                 ConfigurationPathGenerator.GetBindableProperties((INamedTypeSymbol)options.Type))
        {
            PropertyValueOptions propertyOptions = new(
                property,
                options.ValueExpression,
                options.KeysExpression,
                options.Indent);
            if (IsSimpleReadOnlyProperty(property))
            {
                AppendLine(context.Source, options.Indent, "if (!considerWritablePropertiesOnly)");
                AppendLine(context.Source, options.Indent, "{");
                AppendInlineGenericProperty(
                    context,
                    propertyOptions with { Indent = options.Indent + 1 });
                AppendLine(context.Source, options.Indent, "}");
                continue;
            }

            AppendInlineGenericProperty(context, propertyOptions);
        }
    }

    /// <summary>Reads one inline generic property and delegates to its value emitter.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The property and containing model expressions.</param>
    private static void AppendInlineGenericProperty(
        DumpEmissionContext context,
        PropertyValueOptions options)
    {
        string valueName = string.Concat("__genericProperty", context.NextLocalIndex());
        AppendLine(
            context.Source,
            options.Indent,
            string.Concat(
                "var ",
                valueName,
                " = ",
                options.InstanceExpression,
                ".",
                ConfigurationPathGenerator.EscapeIdentifier(options.Property.Name),
                ";"));
        string keysExpression = CreateKeysExpression(options.ParentKeysExpression, options.Property);

        if (options.Property.Type is INamedTypeSymbol nestedType &&
            IsClosedGenericObject(nestedType) &&
            ConfigurationPathGenerator.FindExistingContext(nestedType.OriginalDefinition) is null)
        {
            AppendContextDump(
                context,
                new DumpValueOptions(nestedType, valueName, keysExpression, options.Indent));
            return;
        }

        AppendDumpValue(
            context,
            new DumpValueOptions(options.Property.Type, valueName, keysExpression, options.Indent));
    }

    /// <summary>Checks whether a generic model can be expanded with concrete type arguments at this call site.</summary>
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

    /// <summary>Invokes the cached child context and merges its flattened values under the child path.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The nested model type and generated expressions.</param>
    private static void AppendContextDump(DumpEmissionContext context, DumpValueOptions options)
    {
        INamedTypeSymbol modelType = (INamedTypeSymbol)options.Type;
        string contextVariable = ConfigurationDumpMethodEmitter.CreateContextFieldName(modelType);
        bool existingContext = ConfigurationPathGenerator.FindExistingContext(modelType.OriginalDefinition) is not null;
        // User-provided contexts expose only the public API; generated contexts share the parent's cycle set.
        string methodName = existingContext ? "DumpConfigurationObject" : "DumpConfigurationObjectCore";
        string arguments = CreateContextArguments(context, options, existingContext);
        int bodyIndent = options.Indent;
        if (modelType.IsReferenceType)
        {
            AppendLine(
                context.Source,
                bodyIndent,
                string.Concat("if (", options.ValueExpression, " is not null)"));
            AppendLine(context.Source, bodyIndent, "{");
            bodyIndent++;
        }

        AppendContextCall(
            context,
            new ContextCallOptions(contextVariable, methodName, arguments, bodyIndent));

        if (modelType.IsReferenceType)
        {
            AppendLine(context.Source, options.Indent, "}");
        }
    }

    private static string CreateContextArguments(
        DumpEmissionContext context,
        DumpValueOptions options,
        bool existingContext)
    {
        return existingContext
            ? string.Concat(options.ValueExpression, ", considerWritablePropertiesOnly, ", options.KeysExpression)
            : string.Concat(
                options.ValueExpression,
                ", considerWritablePropertiesOnly, ",
                options.KeysExpression,
                ", ",
                context.VisitedName);
    }

    /// <summary>Emits the child-context loop that copies its entries into the current result dictionary.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The child context call and generated arguments.</param>
    private static void AppendContextCall(DumpEmissionContext context, ContextCallOptions options)
    {
        string entryVariable = string.Concat(options.ContextVariable, "Entry");
        AppendLine(
            context.Source,
            options.Indent,
            string.Concat(
                "foreach (global::System.Collections.Generic.KeyValuePair<string, object?> ",
                entryVariable,
                " in ",
                options.ContextVariable,
                ".",
                options.MethodName,
                "(",
                options.Arguments,
                "))"));
        AppendLine(context.Source, options.Indent, "{");
        AppendLine(
            context.Source,
            options.Indent + 1,
            string.Concat(context.ValuesName, "[", entryVariable, ".Key] = ", entryVariable, ".Value;"));
        AppendLine(context.Source, options.Indent, "}");
    }

    /// <summary>Emits assignment at the colon-joined configuration key, including a null scalar value.</summary>
    /// <param name="context">Shared source and traversal state for the dump.</param>
    /// <param name="options">The scalar's value and generated path expressions.</param>
    internal static void AppendScalar(DumpEmissionContext context, DumpValueOptions options)
    {
        AppendLine(
            context.Source,
            options.Indent,
            string.Concat(
                context.ValuesName,
                "[global::System.String.Join(\":\", ",
                options.KeysExpression,
                ")] = ",
                options.ValueExpression,
                ";"));
    }

    private static string CreateKeysExpression(string parentKeysExpression, IPropertySymbol property)
    {
        return string.Concat(
            "AppendKeys(",
            parentKeysExpression,
            ", ",
            SymbolDisplay.FormatLiteral(ConfigurationPathGenerator.GetKeyName(property), true),
            ")");
    }

    private static bool IsNullableValueType(INamedTypeSymbol type)
    {
        return type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;
    }

    /// <summary>Classifies nullable scalar and named scalar types for writable-only property filtering.</summary>
    /// <param name="type">The property type to classify.</param>
    /// <returns><see langword="true"/> when the type is a scalar rather than a nested configuration model.</returns>
    private static bool IsSimpleProperty(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol namedType && IsNullableValueType(namedType))
        {
            return ConfigurationPathGenerator.IsSimpleType((INamedTypeSymbol)namedType.TypeArguments[0]);
        }

        return type is INamedTypeSymbol simpleType && ConfigurationPathGenerator.IsSimpleType(simpleType);
    }

    private static bool IsSimpleReadOnlyProperty(IPropertySymbol property)
    {
        return IsSimpleProperty(property.Type) &&
            property.SetMethod?.DeclaredAccessibility != Accessibility.Public;
    }

    /// <summary>Appends one source line using the emitter's four-space indentation convention.</summary>
    /// <param name="source">The source buffer receiving the line.</param>
    /// <param name="indent">The indentation depth.</param>
    /// <param name="value">The generated C# line without leading indentation.</param>
    private static void AppendLine(StringBuilder source, int indent, string value)
    {
        source.Append(' ', indent * 4).AppendLine(value);
    }

    private readonly record struct PropertyValueOptions(
        IPropertySymbol Property,
        string InstanceExpression,
        string ParentKeysExpression,
        int Indent);

    private readonly record struct ContextCallOptions(
        string ContextVariable,
        string MethodName,
        string Arguments,
        int Indent);
}
// AI GENERATED END
