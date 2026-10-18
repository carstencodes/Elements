// SPDX-Identifier: MIT
//
//  (C) 2023-2026 Carsten Igel.
//  Published under MIT License

using System;
using System.Diagnostics.CodeAnalysis;

namespace HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys;

/// <summary>
/// Represents a descriptor for a configuration leaf node, containing details
/// about the configuration's path, value type, and an optional description.
/// </summary>
/// <remarks>
/// This struct is used to define and uniquely identify a specific configuration
/// leaf within a hierarchical settings structure. It includes both the path
/// to the configuration entity and the type of value it represents.
/// </remarks>
public readonly struct ConfigurationLeafDescriptor(
    ConfigurationPath path,
    Type configurationValueType,
    string? description = ""): IEquatable<ConfigurationLeafDescriptor>
{
    /// <summary>
    /// Gets the description of the configuration leaf descriptor.
    /// This provides additional context or information regarding
    /// the specific configuration entry, typically useful for
    /// documentation or display purposes.
    /// </summary>
    public string? Description { get; } = description ?? string.Empty;

    /// <summary>
    /// Represents the hierarchical path associated with a configuration leaf descriptor.
    /// This property encapsulates the structured sequence of keys or segments
    /// that define the precise location of a configuration value within a configuration data structure.
    /// </summary>
    public ConfigurationPath Path { get; } = path;

    /// <summary>
    /// Represents the data type of a configuration value associated with a configuration path.
    /// </summary>
    /// <remarks>
    /// This property is used to define the type of the value stored at a specific configuration path.
    /// It ensures type-safety when working with configuration values and provides context about the expected value type.
    /// </remarks>
    public Type ConfigurationValueType { get; } = configurationValueType;

    /// <summary>
    /// Returns a string representation of the current instance.
    /// </summary>
    /// <returns>
    /// A string that concatenates the <c>Path</c> and the type of the configuration value
    /// in the format: "<c>Path -> (ConfigurationValueType)</c>".
    /// </returns>
    public override string ToString()
    {
        return $"{this.Path} -> ({this.ConfigurationValueType})";
    }

    /// <summary>
    /// Determines whether the specified <see cref="ConfigurationLeafDescriptor"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">
    /// The <see cref="ConfigurationLeafDescriptor"/> to compare with the current instance.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="ConfigurationLeafDescriptor"/> is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    [SuppressMessage("Design", "MA0169:Use Equals method instead of operator", Justification = "Intentional use of == operator for `System.Type` comparison.")]
    public bool Equals(ConfigurationLeafDescriptor other)
    {
        return this.Path.Equals(other.Path)
               && this.ConfigurationValueType == other.ConfigurationValueType;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="ConfigurationLeafDescriptor"/> instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is ConfigurationLeafDescriptor other && this.Equals(other);
    }

    /// <summary>
    /// Returns a hash code for the current instance of the <see cref="ConfigurationLeafDescriptor"/>.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code that uniquely represents the current instance.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Path, this.ConfigurationValueType);
    }

    /// <summary>
    /// Determines whether two <see cref="ConfigurationLeafDescriptor"/> instances are equal.
    /// </summary>
    /// <param name="left">
    /// The first <see cref="ConfigurationLeafDescriptor"/> instance to compare.
    /// </param>
    /// <param name="right">
    /// The second <see cref="ConfigurationLeafDescriptor"/> instance to compare.
    /// </param>
    /// <returns>
    /// <c>true</c> if the two instances are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(ConfigurationLeafDescriptor left, ConfigurationLeafDescriptor right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified <c>ConfigurationLeafDescriptor</c> instances are not equal.
    /// </summary>
    /// <param name="left">The first <c>ConfigurationLeafDescriptor</c> to compare.</param>
    /// <param name="right">The second <c>ConfigurationLeafDescriptor</c> to compare.</param>
    /// <returns>
    /// <c>true</c> if the two <c>ConfigurationLeafDescriptor</c> instances are not equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(ConfigurationLeafDescriptor left, ConfigurationLeafDescriptor right)
    {
        return !(left == right);
    }
}
