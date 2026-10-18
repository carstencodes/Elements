// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests.Data;

// AI GENERATED START - model: Claude Sonnet 5.5
/// <summary>Provides generator inputs for the bindable configuration leaf descriptor tests.</summary>
internal static class LeafDescriptorInput
{
    /// <summary>Models covering simple, nullable, enum, collection, nested, generic and cyclic properties.</summary>
    public const string Models = """
                                 namespace Leaf.Settings
                                 {
                                     using Leaf.Settings.Attributes;
                                     using Microsoft.Extensions.Configuration;
                                     using System.Collections.Generic;

                                     public enum Mode
                                     {
                                         Fast,
                                         Slow,
                                     }

                                     [ConfigurationObject]
                                     public class Root
                                     {
                                         /// <summary>
                                         ///   The   display
                                         ///   name.
                                         /// </summary>
                                         public string Title { get; set; } = string.Empty;

                                         public int Port { get; set; }

                                         public int? Retries { get; set; }

                                         public Mode Mode { get; set; }

                                         public List<string> Tags { get; set; } = new();

                                         public int[] Scores { get; set; } = [];

                                         public Dictionary<string, int> Limits { get; set; } = new();

                                         public List<Child> Children { get; set; } = new();

                                         [ConfigurationKeyName("renamed")]
                                         public Child Child { get; set; } = new();

                                         public Node Node { get; set; } = new();

                                         public Box<Child> Box { get; set; } = new();

                                         public byte[] Blob { get; set; } = [];

                                         public List<int?> Maybe { get; set; } = new();

                                         public Child[] ChildArray { get; set; } = [];

                                         public Dictionary<string, Child> ChildMap { get; set; } = new();

                                         /// <summary></summary>
                                         public string EmptyDoc { get; set; } = string.Empty;
                                     }

                                     public class Child
                                     {
                                         public string Name { get; set; } = string.Empty;

                                         public Grand Grand { get; set; } = new();
                                     }

                                     public class Grand
                                     {
                                         public bool Flag { get; set; }
                                     }

                                     public class Node
                                     {
                                         public string Label { get; set; } = string.Empty;

                                         public Node? Next { get; set; }
                                     }

                                     public class Box<T> where T : class, new()
                                     {
                                         public T Value { get; set; } = new();
                                     }
                                 }
                                 """;

    /// <summary>A model whose nested context is user-defined and returns its own leaves.</summary>
    public const string UserContextModels = """
                                            namespace Leaf.User
                                            {
                                                using System.Collections.Generic;
                                                using Leaf.User.Attributes;
                                                using HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys;

                                                [ConfigurationObject]
                                                public class Root
                                                {
                                                    public Child Child { get; } = new();
                                                }

                                                public class Child
                                                {
                                                    public string Name { get; set; } = string.Empty;
                                                }

                                                internal sealed class CustomChildContext :
                                                    HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts.ConfigurationObjectContextBase<Child>
                                                {
                                                    public override IReadOnlySet<ConfigurationPath> GetReadableConfigurationPaths() => new HashSet<ConfigurationPath>();

                                                    public override IReadOnlySet<ConfigurationPath> GetWritableConfigurationPaths() => new HashSet<ConfigurationPath>();

                                                    public override IReadOnlySet<ConfigurationLeafDescriptor> GetBindableConfigurationLeafDescriptors() =>
                                                        new HashSet<ConfigurationLeafDescriptor>
                                                        {
                                                            new(new ConfigurationPath("Custom"), typeof(string), "custom leaf"),
                                                        };

                                                    public override IReadOnlyDictionary<string, object?> DumpConfigurationObject(
                                                        Child instance,
                                                        bool considerWritablePropertiesOnly = true,
                                                        params string[] parentKeys) => new Dictionary<string, object?>();
                                                }
                                            }
                                            """;

    /// <summary>Minimal runtime stand-ins of the configuration key and context base types.</summary>
    public const string ContextStubs = """
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

    /// <summary>Gets the combined source of the model scenario and the context stubs.</summary>
    public static string ModelScenario => Models + "\n" + ContextStubs;

    /// <summary>Gets the combined source of the user context scenario and the context stubs.</summary>
    public static string UserContextScenario => UserContextModels + "\n" + ContextStubs;
}
// AI GENERATED END
