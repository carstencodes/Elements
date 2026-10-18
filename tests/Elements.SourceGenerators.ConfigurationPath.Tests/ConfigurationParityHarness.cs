// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests;

// AI GENERATED START - model: Claude Sonnet 5.5
/// <summary>
/// Compiles a model source with the generator, binds an in-memory configuration with
/// <see cref="ConfigurationBinder"/> and compares the generated dump with the original configuration.
/// </summary>
internal static class ConfigurationParityHarness
{
    private const string DumpMethodName = "DumpConfigurationObject";

    /// <summary>
    /// Runs the generator, binds <paramref name="configuration"/> and compares the generated dump to it.
    /// </summary>
    /// <param name="source">Complete model source, including the support types.</param>
    /// <param name="rootNamespace">Root namespace used for generated attributes.</param>
    /// <param name="rootTypeName">Full name of the root model type inside the compiled source.</param>
    /// <param name="configuration">Flat configuration keys and values to bind.</param>
    /// <returns>The comparison outcome.</returns>
    public static ParityOutcome Execute(
        string source,
        string rootNamespace,
        string rootTypeName,
        IReadOnlyDictionary<string, string?> configuration)
    {
        ConfigurationPathGeneratorTests.GeneratorRun run =
            ConfigurationPathGeneratorTests.RunGenerator(source, rootNamespace);
        if (run.CompilationErrors.Length > 0)
        {
            return ParityOutcome.Failed(run.CompilationErrors);
        }

        System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(run.AssemblyImage);
        Type rootType = assembly.GetType(rootTypeName, throwOnError: true)!;
        object root = Activator.CreateInstance(rootType)!;
        Bind(configuration, root);

        IReadOnlyDictionary<string, object?> dump = Dump(assembly, rootType, root);
        return Compare(configuration, dump, WritablePaths(assembly, rootType));
    }

    private static void Bind(IReadOnlyDictionary<string, string?> configuration, object root)
    {
        IConfigurationRoot built = new ConfigurationBuilder()
            .AddInMemoryCollection(configuration)
            .Build();
        built.Bind(root);
    }

    private static (Type Type, object Instance) CreateContext(System.Reflection.Assembly assembly, Type rootType)
    {
        string contextName = string.Concat(rootType.Namespace, ".ConfigurationContextOf", rootType.Name);
        Type contextType = assembly.GetType(contextName, throwOnError: true)!;
        return (contextType, Activator.CreateInstance(contextType, nonPublic: true)!);
    }

    private static HashSet<string> WritablePaths(System.Reflection.Assembly assembly, Type rootType)
    {
        (Type contextType, object context) = CreateContext(assembly, rootType);
        object paths = contextType.GetMethod("GetWritableConfigurationPaths")!.Invoke(context, null)!;
        return ((IEnumerable)paths)
            .Cast<object>()
            .Select(static item => (string[])item.GetType().GetProperty("Segments")!.GetValue(item)!)
            .Select(static segments => string.Join(':', segments))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static IReadOnlyDictionary<string, object?> Dump(System.Reflection.Assembly assembly, Type rootType, object root)
    {
        (Type contextType, object context) = CreateContext(assembly, rootType);
        object?[] arguments = [root, true, Array.Empty<string>()];
        return (IReadOnlyDictionary<string, object?>)contextType
            .GetMethod(DumpMethodName)!
            .Invoke(context, arguments)!;
    }

    private static ParityOutcome Compare(
        IReadOnlyDictionary<string, string?> expected,
        IReadOnlyDictionary<string, object?> dump,
        HashSet<string> writablePaths)
    {
        Dictionary<string, string?> actual = new(StringComparer.OrdinalIgnoreCase);
        foreach ((string key, object? value) in dump.Where(static entry => entry.Value is not null))
        {
            actual[key] = ToText(value!);
        }

        List<string> missing = expected.Keys.Where(key => !actual.ContainsKey(key)).ToList();
        List<string> extra = actual.Keys
            .Where(key => !expected.Keys.Contains(key, StringComparer.OrdinalIgnoreCase))
            .ToList();
        List<string> mismatched = expected
            .Where(entry => actual.TryGetValue(entry.Key, out string? value) &&
                            !string.Equals(value, entry.Value, StringComparison.OrdinalIgnoreCase))
            .Select(entry => $"{entry.Key}: expected '{entry.Value}' but dump has '{actual[entry.Key]}'")
            .ToList();

        List<string> uncovered = expected.Keys.Where(key => !writablePaths.Contains(key)).ToList();
        return new ParityOutcome(string.Empty, missing, extra, mismatched, uncovered);
    }

    private static string ToText(object value)
    {
        return value switch
        {
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            IEnumerable enumerable and not string => string.Join(",", enumerable.Cast<object>()),
            _ => value.ToString() ?? string.Empty,
        };
    }
}

// AI GENERATED END
