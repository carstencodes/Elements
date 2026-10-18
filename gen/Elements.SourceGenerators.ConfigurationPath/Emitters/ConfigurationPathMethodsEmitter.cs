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

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Emits the readable and writable configuration path methods for a context.</summary>
internal static class ConfigurationPathMethodsEmitter
{
    /// <summary>The fully qualified generic type name of IReadOnlySet.</summary>
    private static readonly string ReadOnlySetTypeName =
        $"global::{typeof(ISet<object>).Namespace}.IReadOnlySet";

    /// <summary>The fully qualified generic type name of HashSet.</summary>
    private static readonly string HashSetTypeName =
        $"global::{typeof(HashSet<object>).Namespace}.{nameof(HashSet<object>)}";

    /// <summary>The fully qualified type name of the ConfigurationPath struct.</summary>
    private const string PathTypeName =
        "global::HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys.ConfigurationPath";

    /// <summary>The fully qualified property name of ReferenceEqualityComparer.Instance.</summary>
    private static readonly string ReferenceEqualityComparerInstanceProperty =
        $"global::{typeof(EqualityComparer<object>).Namespace}.ReferenceEqualityComparer.Instance";

    /// <summary>
    /// Appends a generated method and its cycle-guarded traversal that returns configuration paths.
    /// </summary>
    /// <param name="source">The source buffer receiving the method.</param>
    /// <param name="methodName">The overridden path method's name.</param>
    /// <param name="paths">The direct property paths to emit in declaration-independent order.</param>
    /// <param name="delegations">The nested context delegations to invoke and append.</param>
    internal static void AppendMethod(
        StringBuilder source,
        string methodName,
        IReadOnlyCollection<string[]> paths,
        IReadOnlyCollection<NestedPathDelegation> delegations)
    {
        string coreMethodName = string.Concat(methodName, "Core");
        AppendPublicMethod(source, methodName, coreMethodName, paths, delegations.Count > 0);
        AppendCoreMethod(source, methodName, coreMethodName, paths, delegations);
    }

    /// <summary>Emits the public entry point for extracting configuration paths.</summary>
    /// <param name="source">The source buffer receiving the method declaration.</param>
    /// <param name="methodName">The public method name to emit.</param>
    /// <param name="coreMethodName">The core method name to invoke when delegations exist.</param>
    /// <param name="paths">The direct property paths to emit if no delegations exist.</param>
    /// <param name="hasDelegations">Whether nested delegations require recursive resolution.</param>
    private static void AppendPublicMethod(
        StringBuilder source,
        string methodName,
        string coreMethodName,
        IReadOnlyCollection<string[]> paths,
        bool hasDelegations)
    {
        source.IndentByLevel(1)
            .Append("public override ")
            .Append(ReadOnlySetTypeName)
            .Append('<').Append(PathTypeName).Append("> ")
            .Append(methodName)
            .AppendLine("()");
        source.IndentByLevel(1).AppendLine("{");

        if (!hasDelegations)
        {
            AppendDirectReturn(source, paths);
            return;
        }

        source.IndentByLevel(2)
            .Append("return this.")
            .Append(coreMethodName)
            .Append("(new ")
            .Append(HashSetTypeName)
            .AppendLine("<object>(");
        source.IndentByLevel(3)
            .Append(ReferenceEqualityComparerInstanceProperty)
            .AppendLine("));");
        source.IndentByLevel(1).AppendLine("}");
        source.AppendLine();
    }

    /// <summary>Emits the internal cycle-guarded core method that accumulates nested paths.</summary>
    /// <param name="source">The source buffer receiving the method declaration.</param>
    /// <param name="methodName">The public method name invoked for leaf/direct returns.</param>
    /// <param name="coreMethodName">The core method name.</param>
    /// <param name="paths">The direct property paths to initialize.</param>
    /// <param name="delegations">The nested context delegations to invoke.</param>
    private static void AppendCoreMethod(
        StringBuilder source,
        string methodName,
        string coreMethodName,
        IReadOnlyCollection<string[]> paths,
        IReadOnlyCollection<NestedPathDelegation> delegations)
    {
        source.IndentByLevel(1)
            .Append("internal ")
            .Append(ReadOnlySetTypeName)
            .Append('<').Append(PathTypeName).Append("> ")
            .Append(coreMethodName)
            .Append("(")
            .Append(HashSetTypeName)
            .AppendLine("<object> visited)");
        source.IndentByLevel(1).AppendLine("{");

        if (delegations.Count == 0)
        {
            source.IndentByLevel(2)
                .Append("return this.")
                .Append(methodName)
                .AppendLine("();");
            source.IndentByLevel(1).AppendLine("}");
            source.AppendLine();
            return;
        }

        AppendDelegatedBody(source, methodName, coreMethodName, paths, delegations);
    }

