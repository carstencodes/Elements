// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.CodeGeneration;

internal static class StringBuilderExtensions
{
    private const char WhitespaceCharacter = ' ';

    private const int DefaultIndentation = 4;

    extension(System.Text.StringBuilder sb)
    {
        public System.Text.StringBuilder Indent(int indent, char character = WhitespaceCharacter)
        {
            if (indent > 0)
            {
                return sb.Append(character, indent);
            }

            return sb;
        }

        public System.Text.StringBuilder IndentByLevel(int level, int indentPerLevel = DefaultIndentation, char character = WhitespaceCharacter)
        {
            return sb.Indent(level * indentPerLevel, character);
        }
    }
}
