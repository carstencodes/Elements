// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;

namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts;

// AI GENERATED START - model: Copilot
/// <summary>
/// Compares sequences of configuration path segments for equality and computes their hash codes using ordinal string comparison.
/// </summary>
internal sealed class ConfigurationPathComparer : IEqualityComparer<string[]>
{
    /// <summary>
    /// Determines whether two configuration path segment arrays are equal.
    /// </summary>
    /// <param name="left">The first configuration path segment array to compare.</param>
    /// <param name="right">The second configuration path segment array to compare.</param>
    /// <returns><see langword="true"/> if both arrays contain identical string elements in the same order; otherwise, <see langword="false"/>.</returns>
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

    /// <summary>
    /// Returns a hash code for the specified configuration path segment array.
    /// </summary>
    /// <param name="path">The configuration path segment array.</param>
    /// <returns>A hash code for the specified array computed from its path segments.</returns>
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
