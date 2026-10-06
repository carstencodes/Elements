// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: Copilot
/// <summary>Emits the generated configuration dump API and its recursive value traversal.</summary>
internal static class ConfigurationDumpMethodEmitter
{
    /// <summary>Formats model types consistently in emitted signatures and nested context references.</summary>
    private static readonly SymbolDisplayFormat FullyQualifiedFormat = SymbolDisplayFormat.FullyQualifiedFormat;

    /// <summary>Appends the public API and shared-core traversal method to the context declaration.</summary>
    /// <param name="source">The source buffer receiving generated members.</param>
    /// <param name="type">The model represented by the generated context.</param>
    internal static void AppendMethods(StringBuilder source, INamedTypeSymbol type)
    {
        AppendDumpMethods(source, type);
    }

    /// <summary>Resolves the model's source spelling once, then emits its wrapper and traversal core.</summary>
    /// <param name="source">The source buffer receiving generated methods.</param>
    /// <param name="type">The model represented by the generated context.</param>
    private static void AppendDumpMethods(StringBuilder source, INamedTypeSymbol type)
    {
        string modelType = type.ToDisplayString(FullyQualifiedFormat);
        AppendPublicDumpMethod(source, modelType);
        AppendCoreDumpMethod(source, type, modelType);
    }

    /// <summary>Emits the public entry point and creates one reference-identity cycle set for the whole dump.</summary>
    /// <param name="source">The source buffer receiving the method.</param>
    /// <param name="modelType">The fully qualified model type used in the method signature.</param>
    private static void AppendPublicDumpMethod(StringBuilder source, string modelType)
    {
        source.Append("    public override global::System.Collections.Generic.IReadOnlyDictionary<string, object?> ")
            .Append("DumpConfigurationObject(")
            .Append(modelType)
            .AppendLine(" instance, bool considerWritablePropertiesOnly = true, params string[] parentKeys)");
        source.AppendLine("    {");
        source.AppendLine("        string[] keys = parentKeys ?? global::System.Array.Empty<string>();");
        source.AppendLine("        return this.DumpConfigurationObjectCore(");
        source.AppendLine("            instance,");
        source.AppendLine("            considerWritablePropertiesOnly,");
        source.AppendLine("            keys,");
        source.AppendLine("            new global::System.Collections.Generic.HashSet<object>(");
        source.AppendLine("                global::System.Collections.Generic.ReferenceEqualityComparer.Instance));");
        source.AppendLine("    }");
        source.AppendLine();
    }

    /// <summary>Emits traversal state handling, preserving cleanup even when a property's getter throws.</summary>
    /// <param name="source">The source buffer receiving the method.</param>
    /// <param name="type">The model whose properties the core traverses.</param>
    /// <param name="modelType">The fully qualified model type used in the method signature.</param>
    private static void AppendCoreDumpMethod(
        StringBuilder source,
        INamedTypeSymbol type,
        string modelType)
    {
        source.Append("    internal global::System.Collections.Generic.IReadOnlyDictionary<string, object?> ")
            .Append("DumpConfigurationObjectCore(")
            .Append(modelType)
            .AppendLine(
                " instance, bool considerWritablePropertiesOnly, string[] parentKeys, " +
                "global::System.Collections.Generic.HashSet<object> visited)");
        source.AppendLine("    {");
        if (type.IsReferenceType)
        {
            source.AppendLine("        if (instance is null)");
            source.AppendLine("        {");
            source.AppendLine("            throw new global::System.ArgumentNullException(nameof(instance));");
            source.AppendLine("        }");
            source.AppendLine();
            source.AppendLine("        if (!visited.Add(instance))");
            source.AppendLine("        {");
            source.AppendLine("            return new global::System.Collections.Generic.Dictionary<string, object?>(");
            source.AppendLine("                global::System.StringComparer.Ordinal);");
            source.AppendLine("        }");
            source.AppendLine();
            source.AppendLine("        try");
            source.AppendLine("        {");
        }

        int bodyIndent = type.IsReferenceType ? 3 : 2;
        ConfigurationDumpValueEmitter.AppendBody(source, type, bodyIndent);

        if (type.IsReferenceType)
        {
            source.AppendLine("        }");
            source.AppendLine("        finally");
            source.AppendLine("        {");
            source.AppendLine("            visited.Remove(instance);");
            source.AppendLine("        }");
        }

        source.AppendLine("    }");
        source.AppendLine();
    }

    /// <summary>Formats either an existing consumer context or the generated sibling context for a model.</summary>
    /// <param name="modelType">The concrete model type to be passed to the context.</param>
    /// <returns>A globally qualified context type, with constructed generic arguments when necessary.</returns>
    internal static string CreateContextReference(INamedTypeSymbol modelType)
    {
        INamedTypeSymbol definition = modelType.OriginalDefinition;
        INamedTypeSymbol? existingContext = ConfigurationPathGenerator.FindExistingContext(definition);
        ImmutableArray<ITypeSymbol> typeArguments = ConfigurationPathGenerator.GetTypeArguments(modelType);
        if (existingContext is not null)
        {
            return existingContext.Arity == 0
                ? existingContext.ToDisplayString(FullyQualifiedFormat)
                : existingContext.Construct(typeArguments.ToArray()).ToDisplayString(FullyQualifiedFormat);
        }

        string namespaceName = GetNamespaceName(definition);
        string name = ConfigurationPathGenerator.CreateContextName(
            definition,
            ConfigurationPathGenerator.GetTypeParameters(definition));
        StringBuilder reference = new();
        reference.Append("global::");
        if (namespaceName.Length > 0)
        {
            reference.Append(namespaceName).Append('.');
        }

        reference.Append(name);
        if (typeArguments.Length > 0)
        {
            reference.Append('<')
                .Append(string.Join(
                    ", ",
                    typeArguments.Select(static argument => argument.ToDisplayString(FullyQualifiedFormat))))
                .Append('>');
        }

        return reference.ToString();
    }

    /// <summary>Creates a deterministic valid field identifier for a cached nested context instance.</summary>
    /// <param name="modelType">The context's model type.</param>
    /// <returns>A unique field name incorporating a readable model name and a short stable hash.</returns>
    internal static string CreateContextFieldName(INamedTypeSymbol modelType)
    {
        string symbolName = modelType.ToDisplayString(FullyQualifiedFormat);
        StringBuilder fieldName = new("__nestedContext_");
        foreach (char character in symbolName)
        {
            fieldName.Append(char.IsLetterOrDigit(character) ? character : '_');
        }

        // Keep the readable prefix for generated-code diagnostics and hash it to disambiguate sanitized names.
        using SHA256 algorithm = SHA256.Create();
        byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(symbolName));
        return string.Concat(fieldName, "_", BitConverter.ToString(hash, 0, 6).Replace("-", string.Empty));
    }

    /// <summary>Returns a namespace without the <c>global::</c> prefix for generated declarations.</summary>
    /// <param name="type">The type whose containing namespace is requested.</param>
    /// <returns>An empty string for the global namespace, otherwise the namespace's C# name.</returns>
    private static string GetNamespaceName(INamedTypeSymbol type)
    {
        return type.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : type.ContainingNamespace.ToDisplayString(FullyQualifiedFormat).Substring("global::".Length);
    }
}
// AI GENERATED END
