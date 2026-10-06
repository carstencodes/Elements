// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HedgeCraft.Elements.SourceGenerators.ConfigurationPath;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using TUnit;
using TUnit.Assertions;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests;

// AI GENERATED START - model: Copilot
internal sealed class ConfigurationPathGeneratorTests
{
    private static readonly ImmutableArray<MetadataReference> PlatformReferences =
        ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
        .Split(Path.PathSeparator)
        .Select(static path => (MetadataReference)MetadataReference.CreateFromFile(path))
        .ToImmutableArray();

    private const string NestedModelsSource = """
        namespace Demo.Settings
        {
            using Demo.Settings.Attributes;
            using Microsoft.Extensions.Configuration;
            using System.Collections.Generic;

            [ConfigurationObject]
            public class Root
            {
                [ConfigurationKeyName("renamed")]
                public Child Child { get; } = new();

                public Dictionary<KeyModel, Child> Values { get; } = new();

                public List<Child> Children { get; } = new();

                public int[] Scores { get; } = [3, 4];

                public byte[] Blob { get; } = [5, 6];

                public Box<Child> Box { get; } = new();

                public List<ListValue> Items { get; } = new();

                public ArrayValue[] ArrayValues { get; } = [];
            }

            public class Child
            {
                public string Name { get; set; } = string.Empty;

                public Root? Parent { get; set; }
            }

            public class ListValue
            {
            }

            public class ArrayValue
            {
            }

            public class KeyModel
            {
                public string Name { get; set; } = string.Empty;
            }

            public class Box<T> where T : class, new()
            {
                public T Value { get; } = new();
            }
        }

        namespace Microsoft.Extensions.Configuration
        {
            using System;

            [AttributeUsage(AttributeTargets.Property)]
            public sealed class ConfigurationKeyNameAttribute(string name) : Attribute
            {
                public string Name { get; } = name;
            }
        }

        namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts
        {
            using System.Collections.Generic;

            public abstract class ConfigurationObjectContextBase<T>
            {
                public abstract IReadOnlySet<string[]> GetReadableConfigurationPaths();

                public abstract IReadOnlySet<string[]> GetWritableConfigurationPaths();

                public abstract IReadOnlyDictionary<string, object> DumpConfigurationObject(
                    T instance,
                    params string[] parentKeys);
            }
        }
        """;

    private const string FallbackNamespaceSource = """
        namespace Test.Models
        {
            using HedgeCraft.Extensions.Configuration.UserSettings.Attributes;

            [ConfigurationObject]
            public struct Settings
            {
                public string Name { get; }
            }
        }

        namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts
        {
            using System.Collections.Generic;

            public abstract class ConfigurationObjectContextBase<T>
            {
                public abstract IReadOnlySet<string[]> GetReadableConfigurationPaths();

                public abstract IReadOnlySet<string[]> GetWritableConfigurationPaths();

                public abstract IReadOnlyDictionary<string, object> DumpConfigurationObject(
                    T instance,
                    params string[] parentKeys);
            }
        }
        """;

    private const string ExistingContextSource = """
        namespace Existing.Context
        {
            using System.Collections.Generic;
            using Demo.Context.Attributes;

            [ConfigurationObject]
            public class Root
            {
                public Child Child { get; } = new();
            }

            public class Child
            {
                public string Name { get; set; } = string.Empty;
            }

            internal sealed class ExistingChildContext :
                HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts.ConfigurationObjectContextBase<Child>
            {
                public override IReadOnlySet<string[]> GetReadableConfigurationPaths() => new HashSet<string[]>();

                public override IReadOnlySet<string[]> GetWritableConfigurationPaths() => new HashSet<string[]>();

                public override IReadOnlyDictionary<string, object> DumpConfigurationObject(
                    Child instance,
                    params string[] parentKeys) => new Dictionary<string, object>();
            }
        }

        namespace Demo.Context.Attributes
        {
        }

        namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts
        {
            using System.Collections.Generic;

            public abstract class ConfigurationObjectContextBase<T>
            {
                public abstract IReadOnlySet<string[]> GetReadableConfigurationPaths();

                public abstract IReadOnlySet<string[]> GetWritableConfigurationPaths();

                public abstract IReadOnlyDictionary<string, object> DumpConfigurationObject(
                    T instance,
                    params string[] parentKeys);
            }
        }
        """;

