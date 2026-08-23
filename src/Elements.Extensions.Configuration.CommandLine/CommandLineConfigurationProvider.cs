// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Provides configuration key-value pairs parsed from command-line arguments.
/// </summary>
public sealed class CommandLineConfigurationProvider() : IConfigurationProvider
{
    private readonly Dictionary<string, string> configurationValues = new();
    private readonly ConfigurationReloadToken changeToken = new();

    /// <summary>
    /// Returns the immediate descendant configuration keys for a given parent path.
    /// </summary>
    /// <param name="earlierKeys">The child keys returned by preceding providers for the same parent path.</param>
    /// <param name="parentPath">The parent configuration path.</param>
    /// <returns>The combined sequence of child configuration keys.</returns>
    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        /* AI Generated content using CoPilot Free with public code filter enabled */
        ISet<string> childKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (string key in earlierKeys)
        {
            childKeys.Add(key);
        }

        string prefix = string.IsNullOrEmpty(parentPath)
            ? string.Empty
            : parentPath + ConfigurationPath.KeyDelimiter;

        foreach (string key in this.configurationValues.Keys)
        {
            if (!key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string childKey = key[prefix.Length..];
            int delimiterIndex = childKey.IndexOf(ConfigurationPath.KeyDelimiter);

            if (delimiterIndex >= 0)
            {
                childKey = childKey[..delimiterIndex];
            }

            if (!string.IsNullOrEmpty(childKey))
            {
                childKeys.Add(childKey);
            }
        }

        return childKeys;
        /* End of AI Generated content using CoPilot Free with public code filter enabled */
    }

    /// <summary>
    /// Returns a change token that triggers when configuration values are modified.
    /// </summary>
    /// <returns>An <see cref="IChangeToken"/> tracking reloads.</returns>
    public IChangeToken GetReloadToken()
    {
        return this.changeToken;
    }

    /// <summary>
    /// Loads configuration values into the provider.
    /// </summary>
    public void Load()
    {
    }

    /// <summary>
    /// Sets or removes a configuration value for the specified key.
    /// </summary>
    /// <param name="key">The configuration key.</param>
    /// <param name="value">The configuration value to set, or <see langword="null"/> to remove the key.</param>
    public void Set(string key, string? value)
    {
        if (value is not null)
        {
            this.configurationValues[key] = value;
        }
        else
        {
            this.configurationValues.Remove(key);
        }

        this.changeToken.OnReload();
    }

    /// <summary>
    /// Attempts to retrieve a configuration value for the specified key.
    /// </summary>
    /// <param name="key">The configuration key to locate.</param>
    /// <param name="value">When this method returns, contains the configuration value if found; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the configuration key exists; otherwise, <see langword="false"/>.</returns>
    public bool TryGet(string key, [NotNullWhen(true)] out string? value)
    {
        return this.configurationValues.TryGetValue(key, out value);
    }
}
