// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

using System;
using System.Collections;
using System.Collections.Generic;
using MicrosoftConfigurationPath = Microsoft.Extensions.Configuration.ConfigurationPath;

namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys;

/// <summary>
/// Represents a hierarchical configuration path composed of segments.
/// </summary>
/// <remarks>
/// This struct is used to define unique paths within a configuration structure.
/// A configuration path can be constructed using an array of string segments,
/// where each segment represents a level in the hierarchy. The structure supports
/// path concatenation and equality comparisons.
/// </remarks>
public readonly struct ConfigurationPath(params string[] segments) : IEquatable<ConfigurationPath>, IEnumerable<string>
{
    /// <summary>
    /// Gets an array of strings representing the individual segments of the configuration path.
    /// These segments are used to navigate and organize hierarchical configuration settings.
    ///
    /// Each segment in the path represents a distinct level or key within the configuration hierarchy.
    /// This property can be used to retrieve or manipulate the structure of the configuration path.
    /// </summary>
    public string[] Segments { get; } = segments ?? [];

    // AI GENERATED START - model: gemini-3.7-flash
    /// <summary>
    /// Appends the specified child paths to the current configuration path.
    /// </summary>
    /// <param name="childPaths">An enumerable collection of child paths to append.</param>
    /// <returns>An enumerable collection of <see cref="ConfigurationPath"/> instances representing the combined paths.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="childPaths"/> is null.</exception>
    public IEnumerable<ConfigurationPath> Append(IEnumerable<ConfigurationPath> childPaths)
    {
        ArgumentNullException.ThrowIfNull(childPaths);
        return this.AppendCore(childPaths);
    }

    /// <summary>
    /// Iterates over child paths and yields concatenated <see cref="ConfigurationPath"/> instances.
    /// </summary>
    /// <param name="childPaths">An enumerable collection of child paths to append.</param>
    /// <returns>An enumerable collection of concatenated configuration paths.</returns>
    private IEnumerable<ConfigurationPath> AppendCore(IEnumerable<ConfigurationPath> childPaths)
    {
        foreach (ConfigurationPath child in childPaths)
        {
            yield return new ConfigurationPath([.. this.Segments ?? [], .. child.Segments ?? []]);
        }
    }
    // AI GENERATED END

    /// <summary>
    /// Converts the configuration path into a string representation by joining its segments
    /// using the key delimiter, as specified in the Microsoft.Extensions.Configuration library.
    /// </summary>
    /// <returns>
    /// A string representation of the configuration path, with segments joined by the
    /// key delimiter.
    /// </returns>
    public override string ToString()
    {
        return string.Join(MicrosoftConfigurationPath.KeyDelimiter, this.Segments ?? []);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the segments of the configuration path.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the segments of the configuration path.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.Segments.GetEnumerator();
    }

    /// <summary>
    /// Determines whether the specified <see cref="ConfigurationPath"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">
    /// The <see cref="ConfigurationPath"/> to compare with the current instance.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="ConfigurationPath"/> is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(ConfigurationPath other)
    {
        // AI GENERATED START - model: Claude Sonnet 5.5
        // A default struct has null segments; treat it as an empty path.
        string[] left = this.Segments ?? [];
        string[] right = other.Segments ?? [];
        return System.Linq.Enumerable.SequenceEqual(left, right, StringComparer.Ordinal);
        // AI GENERATED END
    }

    /// <summary>
    /// Returns an enumerator that iterates through the segments of the configuration path.
    /// </summary>
    /// <returns>An enumerator for the <see cref="Segments"/> of the configuration path.</returns>
    public IEnumerator<string> GetEnumerator()
    {
        return ((IEnumerable<string>)this.Segments).GetEnumerator();
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="ConfigurationPath"/> instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="ConfigurationPath"/>.</param>
    /// <returns>
    /// true if the specified object is a <see cref="ConfigurationPath"/> and is equal to the current instance; otherwise, false.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is ConfigurationPath other && this.Equals(other);
    }

    /// <summary>
    /// Returns a hash code for the current <see cref="ConfigurationPath"/> instance.
    /// </summary>
    /// <returns>
    /// An integer representing the hash code of the <see cref="ConfigurationPath"/> instance,
    /// based on the hash code of its <see cref="Segments"/> array.
    /// </returns>
    public override int GetHashCode()
    {
        // AI GENERATED START - model: Claude Sonnet 5.5
        HashCode hash = new();
        foreach (string segment in this.Segments ?? [])
        {
            hash.Add(segment, StringComparer.Ordinal);
        }

        return hash.ToHashCode();
        // AI GENERATED END
    }

    /// Represents an equality operator for comparing two instances of the ConfigurationPath structure.
    ///
    /// Parameters:
    /// left: The first ConfigurationPath instance to compare.
    /// right: The second ConfigurationPath instance to compare.
    ///
    /// Returns:
    /// true if the two ConfigurationPath instances are equal; otherwise, false.
    public static bool operator ==(ConfigurationPath left, ConfigurationPath right)
    {
        return left.Equals(right);
    }

    /// Compares two ConfigurationPath instances for inequality.
    ///
    /// This operator determines whether two ConfigurationPath instances do not
    /// have the same segments.
    ///
    /// Parameters:
    /// left:
    /// The first ConfigurationPath instance to compare.
    ///
    /// right:
    /// The second ConfigurationPath instance to compare.
    ///
    /// Returns:
    /// true if the ConfigurationPath instances do not have equal segments;
    /// otherwise, false.
    public static bool operator !=(ConfigurationPath left, ConfigurationPath right)
    {
        return !left.Equals(right);
    }
}
