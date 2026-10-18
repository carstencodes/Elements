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

                                       namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts
                                       {
                                           using System;
                                           using System.Collections.Generic;

                                           public abstract class ConfigurationObjectContextBase<T>
                                           {
                                               public abstract IReadOnlySet<string[]> GetReadableConfigurationPaths();

                                               public abstract IReadOnlySet<string[]> GetWritableConfigurationPaths();

                                               public abstract IReadOnlyDictionary<string, object?> DumpConfigurationObject(
                                                   T instance,
                                                   bool considerWritablePropertiesOnly = true,
                                                   params string[] parentKeys);

                                               protected static void AppendNestedPaths(
                                                   ISet<string[]> destination,
                                                   string parentKey,
                                                   IEnumerable<string[]> nestedPaths)
                                               {
                                                   foreach (string[] path in nestedPaths)
                                                   {
                                                       string[] combined = new string[path.Length + 1];
                                                       combined[0] = parentKey;
                                                       Array.Copy(path, 0, combined, 1, path.Length);
                                                       destination.Add(combined);
                                                   }
                                               }

                                               protected static void AppendNestedPaths(
                                                   ISet<string[]> destination,
                                                   string[] parentKeys,
                                                   IEnumerable<string[]> nestedPaths)
                                               {
                                                   foreach (string[] path in nestedPaths)
                                                   {
                                                       string[] combined = new string[parentKeys.Length + path.Length];
                                                       Array.Copy(parentKeys, 0, combined, 0, parentKeys.Length);
                                                       Array.Copy(path, 0, combined, parentKeys.Length, path.Length);
                                                       destination.Add(combined);
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

                                            namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts
                                            {
                                                using System;
                                                using System.Collections.Generic;

                                                public abstract class ConfigurationObjectContextBase<T>
                                                {
                                                    public abstract IReadOnlySet<string[]> GetReadableConfigurationPaths();

                                                    public abstract IReadOnlySet<string[]> GetWritableConfigurationPaths();

                                                    public abstract IReadOnlyDictionary<string, object?> DumpConfigurationObject(
                                                        T instance,
                                                        bool considerWritablePropertiesOnly = true,
                                                        params string[] parentKeys);

                                                    protected static void AppendNestedPaths(
                                                        ISet<string[]> destination,
                                                        string parentKey,
                                                        IEnumerable<string[]> nestedPaths)
                                                    {
                                                        foreach (string[] path in nestedPaths)
                                                        {
                                                            string[] combined = new string[path.Length + 1];
                                                            combined[0] = parentKey;
                                                            Array.Copy(path, 0, combined, 1, path.Length);
                                                            destination.Add(combined);
                                                        }
                                                    }

                                                    protected static void AppendNestedPaths(
                                                        ISet<string[]> destination,
                                                        string[] parentKeys,
                                                        IEnumerable<string[]> nestedPaths)
                                                    {
                                                        foreach (string[] path in nestedPaths)
                                                        {
                                                            string[] combined = new string[parentKeys.Length + path.Length];
                                                            Array.Copy(parentKeys, 0, combined, 0, parentKeys.Length);
                                                            Array.Copy(path, 0, combined, parentKeys.Length, path.Length);
                                                            destination.Add(combined);
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

                                                  public override IReadOnlyDictionary<string, object?> DumpConfigurationObject(
                                                      Child instance,
                                                      bool considerWritablePropertiesOnly = true,
                                                      params string[] parentKeys) => new Dictionary<string, object?>();
                                              }
                                          }

                                          namespace Demo.Context.Attributes
                                          {
                                          }

                                          namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts
                                          {
                                              using System;
                                              using System.Collections.Generic;

                                              public abstract class ConfigurationObjectContextBase<T>
                                              {
                                                  public abstract IReadOnlySet<string[]> GetReadableConfigurationPaths();

                                                  public abstract IReadOnlySet<string[]> GetWritableConfigurationPaths();

                                                  public abstract IReadOnlyDictionary<string, object?> DumpConfigurationObject(
                                                      T instance,
                                                      bool considerWritablePropertiesOnly = true,
                                                      params string[] parentKeys);

                                                  protected static void AppendNestedPaths(
                                                      ISet<string[]> destination,
                                                      string parentKey,
                                                      IEnumerable<string[]> nestedPaths)
                                                  {
                                                      foreach (string[] path in nestedPaths)
                                                      {
                                                          string[] combined = new string[path.Length + 1];
                                                          combined[0] = parentKey;
                                                          Array.Copy(path, 0, combined, 1, path.Length);
                                                          destination.Add(combined);
                                                      }
                                                  }

                                                  protected static void AppendNestedPaths(
                                                      ISet<string[]> destination,
                                                      string[] parentKeys,
                                                      IEnumerable<string[]> nestedPaths)
                                                  {
                                                      foreach (string[] path in nestedPaths)
                                                      {
                                                          string[] combined = new string[parentKeys.Length + path.Length];
                                                          Array.Copy(parentKeys, 0, combined, 0, parentKeys.Length);
                                                          Array.Copy(path, 0, combined, parentKeys.Length, path.Length);
                                                          destination.Add(combined);
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

                                          namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts
                                          {
                                              using System;
                                              using System.Collections.Generic;

                                              public abstract class ConfigurationObjectContextBase<T>
                                              {
                                                  public abstract IReadOnlySet<string[]> GetReadableConfigurationPaths();

                                                  public abstract IReadOnlySet<string[]> GetWritableConfigurationPaths();

                                                  public abstract IReadOnlyDictionary<string, object?> DumpConfigurationObject(
                                                      T instance,
                                                      bool considerWritablePropertiesOnly = true,
                                                      params string[] parentKeys);

                                                  protected static void AppendNestedPaths(
                                                      ISet<string[]> destination,
                                                      string parentKey,
                                                      IEnumerable<string[]> nestedPaths)
                                                  {
                                                      foreach (string[] path in nestedPaths)
                                                      {
                                                          string[] combined = new string[path.Length + 1];
                                                          combined[0] = parentKey;
                                                          Array.Copy(path, 0, combined, 1, path.Length);
                                                          destination.Add(combined);
                                                      }
                                                  }

                                                  protected static void AppendNestedPaths(
                                                      ISet<string[]> destination,
                                                      string[] parentKeys,
                                                      IEnumerable<string[]> nestedPaths)
                                                  {
                                                      foreach (string[] path in nestedPaths)
                                                      {
                                                          string[] combined = new string[parentKeys.Length + path.Length];
                                                          Array.Copy(parentKeys, 0, combined, 0, parentKeys.Length);
                                                          Array.Copy(path, 0, combined, parentKeys.Length, path.Length);
                                                          destination.Add(combined);
                                                      }
                                                  }
                                              }
                                          }
                                          """;
}
// AI GENERATED END
