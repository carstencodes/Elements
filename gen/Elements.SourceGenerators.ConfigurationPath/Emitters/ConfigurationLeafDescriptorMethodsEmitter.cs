// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Generic;
using System.Linq;
using System.Text;
using HedgeCraft.Elements.SourceGenerators.ConfigurationPath.CodeGeneration;
using HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters.Options;
using Microsoft.CodeAnalysis.CSharp;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters;

// AI GENERATED START - model: Claude Sonnet 5.5
/// <summary>Emits the bindable configuration leaf descriptor method for a context.</summary>
internal static class ConfigurationLeafDescriptorMethodsEmitter
{
    /// <summary>The overridden public method name.</summary>
    internal const string MethodName = "GetBindableConfigurationLeafDescriptors";

    /// <summary>The cycle-guarded internal method name used by generated sibling contexts.</summary>
    private const string CoreMethodName = MethodName + "Core";

    /// <summary>The fully qualified type name of the ConfigurationPath struct.</summary>
    private const string PathTypeName =
        "global::HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys.ConfigurationPath";

    /// <summary>The fully qualified type name of the ConfigurationLeafDescriptor struct.</summary>
    private const string DescriptorTypeName =
        "global::HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys.ConfigurationLeafDescriptor";

    /// <summary>The fully qualified generic type name of IReadOnlySet.</summary>
    private static readonly string ReadOnlySetTypeName =
        $"global::{typeof(ISet<object>).Namespace}.IReadOnlySet";

    /// <summary>The fully qualified generic type name of HashSet.</summary>
    private static readonly string HashSetTypeName =
        $"global::{typeof(HashSet<object>).Namespace}.{nameof(HashSet<object>)}";

    /// <summary>The fully qualified property name of ReferenceEqualityComparer.Instance.</summary>
    private static readonly string ReferenceEqualityComparerInstanceProperty =
        $"global::{typeof(EqualityComparer<object>).Namespace}.ReferenceEqualityComparer.Instance";

    /// <summary>Appends the public override and its cycle-guarded core method.</summary>
    /// <param name="source">The source buffer receiving the methods.</param>
    /// <param name="leaves">The direct leaves of the context's model.</param>
    /// <param name="delegations">The nested context delegations; collection value delegations are ignored.</param>
    internal static void AppendMethod(
        StringBuilder source,
        IReadOnlyCollection<LeafDescriptorEntry> leaves,
        IReadOnlyCollection<NestedPathDelegation> delegations)
    {
        // Collection values have no compile-time index or key, so their leaves cannot be addressed.
        List<NestedPathDelegation> objectDelegations = delegations.Where(static d => !d.IsCollectionValue).ToList();
        AppendPublicMethod(source, leaves, objectDelegations.Count > 0);
        AppendCoreMethod(source, leaves, objectDelegations);
    }

    /// <summary>Emits the public entry point returning the leaf descriptor set.</summary>
    /// <param name="source">The source buffer receiving the method declaration.</param>
    /// <param name="leaves">The direct leaves returned when no delegations exist.</param>
    /// <param name="hasDelegations">Whether nested contexts require recursive resolution.</param>
    private static void AppendPublicMethod(
        StringBuilder source,
        IReadOnlyCollection<LeafDescriptorEntry> leaves,
        bool hasDelegations)
    {
        source.IndentByLevel(1)
            .Append("public override ")
            .Append(ReadOnlySetTypeName)
            .Append('<').Append(DescriptorTypeName).Append("> ")
            .Append(MethodName)
            .AppendLine("()");
        source.IndentByLevel(1).AppendLine("{");
        if (hasDelegations)
        {
            source.IndentByLevel(2)
                .Append("return this.").Append(CoreMethodName)
                .Append("(new ").Append(HashSetTypeName).AppendLine("<object>(");
            source.IndentByLevel(3).Append(ReferenceEqualityComparerInstanceProperty).AppendLine("));");
        }
        else
        {
            source.IndentByLevel(2)
                .Append("return new ").Append(HashSetTypeName)
                .Append('<').Append(DescriptorTypeName).AppendLine(">");
            AppendLeafElements(source, leaves, 2, ";");
        }

        source.IndentByLevel(1).AppendLine("}");
        source.AppendLine();
    }

    /// <summary>Emits the internal core method used for cycle-guarded nested traversal.</summary>
    /// <param name="source">The source buffer receiving the method declaration.</param>
    /// <param name="leaves">The direct leaves to initialize.</param>
    /// <param name="delegations">The nested context delegations to invoke.</param>
    private static void AppendCoreMethod(
        StringBuilder source,
        IReadOnlyCollection<LeafDescriptorEntry> leaves,
        IReadOnlyCollection<NestedPathDelegation> delegations)
    {
        source.IndentByLevel(1)
            .Append("internal ").Append(ReadOnlySetTypeName)
            .Append('<').Append(DescriptorTypeName).Append("> ")
            .Append(CoreMethodName).Append('(')
            .Append(HashSetTypeName).AppendLine("<object> visited)");
        source.IndentByLevel(1).AppendLine("{");
        if (delegations.Count == 0)
        {
            source.IndentByLevel(2).Append("return this.").Append(MethodName).AppendLine("();");
        }
        else
        {
            AppendDelegatedBody(source, leaves, delegations);
        }

        source.IndentByLevel(1).AppendLine("}");
        source.AppendLine();
    }

