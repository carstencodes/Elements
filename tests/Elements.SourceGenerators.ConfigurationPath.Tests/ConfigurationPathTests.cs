// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisplayNameAttribute = TUnit.Core.DisplayNameAttribute;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests;

using ConfigurationPath = HedgeCraft.Elements.Extensions.Configuration.UserSettings.Keys.ConfigurationPath;

// AI GENERATED START - model: gemini-3.7-flash
/// <summary>Validates the value semantics and operations of <see cref="ConfigurationPath"/>.</summary>
[DisplayName("Configuration Path Tests")]
[Description("Validates ordinal structural equality, hashing, and path composition of ConfigurationPath.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Performance",
    "CA1822:Mark members as static",
    Justification = "TUnit test methods must be instance methods.")]
internal sealed class ConfigurationPathTests
{
    [Test]
    [DisplayName("Paths with identical segments are equal")]
    [Description("Verifies that distinct instances with matching segments are equal.")]
    public async Task PathsWithIdenticalSegmentsAreEqual()
    {
        ConfigurationPath left = new("a", "b");
        ConfigurationPath right = new("a", "b");

        await Assert.That(left == right)
            .IsTrue()
            .Because("paths with identical segments should be equal by value");
    }

    [Test]
    [DisplayName("Paths with identical segments share a hash code")]
    [Description("Verifies that equal paths produce the same hash code.")]
    public async Task PathsWithIdenticalSegmentsShareHashCode()
    {
        ConfigurationPath left = new("a", "b");
        ConfigurationPath right = new("a", "b");

        await Assert.That(left.GetHashCode())
            .IsEqualTo(right.GetHashCode())
            .Because("equal paths must produce equal hash codes");
    }

    [Test]
    [DisplayName("Paths with different segments are not equal")]
    [Description("Verifies that differing segments make paths unequal.")]
    public async Task PathsWithDifferentSegmentsAreNotEqual()
    {
        await Assert.That(new ConfigurationPath("a", "b") != new ConfigurationPath("a", "c"))
            .IsTrue()
            .Because("paths with differing segments should not be equal");
    }

    [Test]
    [DisplayName("Segment comparison is case sensitive")]
    [Description("Verifies that segments are compared ordinally.")]
    public async Task SegmentComparisonIsOrdinal()
    {
        await Assert.That(new ConfigurationPath("A").Equals(new ConfigurationPath("a")))
            .IsFalse()
            .Because("segment comparison should be ordinal and case sensitive");
    }

    [Test]
    [DisplayName("Default path equals an empty path")]
    [Description("Verifies that a default struct with null segments behaves like an empty path.")]
    public async Task DefaultPathEqualsEmptyPath()
    {
        await Assert.That(default(ConfigurationPath).Equals(new ConfigurationPath()))
            .IsTrue()
            .Because("a default path should be treated as an empty path without throwing");
    }

    [Test]
    [DisplayName("Append combines parent and child segments")]
    [Description("Verifies that Append concatenates parent path segments with each child path's segments.")]
    public async Task AppendCombinesParentAndChildSegmentsCorrectly()
    {
        ConfigurationPath parent = new("parent", "section");
        ConfigurationPath[] children =
        [
            new("child1"),
            new("sub", "child2")
        ];

        List<ConfigurationPath> appended = [.. parent.Append(children)];

        using (Assert.Multiple())
        {
            await Assert.That(appended.Count)
                .IsEqualTo(2)
                .Because("appending two child paths should produce two combined paths");

            await Assert.That(appended[0])
                .IsEqualTo(new ConfigurationPath("parent", "section", "child1"))
                .Because("first appended path should concatenate parent and first child segments");

            await Assert.That(appended[1])
                .IsEqualTo(new ConfigurationPath("parent", "section", "sub", "child2"))
                .Because("second appended path should concatenate parent and second child segments");
        }
    }

    [Test]
    [DisplayName("Append throws ArgumentNullException on null child paths")]
    [Description("Verifies that Append throws ArgumentNullException when childPaths is null.")]
    public async Task AppendThrowsOnNullChildPaths()
    {
        ConfigurationPath path = new("parent");

        await Assert.That(() => path.Append(null!))
            .Throws<ArgumentNullException>()
            .Because("passing null child paths to Append must throw ArgumentNullException");
    }
}
// AI GENERATED END
