using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

public sealed class CommandLineConfigurationProvider() : IConfigurationProvider
{
    private readonly Dictionary<string, string> configurationValues = new();
    private readonly ConfigurationReloadToken changeToken = new();

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

    public IChangeToken GetReloadToken()
    {
        return this.changeToken;
    }

    public void Load()
    {
    }

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

    public bool TryGet(string key, [NotNullWhen(true)] out string? value)
    {
        return this.configurationValues.TryGetValue(key, out value);
    }
}
