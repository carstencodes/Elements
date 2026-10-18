// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests.Data;
using DisplayNameAttribute = TUnit.Core.DisplayNameAttribute;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests;

// AI GENERATED START - model: Claude Sonnet 5.5
/// <summary>
/// Validates the generated <c>GetBindableConfigurationLeafDescriptors</c> implementation of configuration contexts.
/// </summary>
[DisplayName("Configuration Leaf Descriptor Generator Tests")]
[Description("Validates leaf selection, types, descriptions, nesting and cycle handling of generated contexts.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Performance",
    "CA1822:Mark members as static",
    Justification = "TUnit test methods must be instance methods.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Major Code Smell",
    "S2325:Methods and properties that don't access instance data should be static",
    Justification = "TUnit test methods must be instance methods.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Major Code Smell",
    "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields",
    Justification = "Test suite invokes generated internal context types.")]
internal sealed class ConfigurationLeafDescriptorGeneratorTests
{
    private const string LeafMethod = Expectations.LeafDescriptorMethod;

    private static readonly Lazy<ConfigurationPathGeneratorTests.GeneratorRun> ModelRun = new(static () =>
        ConfigurationPathGeneratorTests.RunGenerator(LeafDescriptorInput.ModelScenario, "Leaf.Settings.Attributes"));

    private static readonly Lazy<ConfigurationPathGeneratorTests.GeneratorRun> UserRun = new(static () =>
        ConfigurationPathGeneratorTests.RunGenerator(LeafDescriptorInput.UserContextScenario, "Leaf.User.Attributes"));

    [Test]
    [DisplayName("Generated sources compile and implement the leaf descriptor method")]
    [Description("Verifies that all generated contexts compile and that the root context emits the leaf method.")]
    public async Task GeneratedContextsCompileAndEmitLeafMethod()
    {
        ConfigurationPathGeneratorTests.GeneratorRun run = ModelRun.Value;

        using (Assert.Multiple())
        {
            await Assert.That(run.CompilationErrors)
                .IsEqualTo(string.Empty)
                .Because("generated contexts should compile against the abstract leaf descriptor method");

            await Assert.That(run.GeneratedSources.Any(static text =>
                    text.Contains($"public override global::System.Collections.Generic.IReadOnlySet<" +
                                  "global::HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys." +
                                  $"ConfigurationLeafDescriptor> {LeafMethod}()", StringComparison.Ordinal)))
                .IsTrue()
                .Because("a generated context should override the leaf descriptor method");
        }
    }

    [Test]
    [DisplayName("Generated parent contexts delegate to nested leaf descriptor methods")]
    [Description("Verifies that generated sibling contexts call the cycle-guarded core method with a prefix.")]
    public async Task GeneratedParentContextsDelegateToNestedLeafMethods()
    {
        string rootSource = ModelRun.Value.GeneratedSources.First(static text =>
            text.Contains("class ConfigurationContextOfRoot", StringComparison.Ordinal));

        using (Assert.Multiple())
        {
            await Assert.That(rootSource.Contains(Expectations.NestedLeafDescriptorHelper, StringComparison.Ordinal))
                .IsTrue()
                .Because("the root context should merge nested leaf descriptors through the base helper");

            await Assert.That(rootSource.Contains(Expectations.LeafDescriptorCoreCall, StringComparison.Ordinal))
                .IsTrue()
                .Because("generated sibling contexts should share the visited set to terminate cycles");
        }
    }

    [Test]
    [DisplayName("Simple properties become leaves with their property type")]
    [Description("Verifies path and type of plain string, integer and nullable integer leaves.")]
    public async Task SimplePropertiesBecomeLeaves()
    {
        Dictionary<string, LeafInfo> leaves = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot");

        using (Assert.Multiple())
        {
            await Assert.That(leaves["Title"].Type)
                .IsEqualTo(typeof(string))
                .Because("a string property should be a leaf typed as string");

            await Assert.That(leaves["Port"].Type)
                .IsEqualTo(typeof(int))
                .Because("an integer property should be a leaf typed as int");
        }
    }

    [Test]
    [DisplayName("Nullable value types are unwrapped to their underlying type")]
    [Description("Verifies that a nullable integer property is described as int rather than Nullable of int.")]
    public async Task NullableValueTypesAreUnwrapped()
    {
        Dictionary<string, LeafInfo> leaves = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot");

        await Assert.That(leaves["Retries"].Type)
            .IsEqualTo(typeof(int))
            .Because("Nullable<int> should be unwrapped to int");
    }

