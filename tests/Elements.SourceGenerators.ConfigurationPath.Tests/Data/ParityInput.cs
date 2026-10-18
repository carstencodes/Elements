// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests.Data;

// AI GENERATED START - model: Claude Sonnet 5.5
/// <summary>
/// Model sources used by the binder parity tests. Each model source is combined with <see cref="Support"/>.
/// </summary>
internal static class ParityInput
{
    /// <summary>
    /// Root namespace used for the generated marker attributes and contexts of all parity sources.
    /// </summary>
    public const string AttributeNamespace = "Parity.Settings.Attributes";

    /// <summary>
    /// Smallest model proving the harness: a renamed nested object and nullable scalars.
    /// </summary>
    public const string Smoke = """
        namespace Parity.Settings
        {
            using Parity.Settings.Attributes;
            using Microsoft.Extensions.Configuration;

            [ConfigurationObject]
            public class SmokeRoot
            {
                [ConfigurationKeyName("renamed")]
                public SmokeChild Child { get; set; } = new();

                public string? Title { get; set; }

                public int? Count { get; set; }
            }

            public class SmokeChild
            {
                public string? Name { get; set; }
            }
        }

        """ + "\n" + Support;

    /// <summary>
    /// Five levels of nested objects with key name overrides, a get-only nested object and a cycle.
    /// </summary>
    public const string DeepNesting = """
        namespace Parity.Settings
        {
            using Parity.Settings.Attributes;
            using Microsoft.Extensions.Configuration;

            [ConfigurationObject]
            public class DeepRoot
            {
                public string? Title { get; set; }

                [ConfigurationKeyName("l1")]
                public DeepLevel1 First { get; set; } = new();

                public DeepLevel1 ReadOnlyFirst { get; } = new();

                public DeepLevel1? OptionalFirst { get; set; }
            }

            public class DeepLevel1
            {
                public string? Name { get; set; }

                public DeepLevel2 Second { get; set; } = new();

                public DeepRoot? Parent { get; set; }
            }

            public class DeepLevel2
            {
                public int? Number { get; set; }

                [ConfigurationKeyName("third")]
                public DeepLevel3 Third { get; set; } = new();
            }

            public class DeepLevel3
            {
                public bool? Flag { get; set; }

                public DeepLevel4 Fourth { get; set; } = new();
            }

            public class DeepLevel4
            {
                public double? Ratio { get; set; }

                public DeepLevel5 Fifth { get; } = new();
            }

            public class DeepLevel5
            {
                public string? Name { get; set; }

                public System.Guid? Id { get; set; }
            }
        }

        """ + "\n" + Support;

    /// <summary>
    /// One collection property per collection shape the binder supports.
    /// </summary>
    public const string CollectionTypes = """
        namespace Parity.Settings
        {
            using System;
            using System.Collections.Generic;
            using System.Collections.Immutable;
            using Parity.Settings.Attributes;
            using Microsoft.Extensions.Configuration;

            public enum ParityColor
            {
                Red,
                Green,
                Blue,
            }

            public class CollectionItem
            {
                public string? Name { get; set; }

                public int? Weight { get; set; }
            }

            [ConfigurationObject]
            public class CollectionRoot
            {
                public List<int>? IntList { get; set; }

                public int[]? IntArray { get; set; }

                public IList<string>? StringIList { get; set; }

                public ICollection<string>? StringICollection { get; set; }

                public IEnumerable<string>? StringEnumerable { get; set; }

                public IReadOnlyList<string>? StringReadOnlyList { get; set; }

                public IReadOnlyCollection<string>? StringReadOnlyCollection { get; set; }

                public ISet<string>? StringISet { get; set; }

                public HashSet<string>? StringHashSet { get; set; }

                public ImmutableArray<string> StringImmutableArray { get; set; }

                public List<List<int>>? JaggedList { get; set; }

                public int[][]? JaggedArray { get; set; }

                public List<CollectionItem>? Items { get; set; }

                public CollectionItem[]? ItemArray { get; set; }

                public IList<CollectionItem>? ItemIList { get; set; }

                public List<ParityColor>? Colors { get; set; }

                public List<Guid>? Guids { get; set; }

                public List<int?>? NullableInts { get; set; }

                public List<decimal>? Decimals { get; set; }

                public List<TimeSpan>? Spans { get; set; }

                public List<string> GetOnlyList { get; } = new();

                public List<CollectionItem> GetOnlyItems { get; } = new();

                public byte[]? Blob { get; set; }
            }
        }

        """ + "\n" + Support;

