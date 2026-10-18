// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Generic;
using System.Linq;
using System.Text;
using HedgeCraft.Elements.SourceGenerators.ConfigurationPath.CodeGeneration;
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

    /// <summary>
    /// Appends a generated method that returns its configuration paths as an immutable-by-contract set.
    /// </summary>
    /// <param name="source">The source buffer receiving the method.</param>
    /// <param name="methodName">The overridden path method's name.</param>
    /// <param name="paths">The property paths to emit in declaration-independent order.</param>
    internal static void AppendMethod(
        StringBuilder source,
        string methodName,
        IReadOnlyCollection<string[]> paths)
    {
        source.IndentByLevel(1)
            .Append("public override ")
            .Append(ReadOnlySetTypeName)
            .Append("<string[]> ")
            .Append(methodName)
            .AppendLine("()");
        source.IndentByLevel(1).AppendLine("{");
        source.IndentByLevel(2)
            .Append("return new ")
            .Append(HashSetTypeName)
            .AppendLine("<string[]>(");
        source.IndentByLevel(3).AppendLine("new string[][]");
        source.IndentByLevel(3).AppendLine("{");
        foreach (string[] path in paths)
        {
            source.IndentByLevel(4)
                .Append("new string[] { ")
                .Append(string.Join(", ", path.Select(static key => SymbolDisplay.FormatLiteral(key, true))))
                .AppendLine(" },");
        }

        // Arrays otherwise compare by reference, so the generated set uses a structural path comparer.
        source.IndentByLevel(3).AppendLine("},");
        source.IndentByLevel(3).AppendLine("new ConfigurationPathComparer());");
        source.IndentByLevel(1).AppendLine("}");
        source.AppendLine();
    }
}
// AI GENERATED END