    /// <summary>Emits the core body with cycle tracking and nested descriptor merging.</summary>
    /// <param name="source">The source buffer receiving the method statements.</param>
    /// <param name="leaves">The direct leaves to initialize.</param>
    /// <param name="delegations">The nested context delegations to invoke.</param>
    private static void AppendDelegatedBody(
        StringBuilder source,
        IReadOnlyCollection<LeafDescriptorEntry> leaves,
        IReadOnlyCollection<NestedPathDelegation> delegations)
    {
        source.IndentByLevel(2).AppendLine("if (!visited.Add(this))");
        source.IndentByLevel(2).AppendLine("{");
        source.IndentByLevel(3)
            .Append("return new ").Append(HashSetTypeName)
            .Append('<').Append(DescriptorTypeName).AppendLine(">();");
        source.IndentByLevel(2).AppendLine("}");
        source.AppendLine();
        source.IndentByLevel(2).AppendLine("try");
        source.IndentByLevel(2).AppendLine("{");
        source.IndentByLevel(3)
            .Append(HashSetTypeName)
            .Append('<').Append(DescriptorTypeName).AppendLine("> leaves = new()");
        AppendLeafElements(source, leaves, 3, ";");
        source.AppendLine();
        foreach (NestedPathDelegation delegation in delegations)
        {
            AppendDelegationCall(source, delegation);
        }

        source.AppendLine();
        source.IndentByLevel(3).AppendLine("return leaves;");
        source.IndentByLevel(2).AppendLine("}");
        source.IndentByLevel(2).AppendLine("finally");
        source.IndentByLevel(2).AppendLine("{");
        source.IndentByLevel(3).AppendLine("visited.Remove(this);");
        source.IndentByLevel(2).AppendLine("}");
    }

    /// <summary>Emits one prefixing merge of a nested context's leaf descriptors.</summary>
    /// <param name="source">The source buffer receiving the statement.</param>
    /// <param name="delegation">The nested context delegation to invoke.</param>
    private static void AppendDelegationCall(StringBuilder source, NestedPathDelegation delegation)
    {
        // User contexts are opaque, so only their public method is available; siblings share the visited set.
        string targetMethod = delegation.IsUserContext ? MethodName : CoreMethodName;
        string targetArgs = delegation.IsUserContext ? string.Empty : "visited";
        source.IndentByLevel(3).AppendLine("AppendNestedLeafDescriptors(");
        source.IndentByLevel(4).AppendLine("leaves,");
        source.IndentByLevel(4).Append(CreateKeyArgument(delegation.KeyPath)).AppendLine(",");
        source.IndentByLevel(4)
            .Append(delegation.ContextFieldName).Append('.')
            .Append(targetMethod).Append('(').Append(targetArgs).AppendLine("));");
    }

    /// <summary>Creates the key argument for a nested descriptor merge.</summary>
    /// <param name="keyPath">The parent key segments.</param>
    /// <returns>A string literal for one segment, otherwise a string array creation expression.</returns>
    private static string CreateKeyArgument(string[] keyPath)
    {
        if (keyPath.Length == 1)
        {
            return SymbolDisplay.FormatLiteral(keyPath[0], true);
        }

        return string.Concat(
            "new string[] { ",
            string.Join(", ", keyPath.Select(static key => SymbolDisplay.FormatLiteral(key, true))),
            " }");
    }

    /// <summary>Emits a collection initializer containing the direct leaf descriptors.</summary>
    /// <param name="source">The source buffer receiving the initializers.</param>
    /// <param name="leaves">The leaves to emit.</param>
    /// <param name="level">The indentation level of the initializer braces.</param>
    /// <param name="terminator">The text appended after the closing brace.</param>
    private static void AppendLeafElements(
        StringBuilder source,
        IReadOnlyCollection<LeafDescriptorEntry> leaves,
        int level,
        string terminator)
    {
        source.IndentByLevel(level).AppendLine("{");
        foreach (LeafDescriptorEntry leaf in leaves)
        {
            source.IndentByLevel(level + 1)
                .Append("new ").Append(DescriptorTypeName).AppendLine("(");
            source.IndentByLevel(level + 2)
                .Append("new ").Append(PathTypeName).Append('(')
                .Append(string.Join(", ", leaf.Path.Select(static key => SymbolDisplay.FormatLiteral(key, true))))
                .AppendLine("),");
            source.IndentByLevel(level + 2)
                .Append("typeof(").Append(leaf.TypeName).AppendLine("),");
            source.IndentByLevel(level + 2)
                .Append(SymbolDisplay.FormatLiteral(leaf.Description, true)).AppendLine("),");
        }

        source.IndentByLevel(level).Append('}').AppendLine(terminator);
    }
}
// AI GENERATED END
