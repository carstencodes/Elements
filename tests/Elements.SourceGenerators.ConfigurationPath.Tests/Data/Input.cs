// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests.Data;

// AI GENERATED START - model: Copilot
internal static class Input
{
    public const string NestedModels = """
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

                                               public string ReadOnlyName { get; } = "read-only";

                                               public int ReadOnlyScore { get; } = 9;

                                               public string? NullableName { get; set; }

                                               public int? NullableScore { get; set; }

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

    public const string FallbackNamespace = """
                                            namespace Test.Models
                                            {
                                                using HedgeCraft.Extensions.Configuration.UserSettings.Attributes;

                                                [ConfigurationObject]
                                                public struct Settings
                                                {
                                                    public string Name { get; }
                                                }
                                            }

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

    public const string ExistingContext = """
                                          namespace Existing.Context
                                          {
                                              using System.Collections.Generic;
                                              using Demo.Context.Attributes;
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

                                              internal sealed class ExistingChildContext :
                                                  HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts.ConfigurationObjectContextBase<Child>
                                              {
                                                  public override IReadOnlySet<ConfigurationPath> GetReadableConfigurationPaths() => new HashSet<ConfigurationPath>();

                                                  public override IReadOnlySet<ConfigurationPath> GetWritableConfigurationPaths() => new HashSet<ConfigurationPath>();

                                                  public override IReadOnlySet<ConfigurationLeafDescriptor> GetBindableConfigurationLeafDescriptors() => new HashSet<ConfigurationLeafDescriptor>();

                                                  public override IReadOnlyDictionary<string, object?> DumpConfigurationObject(
                                                      Child instance,
                                                      bool considerWritablePropertiesOnly = true,
                                                      params string[] parentKeys) => new Dictionary<string, object?>();
                                              }
                                          }

                                          namespace Demo.Context.Attributes
                                          {
                                          }

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

    public const string BindingScenario = """
                                          namespace BindingScenario.Settings
                                          {
                                              using BindingScenario.Settings.Attributes;
                                              using Microsoft.Extensions.Configuration;

                                              [ConfigurationObject]
                                              public class Root
                                              {
                                                  [ConfigurationKeyName("renamed")]
                                                  public Child Child { get; set; } = new();

                                                  public string ReadOnlyName { get; } = "read-only";

                                                  public int ReadOnlyScore { get; } = 9;

                                                  public string? NullableName { get; set; }

                                                  public int? NullableScore { get; set; }
                                              }

                                              public class Child
                                              {
                                                  public string Name { get; set; } = string.Empty;
                                              }
                                          }

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

    public const string CollidingMemberScenario = """
                                                  namespace Collision.Settings
                                                  {
                                                      using Collision.Settings.Attributes;

                                                      [ConfigurationObject]
                                                      public class CollidingModel
                                                      {
                                                          public string Name { get; set; } = string.Empty;
                                                          public string GetConfigurationContext { get; set; } = string.Empty;
                                                      }
                                                  }

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
                                                      }
                                                  }
                                                  """;

    public const string UnboundGenericScenario = """
                                                 namespace Generics.Settings
                                                 {
                                                     using Generics.Settings.Attributes;

                                                     [ConfigurationObject]
                                                     public class UnboundContainer<T>
                                                     {
                                                         public T? Value { get; set; }
                                                     }
                                                 }

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
                                                     }
                                                 }
                                                 """;

    public const string MultiNamespaceScenario = """
                                                 namespace NamespaceA.Settings
                                                 {
                                                     using NamespaceA.Settings.Attributes;

                                                     [ConfigurationObject]
                                                     public class ModelA
                                                     {
                                                         public string Title { get; set; } = string.Empty;
                                                     }
                                                 }

                                                 namespace NamespaceB.Settings
                                                 {
                                                     using NamespaceA.Settings.Attributes;

                                                     [ConfigurationObject]
                                                     public class ModelB
                                                     {
                                                         public int Count { get; set; }
                                                     }
                                                 }

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
                                                     }
                                                 }
                                                 """;
}
// AI GENERATED END