    /// <summary>Emits the core body with cycle tracking and nested context appending calls.</summary>
    /// <param name="source">The source buffer receiving the method statements.</param>
    /// <param name="methodName">The public method name for external user context delegations.</param>
    /// <param name="coreMethodName">The core method name for generated sibling context delegations.</param>
    /// <param name="paths">The direct property paths to initialize.</param>
    /// <param name="delegations">The nested context delegations to invoke.</param>
    private static void AppendDelegatedBody(
        StringBuilder source,
        string methodName,
        string coreMethodName,
        IReadOnlyCollection<string[]> paths,
        IReadOnlyCollection<NestedPathDelegation> delegations)
    {
        source.IndentByLevel(2).AppendLine("if (!visited.Add(this))");
        source.IndentByLevel(2).AppendLine("{");
        source.IndentByLevel(3)
            .Append("return new ")
            .Append(HashSetTypeName)
            .Append('<').Append(PathTypeName).AppendLine(">();");
        source.IndentByLevel(2).AppendLine("}");
        source.AppendLine();
        source.IndentByLevel(2).AppendLine("try");
        source.IndentByLevel(2).AppendLine("{");
        source.IndentByLevel(3)
            .Append(HashSetTypeName)
            .Append('<').Append(PathTypeName).AppendLine("> paths = new()");
        AppendPathElements(source, paths, 3, ";");
        source.AppendLine();

        foreach (NestedPathDelegation delegation in delegations)
        {
            string targetMethod = delegation.IsUserContext ? methodName : coreMethodName;
            string targetArgs = delegation.IsUserContext ? string.Empty : "visited";
            source.IndentByLevel(3).AppendLine("AppendNestedPaths(");
            source.IndentByLevel(4).AppendLine("paths,");
            if (delegation.KeyPath.Length == 1)
            {
                source.IndentByLevel(4)
                    .Append(SymbolDisplay.FormatLiteral(delegation.KeyPath[0], true))
                    .AppendLine(",");
            }
            else
            {
                source.IndentByLevel(4)
                    .Append("new string[] { ")
                    .Append(string.Join(
                        ", ",
                        delegation.KeyPath.Select(static key => SymbolDisplay.FormatLiteral(key, true))))
                    .AppendLine(" },");
            }

            source.IndentByLevel(4)
                .Append(delegation.ContextFieldName)
                .Append('.')
                .Append(targetMethod)
                .Append('(')
                .Append(targetArgs)
                .AppendLine("));");
        }

        source.AppendLine();
        source.IndentByLevel(3).AppendLine("return paths;");
        source.IndentByLevel(2).AppendLine("}");
        source.IndentByLevel(2).AppendLine("finally");
        source.IndentByLevel(2).AppendLine("{");
        source.IndentByLevel(3).AppendLine("visited.Remove(this);");
        source.IndentByLevel(2).AppendLine("}");
        source.IndentByLevel(1).AppendLine("}");
        source.AppendLine();
    }

    /// <summary>Emits a direct return of the local property path set when no nested delegations exist.</summary>
    /// <param name="source">The source buffer receiving the method body.</param>
    /// <param name="paths">The direct property paths to return.</param>
    private static void AppendDirectReturn(
        StringBuilder source,
        IReadOnlyCollection<string[]> paths)
    {
        source.IndentByLevel(2)
            .Append("return new ")
            .Append(HashSetTypeName)
            .Append('<').Append(PathTypeName).AppendLine(">");
        AppendPathElements(source, paths, 2, ";");
        source.IndentByLevel(1).AppendLine("}");
        source.AppendLine();
    }

    /// <summary>Emits a collection initializer containing the initial path set.</summary>
    /// <param name="source">The source buffer receiving the path initializers.</param>
    /// <param name="paths">The collection of string segment arrays to emit.</param>
    /// <param name="level">The indentation level of the initializer braces.</param>
    /// <param name="terminator">The text appended after the closing brace.</param>
    private static void AppendPathElements(
        StringBuilder source,
        IReadOnlyCollection<string[]> paths,
        int level,
        string terminator)
    {
        source.IndentByLevel(level).AppendLine("{");
        foreach (string[] path in paths)
        {
            source.IndentByLevel(level + 1)
                .Append("new ")
                .Append(PathTypeName)
                .Append('(')
                .Append(string.Join(", ", path.Select(static key => SymbolDisplay.FormatLiteral(key, true))))
                .AppendLine("),");
        }

        source.IndentByLevel(level).Append('}').AppendLine(terminator);
    }
}
// AI GENERATED END