    /// <summary>
    /// One dictionary property per key and value combination the binder supports.
    /// </summary>
    public const string MappingTypes = """
        namespace Parity.Settings
        {
            using System;
            using System.Collections.Generic;
            using Parity.Settings.Attributes;
            using Microsoft.Extensions.Configuration;

            public enum ParityColor
            {
                Red,
                Green,
                Blue,
            }

            public class MappingItem
            {
                public string? Name { get; set; }

                public int? Weight { get; set; }
            }

            [ConfigurationObject]
            public class MappingRoot
            {
                public Dictionary<string, int>? StringIntMap { get; set; }

                public IDictionary<string, string>? StringIDictionary { get; set; }

                public IReadOnlyDictionary<string, string>? StringReadOnlyDictionary { get; set; }

                public Dictionary<int, string>? IntKeyMap { get; set; }

                public Dictionary<ParityColor, string>? EnumKeyMap { get; set; }

                public Dictionary<string, ParityColor>? EnumValueMap { get; set; }

                public Dictionary<string, Guid>? GuidValueMap { get; set; }

                public Dictionary<string, int?>? NullableValueMap { get; set; }

                public Dictionary<string, MappingItem>? ObjectMap { get; set; }

                public IDictionary<string, MappingItem>? ObjectIDictionary { get; set; }

                public IReadOnlyDictionary<string, MappingItem>? ObjectReadOnlyDictionary { get; set; }

                public Dictionary<string, List<int>>? ListMap { get; set; }

                public Dictionary<string, string[]>? ArrayMap { get; set; }

                public Dictionary<string, List<MappingItem>>? ObjectListMap { get; set; }

                public Dictionary<string, Dictionary<string, int>>? NestedMap { get; set; }

                public Dictionary<string, Dictionary<string, MappingItem>>? NestedObjectMap { get; set; }

                public Dictionary<string, int> GetOnlyMap { get; } = new();

                public Dictionary<string, MappingItem> GetOnlyObjectMap { get; } = new();
            }
        }

        """ + "\n" + Support;

    /// <summary>
    /// Minimal stand-ins for the runtime types the generated code depends on.
    /// </summary>
    public const string Support = """
        namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys
        {
            using System;
            using System.Linq;

            public readonly record struct ConfigurationLeafDescriptor(
                ConfigurationPath Path,
                Type ConfigurationValueType,
                string? Description = "");

            public readonly struct ConfigurationPath(params string[] segments) : IEquatable<ConfigurationPath>
            {
                public string[] Segments { get; } = segments ?? [];

                public bool Equals(ConfigurationPath other) =>
                    (Segments ?? []).SequenceEqual(other.Segments ?? [], StringComparer.Ordinal);

                public override bool Equals(object? obj) => obj is ConfigurationPath other && Equals(other);

                public override int GetHashCode()
                {
                    HashCode hash = new();
                    foreach (string segment in Segments ?? [])
                    {
                        hash.Add(segment, StringComparer.Ordinal);
                    }

                    return hash.ToHashCode();
                }
            }
        }

        namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts
        {
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys;

            public abstract class ConfigurationObjectContextBase<T>
            {
                public abstract IReadOnlySet<ConfigurationPath> GetReadableConfigurationPaths();

                public abstract IReadOnlySet<ConfigurationPath> GetWritableConfigurationPaths();

                public abstract IReadOnlySet<ConfigurationLeafDescriptor> GetBindableConfigurationLeafDescriptors();

                protected static void AppendNestedLeafDescriptors(
                    ISet<ConfigurationLeafDescriptor> destination,
                    string parentKey,
                    IEnumerable<ConfigurationLeafDescriptor> nestedLeaves)
                {
                    AppendNestedLeafDescriptors(destination, new[] { parentKey }, nestedLeaves);
                }

                protected static void AppendNestedLeafDescriptors(
                    ISet<ConfigurationLeafDescriptor> destination,
                    string[] parentKeys,
                    IEnumerable<ConfigurationLeafDescriptor> nestedLeaves)
                {
                    foreach (ConfigurationLeafDescriptor leaf in nestedLeaves)
                    {
                        destination.Add(new ConfigurationLeafDescriptor(
                            new ConfigurationPath(parentKeys.Concat(leaf.Path.Segments).ToArray()),
                            leaf.ConfigurationValueType,
                            leaf.Description));
                    }
                }

                public abstract IReadOnlyDictionary<string, object?> DumpConfigurationObject(
                    T instance,
                    bool considerWritablePropertiesOnly = true,
                    params string[] parentKeys);

                protected static void AppendNestedPaths(
                    ISet<ConfigurationPath> destination,
                    string parentKey,
                    IEnumerable<ConfigurationPath> nestedPaths)
                {
                    foreach (ConfigurationPath path in nestedPaths)
                    {
                        destination.Add(new ConfigurationPath(new[] { parentKey }.Concat(path.Segments).ToArray()));
                    }
                }

                protected static void AppendNestedPaths(
                    ISet<ConfigurationPath> destination,
                    string[] parentKeys,
                    IEnumerable<ConfigurationPath> nestedPaths)
                {
                    foreach (ConfigurationPath path in nestedPaths)
                    {
                        destination.Add(new ConfigurationPath(parentKeys.Concat(path.Segments).ToArray()));
                    }
                }
            }
        }
    """;
}
// AI GENERATED END
