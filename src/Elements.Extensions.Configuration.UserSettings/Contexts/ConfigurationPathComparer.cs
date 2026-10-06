// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;

namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts;

// AI GENERATED START - model: Copilot
internal sealed class ConfigurationPathComparer : IEqualityComparer<string[]>
{
    public bool Equals(string[]? left, string[]? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null || left.Length != right.Length)
        {
            return false;
        }

        for (int index = 0; index < left.Length; index++)
        {
            if (!StringComparer.Ordinal.Equals(left[index], right[index]))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(string[] path)
    {
        ArgumentNullException.ThrowIfNull(path);
        unchecked
        {
            int hash = 17;
            foreach (string key in path)
            {
                hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(key);
            }

            return hash;
        }
    }
}
// AI GENERATED END
