// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests.Data;

// AI GENERATED START - model: Copilot
internal static class Expectations
{
    public const string AttributeNamespace = "namespace Demo.Settings.Attributes;";

    public const string MarkerAttribute = "class ConfigurationObjectAttribute";

    public const string RootContext = "ConfigurationContextOfRoot";

    public const string ChildContext = "ConfigurationContextOfChild";

    public const string ListValueContext = "ConfigurationContextOfListValue";

    public const string ArrayValueContext = "ConfigurationContextOfArrayValue";

    public const string GenericBoxContext = "ConfigurationContextOfBox_T<T>";

    public const string GenericBoxConstraint = "where T : class, new()";

    public const string DefaultAttributeNamespace =
        "namespace HedgeCraft.Extensions.Configuration.UserSettings.Attributes;";

    public const string ContextBaseType =
        "internal sealed partial class ConfigurationContextOfChild";

    public const string GeneratedCodeAttribute = "global::System.CodeDom.Compiler.GeneratedCode";

    public const string DefinitionList = "<dl>";

    public const string ReadOnlyPropertyTerm = $"<dt>{nameof(TestRootModel.ReadOnlyName)}</dt>";

    public const string ChildContextReference = "<see cref=";

    public const string BoundTypeExtensionsClass = "internal static class BoundTypeContextExtensions";

    public const string GetConfigurationContextMethod = "GetConfigurationContext(this global::Demo.Settings.Root instance)";

    public const string ConfigurationContextMethod = "ConfigurationContext(this global::Demo.Settings.Root instance)";

    public const string LeafDescriptorMethod = "GetBindableConfigurationLeafDescriptors";

    public const string LeafDescriptorCoreCall = LeafDescriptorMethod + "Core(visited)";

    public const string NestedLeafDescriptorHelper = "AppendNestedLeafDescriptors(";

    public const string LazyCachedField = "private static readonly global::System.Lazy<";
}
// AI GENERATED END