    [Test]
    [DisplayName("Enum properties become leaves typed as the enum")]
    [Description("Verifies that an enum property is a leaf whose type is the enum type.")]
    public async Task EnumPropertiesBecomeLeaves()
    {
        Dictionary<string, LeafInfo> leaves = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot");

        await Assert.That(leaves["Mode"].Type.FullName)
            .IsEqualTo("Leaf.Settings.Mode")
            .Because("an enum property should be a leaf typed as the enum");
    }

    [Test]
    [DisplayName("Collections of simple values are leaves typed as the property type")]
    [Description("Verifies list, array and dictionary of simple values as leaves with the collection type.")]
    public async Task SimpleCollectionsBecomeLeaves()
    {
        Dictionary<string, LeafInfo> leaves = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot");

        using (Assert.Multiple())
        {
            await Assert.That(leaves["Tags"].Type)
                .IsEqualTo(typeof(List<string>))
                .Because("a list of strings should be a leaf typed as the list");

            await Assert.That(leaves["Scores"].Type)
                .IsEqualTo(typeof(int[]))
                .Because("an array of integers should be a leaf typed as the array");

            await Assert.That(leaves["Blob"].Type)
                .IsEqualTo(typeof(byte[]))
                .Because("a byte array should be a leaf typed as the array");

            await Assert.That(leaves["Maybe"].Type)
                .IsEqualTo(typeof(List<int?>))
                .Because("a list of nullable integers should be a leaf typed as the list");

            await Assert.That(leaves["Limits"].Type)
                .IsEqualTo(typeof(Dictionary<string, int>))
                .Because("a dictionary of simple values should be a leaf typed as the dictionary");
        }
    }

    [Test]
    [DisplayName("Collections of models and their elements emit no leaves")]
    [Description("Verifies that neither the model collection node nor element paths appear as leaves.")]
    public async Task CollectionsOfModelsEmitNoLeaves()
    {
        Dictionary<string, LeafInfo> leaves = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot");

        await Assert.That(leaves.Keys.Any(static path => path.StartsWith("Children", StringComparison.Ordinal) || path.StartsWith("ChildArray", StringComparison.Ordinal) || path.StartsWith("ChildMap", StringComparison.Ordinal)))
            .IsFalse()
            .Because("a collection of models has no compile-time index and must not produce leaves");
    }

    [Test]
    [DisplayName("Nested models contribute prefixed leaves but no object node")]
    [Description("Verifies renamed nested paths, deep nesting and absence of the object node paths.")]
    public async Task NestedModelsContributePrefixedLeaves()
    {
        HashSet<string> paths = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot")
            .Keys.ToHashSet(StringComparer.Ordinal);

        using (Assert.Multiple())
        {
            await Assert.That(paths.Contains("renamed:Name"))
                .IsTrue()
                .Because("a nested leaf should be prefixed with the configured key name");

            await Assert.That(paths.Contains("renamed:Grand:Flag"))
                .IsTrue()
                .Because("leaves of deeper nested models should carry the full prefix");

            await Assert.That(paths.Contains("renamed") || paths.Contains("renamed:Grand"))
                .IsFalse()
                .Because("object nodes accept no primitive value and must be omitted");
        }
    }

    [Test]
    [DisplayName("Closed generic models are expanded")]
    [Description("Verifies that leaves of a closed generic container are found below the container property.")]
    public async Task ClosedGenericModelsAreExpanded()
    {
        Dictionary<string, LeafInfo> leaves = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot");

        await Assert.That(leaves.ContainsKey("Box:Value:Name"))
            .IsTrue()
            .Because("the closed generic Box<Child> should expand its Value child leaves");
    }

    [Test]
    [DisplayName("Self-referencing models terminate")]
    [Description("Verifies that a cyclic model returns its leaves instead of recursing endlessly.")]
    public async Task SelfReferencingModelsTerminate()
    {
        Dictionary<string, LeafInfo> leaves = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot");

        await Assert.That(leaves.ContainsKey("Node:Label"))
            .IsTrue()
            .Because("a cyclic model should still contribute its leaves once and terminate");
    }

    [Test]
    [DisplayName("Leaf descriptions use the normalized XML summary or the property name")]
    [Description("Verifies the documented summary is whitespace-normalized and undocumented properties use their name.")]
    public async Task DescriptionsUseSummaryOrPropertyName()
    {
        Dictionary<string, LeafInfo> leaves = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot");

        using (Assert.Multiple())
        {
            await Assert.That(leaves["Title"].Description)
                .IsEqualTo("The display name.")
                .Because("the XML summary should be used with whitespace collapsed");

            await Assert.That(leaves["EmptyDoc"].Description)
                .IsEqualTo("EmptyDoc")
                .Because("an empty XML summary should fall back to the property name");

            await Assert.That(leaves["Port"].Description)
                .IsEqualTo("Port")
                .Because("an undocumented property should fall back to its name");

            await Assert.That(leaves.Values.All(static leaf => !string.IsNullOrWhiteSpace(leaf.Description)))
                .IsTrue()
                .Because("a description must never be empty");
        }
    }

