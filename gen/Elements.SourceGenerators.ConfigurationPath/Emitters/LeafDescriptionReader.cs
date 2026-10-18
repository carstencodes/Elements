// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Emitters;

// AI GENERATED START - model: Claude Sonnet 5.5
/// <summary>Derives single-line leaf descriptions from property XML documentation.</summary>
internal static class LeafDescriptionReader
{
    /// <summary>Gets the normalized XML summary of a property, or its name when no summary is available.</summary>
    /// <param name="property">The property that backs the configuration leaf.</param>
    /// <returns>A non-empty, whitespace-normalized description.</returns>
    internal static string GetDescription(IPropertySymbol property)
    {
        string summary = ReadSummary(property);
        return summary.Length > 0 ? summary : property.Name;
    }

    /// <summary>Reads the summary element from the property's documentation comment.</summary>
    /// <param name="property">The documented property.</param>
    /// <returns>The normalized summary text, or an empty string when absent or malformed.</returns>
    private static string ReadSummary(IPropertySymbol property)
    {
        try
        {
            string xml = property.GetDocumentationCommentXml(expandIncludes: false) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(xml))
            {
                return string.Empty;
            }

            XElement? summary = XDocument.Parse(xml).Root?.Element("summary");
            return summary is null ? string.Empty : Normalize(RenderNodes(summary));
        }
        catch (System.Xml.XmlException)
        {
            // Malformed documentation must never break generation; the property name is the fallback.
            return string.Empty;
        }
    }

    /// <summary>Flattens summary content, keeping the referenced name of inline elements such as see tags.</summary>
    /// <param name="element">The element whose child nodes are rendered.</param>
    /// <returns>The concatenated text.</returns>
    private static string RenderNodes(XElement element)
    {
        StringBuilder text = new();
        foreach (XNode node in element.Nodes())
        {
            switch (node)
            {
                case XText value:
                    text.Append(value.Value);
                    break;
                case XElement child:
                    text.Append(RenderInline(child));
                    break;
            }
        }

        return text.ToString();
    }

    /// <summary>Renders an inline element using its text or its cref, langword, or name reference.</summary>
    /// <param name="child">The inline element.</param>
    /// <returns>The text representing the element.</returns>
    private static string RenderInline(XElement child)
    {
        string content = RenderNodes(child);
        if (content.Trim().Length > 0)
        {
            return content;
        }

        string reference = (string?)child.Attribute("cref") ??
                           (string?)child.Attribute("langword") ??
                           (string?)child.Attribute("name") ??
                           string.Empty;
        int separator = reference.IndexOf(':');
        return separator >= 0 ? reference.Substring(separator + 1) : reference;
    }

    /// <summary>Collapses whitespace runs and trims the text.</summary>
    /// <param name="text">The raw text.</param>
    /// <returns>A single-line string.</returns>
    private static string Normalize(string text)
    {
        return string.Join(" ", text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
    }
}
// AI GENERATED END
