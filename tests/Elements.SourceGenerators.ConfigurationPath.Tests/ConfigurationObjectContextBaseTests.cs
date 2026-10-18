// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts;
using DisplayNameAttribute = TUnit.Core.DisplayNameAttribute;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>
/// Validates helper methods and base class logic on <see cref="ConfigurationObjectContextBase{TBoundType}"/>.
/// </summary>
[DisplayName("Configuration Object Context Base Tests")]
[Description("Validates helper methods and contracts on ConfigurationObjectContextBase.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Performance",
    "CA1822:Mark members as static",
    Justification = "TUnit test methods must be instance methods.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Major Code Smell",
    "S2325:Methods and properties that don't access instance data should be static",
    Justification = "TUnit test methods must be instance methods.")]
internal sealed class ConfigurationObjectContextBaseTests
{
    [Test]
    [DisplayName("AppendNestedPaths prefixes nested paths with single parent key")]
    [Description("Verifies that AppendNestedPaths prefixes each child path array with the given single parent key.")]
    public async Task AppendNestedPathsWithSingleKeyPrefixesPathsCorrectly()
    {
        TestContext context = new();
        HashSet<string[]> destination = new(new TestPathComparer());
        string[][] nestedPaths =
        [
            ["server"],
            ["credentials", "user"],
        ];

        context.InvokeAppendNestedPaths(destination, "database", nestedPaths);

        using (Assert.Multiple())
        {
            await Assert.That(destination.Count)
                .IsEqualTo(2)
                .Because("destination set should contain exactly two prefixed path entries");

            await Assert.That(destination.Contains(["database", "server"]))
                .IsTrue()
                .Because("destination set should contain the single-level nested path prefixed with parent key");

            await Assert.That(destination.Contains(["database", "credentials", "user"]))
                .IsTrue()
                .Because("destination set should contain the multi-level nested path prefixed with parent key");
        }
    }

    [Test]
    [DisplayName("AppendNestedPaths prefixes nested paths with multiple parent keys")]
    [Description("Verifies that AppendNestedPaths prefixes each child path array with multiple parent keys.")]
    public async Task AppendNestedPathsWithMultipleKeysPrefixesPathsCorrectly()
    {
        TestContext context = new();
        HashSet<string[]> destination = new(new TestPathComparer());
        string[][] nestedPaths =
        [
            ["host"],
            ["auth", "token"],
        ];

        context.InvokeAppendNestedPaths(destination, ["services", "database"], nestedPaths);

        using (Assert.Multiple())
        {
            await Assert.That(destination.Count)
                .IsEqualTo(2)
                .Because("destination set should contain exactly two prefixed path entries");

            await Assert.That(destination.Contains(["services", "database", "host"]))
                .IsTrue()
                .Because("destination set should contain the path prefixed with multiple parent keys");

            await Assert.That(destination.Contains(["services", "database", "auth", "token"]))
                .IsTrue()
                .Because("destination set should contain the multi-level path prefixed with multiple parent keys");
        }
    }

    [Test]
    [DisplayName("AppendNestedPaths throws ArgumentNullException on null arguments")]
    [Description("Verifies that both AppendNestedPaths overloads validate against null parameters.")]
    public async Task AppendNestedPathsThrowsOnNullArguments()
    {
        TestContext context = new();
        HashSet<string[]> validDestination = new(new TestPathComparer());
        string[] validKeys = ["parent"];
        string[][] validPaths = [["child"]];

        using (Assert.Multiple())
        {
            await Assert.That(() => context.InvokeAppendNestedPaths(null!, "parent", validPaths))
                .Throws<ArgumentNullException>()
                .Because("null destination should throw ArgumentNullException for single key overload");

            await Assert.That(() => context.InvokeAppendNestedPaths(validDestination, (string)null!, validPaths))
                .Throws<ArgumentNullException>()
                .Because("null parentKey should throw ArgumentNullException for single key overload");

            await Assert.That(() => context.InvokeAppendNestedPaths(validDestination, "parent", null!))
                .Throws<ArgumentNullException>()
                .Because("null nestedPaths should throw ArgumentNullException for single key overload");

            await Assert.That(() => context.InvokeAppendNestedPaths(null!, validKeys, validPaths))
                .Throws<ArgumentNullException>()
                .Because("null destination should throw ArgumentNullException for multiple keys overload");

            await Assert.That(() => context.InvokeAppendNestedPaths(validDestination, (string[])null!, validPaths))
                .Throws<ArgumentNullException>()
                .Because("null parentKeys should throw ArgumentNullException for multiple keys overload");

            await Assert.That(() => context.InvokeAppendNestedPaths(validDestination, validKeys, null!))
                .Throws<ArgumentNullException>()
                .Because("null nestedPaths should throw ArgumentNullException for multiple keys overload");
        }
    }

    /// <summary>
    /// Test stub inheriting from ConfigurationObjectContextBase to expose protected helpers.
    /// </summary>
    private sealed class TestContext : ConfigurationObjectContextBase<object>
    {
        /// <inheritdoc />
        public override IReadOnlySet<string[]> GetReadableConfigurationPaths()
        {
            return new HashSet<string[]>(new TestPathComparer());
        }

        /// <inheritdoc />
        public override IReadOnlySet<string[]> GetWritableConfigurationPaths()
        {
            return new HashSet<string[]>(new TestPathComparer());
        }

        /// <inheritdoc />
        public override IReadOnlyDictionary<string, object?> DumpConfigurationObject(
            object instance,
            bool considerWritablePropertiesOnly = true,
            params string[] parentKeys)
        {
            return new Dictionary<string, object?>(StringComparer.Ordinal);
        }

        /// <summary>Invokes protected AppendNestedPaths overload with single parent key.</summary>
        /// <param name="destination">The destination set.</param>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="nestedPaths">The nested paths.</param>
        public void InvokeAppendNestedPaths(
            ISet<string[]> destination,
            string parentKey,
            IEnumerable<string[]> nestedPaths)
        {
            AppendNestedPaths(destination, parentKey, nestedPaths);
        }

        /// <summary>Invokes protected AppendNestedPaths overload with parent keys array.</summary>
        /// <param name="destination">The destination set.</param>
        /// <param name="parentKeys">The parent keys.</param>
        /// <param name="nestedPaths">The nested paths.</param>
        public void InvokeAppendNestedPaths(
            ISet<string[]> destination,
            string[] parentKeys,
            IEnumerable<string[]> nestedPaths)
        {
            AppendNestedPaths(destination, parentKeys, nestedPaths);
        }
    }

    /// <summary>Path comparer for test sets.</summary>
    private sealed class TestPathComparer : IEqualityComparer<string[]>
    {
        /// <inheritdoc />
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

            for (int i = 0; i < left.Length; i++)
            {
                if (!StringComparer.Ordinal.Equals(left[i], right[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public int GetHashCode(string[] obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            unchecked
            {
                int hash = 17;
                foreach (string item in obj)
                {
                    hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(item);
                }

                return hash;
            }
        }
    }
}
// AI GENERATED END
