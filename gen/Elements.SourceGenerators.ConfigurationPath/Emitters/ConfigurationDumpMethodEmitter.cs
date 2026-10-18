// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HedgeCraft.Elements.SourceGenerators.ConfigurationPath.CodeGeneration;
using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Emits the generated configuration dump API and its recursive value traversal.</summary>
internal static class ConfigurationDumpMethodEmitter
{
    private const string GlobalPrefix = "global::";

    private const string NestedContextPrefix = "__nestedContext_";

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
        source.IndentByLevel(1)
            .Append("public override global::System.Collections.Generic.IReadOnlyDictionary<string, object?> ")
            .Append("DumpConfigurationObject(")
            .Append(modelType)
            .AppendLine(" instance, bool considerWritablePropertiesOnly = true, params string[] parentKeys)");
        source.IndentByLevel(1).AppendLine("{");
        source.IndentByLevel(2).AppendLine("string[] keys = parentKeys ?? global::System.Array.Empty<string>();");
        source.IndentByLevel(2).AppendLine("return this.DumpConfigurationObjectCore(");
        source.IndentByLevel(3).AppendLine("instance,");
        source.IndentByLevel(3).AppendLine("considerWritablePropertiesOnly,");
        source.IndentByLevel(3).AppendLine("keys,");
        source.IndentByLevel(3).AppendLine("new global::System.Collections.Generic.HashSet<object>(");
        source.IndentByLevel(4).AppendLine("global::System.Collections.Generic.ReferenceEqualityComparer.Instance));");
        source.IndentByLevel(1).AppendLine("}");
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
        source.IndentByLevel(1)
            .Append("internal global::System.Collections.Generic.IReadOnlyDictionary<string, object?> ")
            .Append("DumpConfigurationObjectCore(")
            .Append(modelType)
            .AppendLine(
                " instance, bool considerWritablePropertiesOnly, string[] parentKeys, " +
                "global::System.Collections.Generic.HashSet<object> visited)");
        source.IndentByLevel(1).AppendLine("{");
        if (type.IsReferenceType)
        {
            source.IndentByLevel(2).AppendLine("if (instance is null)");
            source.IndentByLevel(2).AppendLine("{");
            source.IndentByLevel(3).AppendLine("throw new global::System.ArgumentNullException(nameof(instance));");
            source.IndentByLevel(2).AppendLine("}");
            source.AppendLine();
            source.IndentByLevel(2).AppendLine("if (!visited.Add(instance))");
            source.IndentByLevel(2).AppendLine("{");
            source.IndentByLevel(3)
                .AppendLine("return new global::System.Collections.Generic.Dictionary<string, object?>(");
            source.IndentByLevel(4).AppendLine("global::System.StringComparer.Ordinal);");
            source.IndentByLevel(2).AppendLine("}");
            source.AppendLine();
            source.IndentByLevel(2).AppendLine("try");
            source.IndentByLevel(2).AppendLine("{");
        }

        int bodyIndent = type.IsReferenceType ? 3 : 2;
        ConfigurationDumpValueEmitter.AppendBody(source, type, bodyIndent);

        if (type.IsReferenceType)
        {
            source.IndentByLevel(2).AppendLine("}");
            source.IndentByLevel(2).AppendLine("finally");
            source.IndentByLevel(2).AppendLine("{");
            source.IndentByLevel(3).AppendLine("visited.Remove(instance);");
            source.IndentByLevel(2).AppendLine("}");
        }

        source.IndentByLevel(1).AppendLine("}");
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
        reference.Append(GlobalPrefix);
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
        StringBuilder fieldName = new(NestedContextPrefix);
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
            : type.ContainingNamespace.ToDisplayString(FullyQualifiedFormat).Substring(GlobalPrefix.Length);
    }
}
// AI GENERATED END
