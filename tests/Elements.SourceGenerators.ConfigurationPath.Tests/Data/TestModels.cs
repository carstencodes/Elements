// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

#pragma warning disable MA0048 // File name must match type name
#pragma warning disable S2094 // Classes should not be empty
#pragma warning disable CA1812 // Internal class is never instantiated

using System.Collections.Generic;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests.Data;

// AI GENERATED START - model: gemini-3.7-flash
internal sealed class TestRootModel
{
    public TestChildModel Child { get; set; } = new();

    public string ReadOnlyName { get; } = "read-only";

    public int ReadOnlyScore { get; } = 9;

    public string? NullableName { get; set; }

    public int? NullableScore { get; set; }

    public Dictionary<TestKeyModel, TestChildModel> Values { get; } = new();

    public List<TestChildModel> Children { get; } = new();

    public int[] Scores { get; } = [3, 4];

    public byte[] Blob { get; } = [5, 6];

    public TestBoxModel<TestChildModel> Box { get; } = new();

    public List<TestListValueModel> Items { get; } = new();

    public TestArrayValueModel[] ArrayValues { get; } = [];
}

internal sealed class TestChildModel
{
    public string Name { get; set; } = string.Empty;

    public TestRootModel? Parent { get; set; }
}

internal sealed class TestListValueModel
{
}

internal sealed class TestArrayValueModel
{
}

internal sealed class TestKeyModel
{
    public string Name { get; set; } = string.Empty;
}

internal sealed class TestBoxModel<T> where T : class, new()
{
    public T Value { get; set; } = new();
}

internal struct TestSettingsModel
{
    public string Name { get; set; }
}
// AI GENERATED END