    [Test]
    [DisplayName("Leaves are a subset of readable paths and exclude object nodes")]
    [Description("Parity check between the leaf descriptors and the readable configuration paths of the root context.")]
    public async Task LeavesAreReadablePathsWithoutObjectNodes()
    {
        Type contextType = ModelRun.Value.Load().GetType("Leaf.Settings.ConfigurationContextOfRoot")!;
        object context = Activator.CreateInstance(contextType, nonPublic: true)!;
        HashSet<string> readable = ReadPaths(contextType, context, "GetReadableConfigurationPaths");
        HashSet<string> leaves = GetLeaves(ModelRun.Value, "Leaf.Settings.ConfigurationContextOfRoot")
            .Keys.ToHashSet(StringComparer.Ordinal);
        string[] objectNodes = ["renamed", "renamed:Grand", "Node", "Box", "Box:Value", "Box:Value:Grand", "Children", "ChildArray", "ChildMap"];

        using (Assert.Multiple())
        {
            await Assert.That(leaves.IsSubsetOf(readable))
                .IsTrue()
                .Because("every leaf path should also be a readable configuration path");

            await Assert.That(objectNodes.Any(leaves.Contains))
                .IsFalse()
                .Because("no object node path should be reported as a leaf");
        }
    }

    [Test]
    [DisplayName("User-defined nested contexts are merged with the parent prefix")]
    [Description("Verifies that leaves of a user-provided nested context are prefixed via its public method.")]
    public async Task UserDefinedNestedContextLeavesAreMerged()
    {
        ConfigurationPathGeneratorTests.GeneratorRun run = UserRun.Value;
        Dictionary<string, LeafInfo> leaves = GetLeaves(run, "Leaf.User.ConfigurationContextOfRoot");

        using (Assert.Multiple())
        {
            await Assert.That(run.CompilationErrors)
                .IsEqualTo(string.Empty)
                .Because("the user context scenario should compile");

            await Assert.That(leaves.Keys)
                .IsEquivalentTo(["Child:Custom"])
                .Because("only the user context leaf should appear, prefixed with the parent key");

            await Assert.That(leaves["Child:Custom"].Description)
                .IsEqualTo("custom leaf")
                .Because("the user context description should be preserved");
        }
    }

    private static Dictionary<string, LeafInfo> GetLeaves(
        ConfigurationPathGeneratorTests.GeneratorRun run,
        string contextTypeName)
    {
        Type contextType = run.Load().GetType(contextTypeName)!;
        object context = Activator.CreateInstance(contextType, nonPublic: true)!;
        object set = contextType.GetMethod(LeafMethod)!.Invoke(context, null)!;

        return ((IEnumerable)set).Cast<object>().ToDictionary(
            static item => string.Join(':', (string[])GetMember(item, "Path").GetType()
                .GetProperty("Segments")!.GetValue(GetMember(item, "Path"))!),
            static item => new LeafInfo(
                (Type)GetMember(item, "ConfigurationValueType"),
                (string)GetMember(item, "Description")),
            StringComparer.Ordinal);
    }

    private static HashSet<string> ReadPaths(Type contextType, object context, string methodName)
    {
        object set = contextType.GetMethod(methodName)!.Invoke(context, null)!;

        return ((IEnumerable)set).Cast<object>()
            .Select(static item => string.Join(':', (string[])item.GetType().GetProperty("Segments")!.GetValue(item)!))
            .ToHashSet(StringComparer.Ordinal);
    }

    private static object GetMember(object instance, string propertyName)
    {
        return instance.GetType().GetProperty(propertyName)!.GetValue(instance)!;
    }

    private sealed record LeafInfo(Type Type, string Description);
}

/// <summary>Loads the dynamic assembly emitted by a generator run.</summary>
internal static class GeneratorRunExtensions
{
    /// <summary>Loads the emitted assembly image of the run.</summary>
    /// <param name="run">The generator run.</param>
    /// <returns>The loaded assembly.</returns>
    public static System.Reflection.Assembly Load(this ConfigurationPathGeneratorTests.GeneratorRun run)
    {
        return System.Reflection.Assembly.Load(run.AssemblyImage);
    }
}
// AI GENERATED END