    [Test]
    public async Task EmitsAttributeInConfiguredRootNamespaceAndGeneratesNestedContexts()
    {
        GeneratorRun run = RunGenerator(NestedModelsSource, "Demo.Settings.Attributes");
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("namespace Demo.Settings.Attributes;", StringComparison.Ordinal) &&
            text.Contains("class ConfigurationObjectAttribute", StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("ConfigurationContextOfRoot", StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("ConfigurationContextOfChild", StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("ConfigurationContextOfListValue", StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("ConfigurationContextOfArrayValue", StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("ConfigurationContextOfBox_T<T>", StringComparison.Ordinal) &&
            text.Contains("where T : class, new()", StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("ConfigurationContextOfKeyModel", StringComparison.Ordinal))).IsFalse();
        await Assert.That(run.CompilationErrors).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task EmitsFallbackAttributeNamespaceWhenRootNamespaceIsUnavailable()
    {
        GeneratorRun run = RunGenerator(FallbackNamespaceSource, null);
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains(
                "namespace HedgeCraft.Extensions.Configuration.UserSettings.Attributes;",
                StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("ConfigurationContextOfSettings", StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.CompilationErrors).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task ReusesAnExistingContextImplementationForNestedModels()
    {
        GeneratorRun run = RunGenerator(ExistingContextSource, "Demo.Context.Attributes");
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("ConfigurationContextOfRoot", StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains(
                "internal sealed partial class ConfigurationContextOfChild",
                StringComparison.Ordinal))).IsFalse();
        await Assert.That(run.GeneratedSources.Any(static text =>
            text.Contains("DumpConfigurationObject(", StringComparison.Ordinal) &&
            text.Contains("global::Existing.Context.ExistingChildContext", StringComparison.Ordinal))).IsTrue();
        await Assert.That(run.CompilationErrors).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task DumpsNestedObjectsAndCollectionsWithParentKeys()
    {
        GeneratorRun run = RunGenerator(NestedModelsSource, "Demo.Settings.Attributes");
        System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(run.AssemblyImage);
        Type rootType = assembly.GetType("Demo.Settings.Root")!;
        Type contextType = assembly.GetType("Demo.Settings.ConfigurationContextOfRoot")!;
        object root = Activator.CreateInstance(rootType)!;
        object child = rootType.GetProperty("Child")!.GetValue(root)!;
        child.GetType().GetProperty("Name")!.SetValue(child, "root child");
        child.GetType().GetProperty("Parent")!.SetValue(child, root);

        object box = rootType.GetProperty("Box")!.GetValue(root)!;
        object boxChild = box.GetType().GetProperty("Value")!.GetValue(box)!;
        boxChild.GetType().GetProperty("Name")!.SetValue(boxChild, "box child");

        ((System.Collections.IList)rootType.GetProperty("Children")!.GetValue(root)!).Add(child);

        object dictionaryChild = Activator.CreateInstance(assembly.GetType("Demo.Settings.Child")!)!;
        dictionaryChild.GetType().GetProperty("Name")!.SetValue(dictionaryChild, "dictionary child");
        object key = Activator.CreateInstance(assembly.GetType("Demo.Settings.KeyModel")!)!;
        ((System.Collections.IDictionary)rootType.GetProperty("Values")!.GetValue(root)!).Add(key, dictionaryChild);

        object context = Activator.CreateInstance(contextType, nonPublic: true)!;
        object?[] arguments = [root, new[] { "Parent" }];
        IReadOnlyDictionary<string, object> values =
            (IReadOnlyDictionary<string, object>)contextType.GetMethod("DumpConfigurationObject")!
                .Invoke(context, arguments)!;

        await Assert.That(values["Parent:renamed:Name"]).IsEqualTo("root child");
        await Assert.That(values["Parent:Children:0:Name"]).IsEqualTo("root child");
        await Assert.That(values["Parent:Box:Value:Name"]).IsEqualTo("box child");
        await Assert.That(values["Parent:Scores:0"]).IsEqualTo(3);
        await Assert.That(values["Parent:Scores:1"]).IsEqualTo(4);
        await Assert.That(((byte[])values["Parent:Blob"]).SequenceEqual(new byte[] { 5, 6 })).IsTrue();
        await Assert.That(values.ContainsKey("Parent:Blob:0")).IsFalse();
        string dictionaryValue = values.First(entry =>
            entry.Key.StartsWith("Parent:Values:", StringComparison.Ordinal) &&
            entry.Key.EndsWith(":Name", StringComparison.Ordinal)).Value.ToString()!;
        await Assert.That(dictionaryValue).IsEqualTo("dictionary child");

        IReadOnlySet<string[]> readable =
            (IReadOnlySet<string[]>)contextType.GetMethod("GetReadableConfigurationPaths")!.Invoke(context, null)!;
        IReadOnlySet<string[]> writable =
            (IReadOnlySet<string[]>)contextType.GetMethod("GetWritableConfigurationPaths")!.Invoke(context, null)!;
        await Assert.That(readable.Contains(["renamed", "Name"])).IsTrue();
        await Assert.That(readable.Contains(["Box", "Value", "Name"])).IsTrue();
        await Assert.That(writable.Contains(["renamed", "Name"])).IsTrue();
        await Assert.That(writable.Contains(["renamed"])).IsFalse();
    }

    private static GeneratorRun RunGenerator(string source, string? rootNamespace)
    {
        CSharpParseOptions parseOptions = new(LanguageVersion.CSharp14);
        CSharpCompilation compilation = CSharpCompilation.Create(
            string.Concat("GeneratorTests_", Guid.NewGuid().ToString("N")),
            [CSharpSyntaxTree.ParseText(SourceText.From(source), parseOptions)],
            PlatformReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new ConfigurationPathGenerator().AsSourceGenerator()],
            parseOptions: parseOptions,
            optionsProvider: new TestAnalyzerConfigOptionsProvider(rootNamespace));
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out Compilation updatedCompilation, out _);
        ImmutableArray<string> generatedSources = driver.GetRunResult()
            .Results
            .SelectMany(static result => result.GeneratedSources)
            .Select(static generated => generated.SourceText.ToString())
            .ToImmutableArray();

        string compilationErrors = string.Join(
            Environment.NewLine,
            updatedCompilation.GetDiagnostics()
                .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                .Select(static diagnostic => string.Concat(
                    diagnostic.Id,
                    ": ",
                    diagnostic.GetMessage(CultureInfo.InvariantCulture))));
        using MemoryStream output = new();
        Microsoft.CodeAnalysis.Emit.EmitResult emitResult = updatedCompilation.Emit(output);
        if (!emitResult.Success)
        {
            compilationErrors = string.Join(
                Environment.NewLine,
                emitResult.Diagnostics
                    .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                    .Select(static diagnostic => string.Concat(
                        diagnostic.Id,
                        ": ",
                        diagnostic.GetMessage(CultureInfo.InvariantCulture))));
        }

        return new GeneratorRun(generatedSources, compilationErrors, emitResult.Success ? output.ToArray() : []);
    }

    private sealed record GeneratorRun(
        ImmutableArray<string> GeneratedSources,
        string CompilationErrors,
        byte[] AssemblyImage);

    private sealed class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
    {
        public TestAnalyzerConfigOptionsProvider(string? rootNamespace)
        {
            Dictionary<string, string> options = new(StringComparer.OrdinalIgnoreCase);
            if (rootNamespace is not null)
            {
                options["build_property.RootNamespace"] = rootNamespace;
            }

            this.GlobalOptions = new TestAnalyzerConfigOptions(options);
        }

        public override AnalyzerConfigOptions GlobalOptions { get; }

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
        {
            return TestAnalyzerConfigOptions.Empty;
        }

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
        {
            return TestAnalyzerConfigOptions.Empty;
        }
    }

    private sealed class TestAnalyzerConfigOptions(IDictionary<string, string> options) : AnalyzerConfigOptions
    {
        public static TestAnalyzerConfigOptions Empty { get; } =
            new(new Dictionary<string, string>(StringComparer.Ordinal));

        public override bool TryGetValue(string key, out string value)
        {
            return options.TryGetValue(key, out value!);
        }
    }
}
// AI GENERATED END
