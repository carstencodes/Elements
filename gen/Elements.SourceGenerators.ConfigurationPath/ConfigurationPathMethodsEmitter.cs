// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: Copilot
/// <summary>Emits the readable and writable configuration path methods for a context.</summary>
internal static class ConfigurationPathMethodsEmitter
{
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
        source.Append("    public override global::System.Collections.Generic.IReadOnlySet<string[]> ")
            .Append(methodName)
            .AppendLine("()");
        source.AppendLine("    {");
        source.AppendLine("        return new global::System.Collections.Generic.HashSet<string[]>(");
        source.AppendLine("            new string[][]");
        source.AppendLine("            {");
        foreach (string[] path in paths)
        {
            source.Append("                new string[] { ")
                .Append(string.Join(", ", path.Select(static key => SymbolDisplay.FormatLiteral(key, true))))
                .AppendLine(" },");
        }

        // Arrays otherwise compare by reference, so the generated set uses a structural path comparer.
        source.AppendLine("            },");
        source.AppendLine("            new ConfigurationPathComparer());");
        source.AppendLine("    }");
        source.AppendLine();
    }
}
// AI GENERATED END
