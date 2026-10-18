// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

using System;
using System.Collections.Generic;
using HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys;

namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts;

/// <summary>
/// Provides a base class for configuration object contexts that define configuration path metadata
/// and serialization mappings for a bound configuration type.
/// </summary>
/// <typeparam name="TBoundType">The type of the configuration object bound to this context.</typeparam>
public abstract class ConfigurationObjectContextBase<TBoundType>
{
    /// <summary>
    /// When overridden in a derived class, gets the read-only set of configuration paths that are readable.
    /// </summary>
    /// <returns>A read-only set containing the readable configuration path segments.</returns>
    public abstract IReadOnlySet<ConfigurationPath> GetReadableConfigurationPaths();

    /// <summary>
    /// When overridden in a derived class, gets the read-only set of configuration paths that are writable.
    /// </summary>
    /// <returns>A read-only set containing the writable configuration path segments.</returns>
    public abstract IReadOnlySet<ConfigurationPath> GetWritableConfigurationPaths();

    /// <summary>
    /// When overridden in a derived class, retrieves the set of descriptors for bindable configuration leaf nodes
    /// associated with the bound configuration object type, i.e. the nodes that accept primitive values only and
    /// can be bound to a configuration source using the
    /// <code>Microsoft.Extensions.Configuration.IConfigurationBinder</code> APIs.
    /// </summary>
    /// <returns>A read-only set containing descriptors for bindable configuration leaf nodes.</returns>
    public abstract IReadOnlySet<ConfigurationLeafDescriptor> GetBindableConfigurationLeafDescriptors();

    /// <summary>
    /// Gets the set of all configuration paths, combining both readable and writable paths.
    /// </summary>
    /// <returns>A read-only set containing all unique readable and writable configuration path segments.</returns>
    public IReadOnlySet<ConfigurationPath> GetConfigurationPaths()
    {
        IReadOnlySet<ConfigurationPath> readablePaths = this.GetReadableConfigurationPaths();
        IReadOnlySet<ConfigurationPath> writablePaths = this.GetWritableConfigurationPaths();
        HashSet<ConfigurationPath> allPaths = new(readablePaths);
        allPaths.UnionWith(writablePaths);
        return allPaths;
    }

    /// <summary>
    /// When overridden in a derived class, dumps the specified configuration object instance into a
    /// dictionary representation.
    /// </summary>
    /// <param name="instance">The configuration object instance to dump.</param>
    /// <param name="considerWritablePropertiesOnly">
    /// Whether to omit read-only simple properties. Non-simple properties are retained to include their children.
    /// </param>
    /// <param name="parentKeys">The parent configuration keys to prepend to dumped keys.</param>
    /// <returns>A read-only dictionary mapping configuration keys to their corresponding values.</returns>
    public abstract IReadOnlyDictionary<string, object?> DumpConfigurationObject(
        TBoundType instance,
        bool considerWritablePropertiesOnly = true,
        params string[] parentKeys);

    // AI GENERATED START - model: gemini-3.7-flash
    /// <summary>
    /// Prefixes the specified nested configuration path segments with a parent key and adds them to the destination set.
    /// </summary>
    /// <param name="destination">The destination set receiving the prefixed configuration paths.</param>
    /// <param name="parentKey">The parent key segment to prefix.</param>
    /// <param name="nestedPaths">The collection of nested configuration path segments.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="destination"/>, <paramref name="parentKey"/>, or <paramref name="nestedPaths"/> is <see langword="null"/>.
    /// </exception>
    protected static void AppendNestedPaths(
        ISet<ConfigurationPath> destination,
        string parentKey,
        IEnumerable<ConfigurationPath> nestedPaths)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(parentKey);
        ArgumentNullException.ThrowIfNull(nestedPaths);

        destination.UnionWith(new ConfigurationPath(parentKey).Append(nestedPaths));
    }

    /// <summary>
    /// Prefixes the specified nested configuration path segments with multiple parent keys and adds them to the destination set.
    /// </summary>
    /// <param name="destination">The destination set receiving the prefixed configuration paths.</param>
    /// <param name="parentKeys">The parent key segments to prefix.</param>
    /// <param name="nestedPaths">The collection of nested configuration path segments.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="destination"/>, <paramref name="parentKeys"/>, or <paramref name="nestedPaths"/> is <see langword="null"/>.
    /// </exception>
    protected static void AppendNestedPaths(
        ISet<ConfigurationPath> destination,
        string[] parentKeys,
        IEnumerable<ConfigurationPath> nestedPaths)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(parentKeys);
        ArgumentNullException.ThrowIfNull(nestedPaths);

        destination.UnionWith(new ConfigurationPath(parentKeys).Append(nestedPaths));
    }

    /// <summary>
    /// Prefixes the specified nested configuration leaf descriptors with a parent key and adds them to the
    /// destination set.
    /// </summary>
    /// <param name="destination">The destination set receiving the prefixed leaf descriptors.</param>
    /// <param name="parentKey">The parent key segment to prefix.</param>
    /// <param name="nestedLeaves">The leaf descriptors of the nested context.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="destination"/>, <paramref name="parentKey"/>, or <paramref name="nestedLeaves"/> is <see langword="null"/>.
    /// </exception>
    protected static void AppendNestedLeafDescriptors(
        ISet<ConfigurationLeafDescriptor> destination,
        string parentKey,
        IEnumerable<ConfigurationLeafDescriptor> nestedLeaves)
    {
        ArgumentNullException.ThrowIfNull(parentKey);

        AppendNestedLeafDescriptors(destination, new[] { parentKey }, nestedLeaves);
    }

    /// <summary>
    /// Prefixes the specified nested configuration leaf descriptors with multiple parent keys and adds them to the
    /// destination set. The value type and description of each descriptor are preserved.
    /// </summary>
    /// <param name="destination">The destination set receiving the prefixed leaf descriptors.</param>
    /// <param name="parentKeys">The parent key segments to prefix.</param>
    /// <param name="nestedLeaves">The leaf descriptors of the nested context.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="destination"/>, <paramref name="parentKeys"/>, or <paramref name="nestedLeaves"/> is <see langword="null"/>.
    /// </exception>
    protected static void AppendNestedLeafDescriptors(
        ISet<ConfigurationLeafDescriptor> destination,
        string[] parentKeys,
        IEnumerable<ConfigurationLeafDescriptor> nestedLeaves)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(parentKeys);
        ArgumentNullException.ThrowIfNull(nestedLeaves);

        foreach (ConfigurationLeafDescriptor leaf in nestedLeaves)
        {
            ConfigurationPath prefixed = new([.. parentKeys, .. leaf.Path.Segments ?? []]);
            destination.Add(new ConfigurationLeafDescriptor(prefixed, leaf.ConfigurationValueType, leaf.Description));
        }
    }
    // AI GENERATED END
}
