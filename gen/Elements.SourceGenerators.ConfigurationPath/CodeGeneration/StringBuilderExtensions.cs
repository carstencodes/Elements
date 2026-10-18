// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.CodeGeneration;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Provides extension methods for indented code emission into a <see cref="System.Text.StringBuilder"/>.</summary>
internal static class StringBuilderExtensions
{
    /// <summary>The default whitespace character used for indentation.</summary>
    private const char WhitespaceCharacter = ' ';

    /// <summary>The default number of characters per indentation level.</summary>
    private const int DefaultIndentation = 4;

    extension(System.Text.StringBuilder sb)
    {
        /// <summary>Appends indentation whitespace to the builder.</summary>
        /// <param name="indent">The total character count to indent.</param>
        /// <param name="character">The whitespace character to use.</param>
        /// <returns>The builder instance for chaining.</returns>
        public System.Text.StringBuilder Indent(int indent, char character = WhitespaceCharacter)
        {
            if (indent > 0)
            {
                return sb.Append(character, indent);
            }

            return sb;
        }

        /// <summary>Appends indentation whitespace calculated from an indentation level.</summary>
        /// <param name="level">The indentation level.</param>
        /// <param name="indentPerLevel">The number of whitespace characters per level.</param>
        /// <param name="character">The whitespace character to use.</param>
        /// <returns>The builder instance for chaining.</returns>
        public System.Text.StringBuilder IndentByLevel(int level, int indentPerLevel = DefaultIndentation,
            char character = WhitespaceCharacter)
        {
            return sb.Indent(level * indentPerLevel, character);
        }
    }
}
// AI GENERATED END
