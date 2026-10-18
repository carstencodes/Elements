// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using Microsoft.CodeAnalysis;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Provides diagnostic descriptors for configuration path generation issues.</summary>
public static class DiagnosticDescriptors
{
    /// <summary>Reported when multiple properties in the same model resolve to identical configuration keys.</summary>
    public static readonly DiagnosticDescriptor DuplicateConfigurationKey = new(
        id: "HC_CP001",
        title: "Duplicate configuration key name",
        messageFormat: "Property '{0}' defines duplicate configuration key '{1}' in type '{2}'",
        category: "ConfigurationPathGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Configuration keys within the same type must be unique to avoid ambiguous binding.");

    /// <summary>Reported when a dictionary uses an unsupported non-string-convertible key type.</summary>
    public static readonly DiagnosticDescriptor UnsupportedDictionaryKey = new(
        id: "HC_CP002",
        title: "Unsupported dictionary key type",
        messageFormat: "Dictionary key type '{0}' in property '{1}' is unsupported for configuration binding",
        category: "ConfigurationPathGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Configuration dictionaries should use string-compatible key types.");

    /// <summary>Reported when an extension member name collides with an existing member on the target model type.</summary>
    public static readonly DiagnosticDescriptor ExtensionMemberCollision = new(
        id: "HC_CP003",
        title: "Extension member collision",
        messageFormat: "Extension member '{0}' collides with an existing member on model type '{1}' and will not be generated",
        category: "ConfigurationPathGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Model types should not define members that collide with generated context extension members.");

    /// <summary>Reported when a model type contains unbound generic type parameters preventing extension generation.</summary>
    public static readonly DiagnosticDescriptor UnboundGenericModel = new(
        id: "HC_CP004",
        title: "Unbound generic model type parameter",
        messageFormat: "Model type '{0}' has unbound generic type parameters and context extensions will not be generated",
        category: "ConfigurationPathGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Extension members can only be generated for fully bound configuration model types.");
}
// AI GENERATED END
