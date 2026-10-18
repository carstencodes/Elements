// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

using System;
using System.Collections.Generic;

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
    public abstract IReadOnlySet<string[]> GetReadableConfigurationPaths();

    /// <summary>
    /// When overridden in a derived class, gets the read-only set of configuration paths that are writable.
    /// </summary>
    /// <returns>A read-only set containing the writable configuration path segments.</returns>
    public abstract IReadOnlySet<string[]> GetWritableConfigurationPaths();

    /// <summary>
    /// Gets the set of all configuration paths, combining both readable and writable paths.
    /// </summary>
    /// <returns>A read-only set containing all unique readable and writable configuration path segments.</returns>
    public IReadOnlySet<string[]> GetConfigurationPaths()
    {
        IReadOnlySet<string[]> readablePaths = this.GetReadableConfigurationPaths();
        IReadOnlySet<string[]> writablePaths = this.GetWritableConfigurationPaths();
        HashSet<string[]> allPaths = new(readablePaths, new ConfigurationPathComparer());
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
        ISet<string[]> destination,
        string parentKey,
        IEnumerable<string[]> nestedPaths)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(parentKey);
        ArgumentNullException.ThrowIfNull(nestedPaths);

        foreach (string[] path in nestedPaths)
        {
            string[] combined = new string[path.Length + 1];
            combined[0] = parentKey;
            Array.Copy(path, 0, combined, 1, path.Length);
            destination.Add(combined);
        }
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
        ISet<string[]> destination,
        string[] parentKeys,
        IEnumerable<string[]> nestedPaths)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(parentKeys);
        ArgumentNullException.ThrowIfNull(nestedPaths);

        foreach (string[] path in nestedPaths)
        {
            string[] combined = new string[parentKeys.Length + path.Length];
            Array.Copy(parentKeys, 0, combined, 0, parentKeys.Length);
            Array.Copy(path, 0, combined, parentKeys.Length, path.Length);
            destination.Add(combined);
        }
    }
    // AI GENERATED END
}
