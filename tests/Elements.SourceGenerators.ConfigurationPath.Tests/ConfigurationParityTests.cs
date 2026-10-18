// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests.Data;
using DisplayNameAttribute = TUnit.Core.DisplayNameAttribute;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath.Tests;

// AI GENERATED START - model: Claude Sonnet 5.5
[DisplayName("Configuration Binder Parity Tests")]
[Description(
    "Verifies that the values dumped by generated contexts equal what ConfigurationBinder bound from the same configuration.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Performance",
    "CA1822:Mark members as static",
    Justification = "TUnit test methods must be instance methods.")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Major Code Smell",
    "S2325:Methods and properties that don't access instance data should be static",
    Justification = "TUnit test methods must be instance methods.")]
internal sealed class ConfigurationParityTests
{
    private static readonly Dictionary<string, string?> FullDeepConfiguration = new(StringComparer.Ordinal)
    {
        ["Title"] = "root",
        ["l1:Name"] = "one",
        ["l1:Second:Number"] = "2",
        ["l1:Second:third:Flag"] = "true",
        ["l1:Second:third:Fourth:Ratio"] = "0.5",
        ["l1:Second:third:Fourth:Fifth:Name"] = "five",
        ["l1:Second:third:Fourth:Fifth:Id"] = "3f2504e0-4f89-41d3-9a0c-0305e82c3301",
    };

    [Test]
    [DisplayName("Smoke model dump equals binder result")]
    [Description(
        "Binds a renamed nested object and nullable scalars and verifies the generated dump reports exactly them.")]
    public async Task SmokeModelDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ConfigurationParityHarness.Execute(
            ParityInput.Smoke,
            ParityInput.AttributeNamespace,
            "Parity.Settings.SmokeRoot",
            new Dictionary<string, string?>(StringComparer.Ordinal)
            {
                ["renamed:Name"] = "child",
                ["Title"] = "title",
                ["Count"] = "3",
            });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Five level nesting dump equals binder result")]
    [Description(
        "Binds scalars on every level of a five level model including key name overrides and verifies the dump.")]
    public async Task FiveLevelNestingDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteDeep(FullDeepConfiguration);

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Five level nesting writable paths cover every bound key")]
    [Description("Verifies that each key bound by the binder in a five level model is a generated writable path.")]
    public async Task FiveLevelNestingWritablePathsCoverBoundKeys()
    {
        ParityOutcome outcome = ExecuteDeep(FullDeepConfiguration);

        await Assert.That(outcome.UncoveredByPaths)
            .IsEmpty()
            .Because("every bound deep key should be listed by GetWritableConfigurationPaths");
    }

    [Test]
    [DisplayName("Binder keys match case insensitively")]
    [Description("Binds keys with different casing and verifies the dump still reports every value.")]
    public async Task BinderKeysMatchCaseInsensitively()
    {
        ParityOutcome outcome = ExecuteDeep(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["TITLE"] = "root",
            ["L1:name"] = "one",
            ["l1:SECOND:THIRD:fourth:FIFTH:name"] = "five",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Get-only nested object is bound like the binder does")]
    [Description("Binds values below a get-only nested object property and verifies the dump reports them.")]
    public async Task GetOnlyNestedObjectIsBound()
    {
        ParityOutcome outcome = ExecuteDeep(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["ReadOnlyFirst:Name"] = "read-only first",
            ["ReadOnlyFirst:Second:third:Fourth:Fifth:Name"] = "deep",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Null nested object is created by binder and dumped")]
    [Description("Binds values below a nested object that starts as null and verifies the dump reports them.")]
    public async Task NullNestedObjectIsCreatedAndDumped()
    {
        ParityOutcome outcome = ExecuteDeep(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["OptionalFirst:Name"] = "optional",
            ["OptionalFirst:Second:Number"] = "9",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Absent nested objects are not dumped")]
    [Description("Binds nothing and verifies the dump reports no value for nested objects or scalars.")]
    public async Task AbsentConfigurationDumpsNoValues()
    {
        ParityOutcome outcome = ExecuteDeep(new Dictionary<string, string?>(StringComparer.Ordinal));

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Self referencing model terminates and equals binder result")]
    [Description("Binds a value through a cyclic Parent reference and verifies the dump terminates and matches.")]
    public async Task SelfReferencingModelTerminatesAndMatchesBinder()
    {
        ParityOutcome outcome = ExecuteDeep(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["l1:Parent:Title"] = "cyclic",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection List of int dump equals binder result")]
    [Description("Binds the IntList collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionIntListDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["IntList:0"] = "1",
            ["IntList:1"] = "2",
            ["IntList:2"] = "3",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection int array dump equals binder result")]
    [Description("Binds the IntArray collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionIntArrayDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["IntArray:0"] = "4",
            ["IntArray:1"] = "5",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection IList of string dump equals binder result")]
    [Description("Binds the StringIList collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionStringIListDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringIList:0"] = "a",
            ["StringIList:1"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection ICollection of string dump equals binder result")]
    [Description("Binds the StringICollection collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionStringICollectionDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringICollection:0"] = "a",
            ["StringICollection:1"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection IEnumerable of string dump equals binder result")]
    [Description("Binds the StringEnumerable collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionStringEnumerableDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringEnumerable:0"] = "a",
            ["StringEnumerable:1"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection IReadOnlyList of string dump equals binder result")]
    [Description("Binds the StringReadOnlyList collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionStringReadOnlyListDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringReadOnlyList:0"] = "a",
            ["StringReadOnlyList:1"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection IReadOnlyCollection of string dump equals binder result")]
    [Description("Binds the StringReadOnlyCollection collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionStringReadOnlyCollectionDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringReadOnlyCollection:0"] = "a",
            ["StringReadOnlyCollection:1"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection ISet of string dump equals binder result")]
    [Description("Binds the StringISet collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionStringISetDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringISet:0"] = "a",
            ["StringISet:1"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection HashSet of string dump equals binder result")]
    [Description("Binds the StringHashSet collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionStringHashSetDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringHashSet:0"] = "a",
            ["StringHashSet:1"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection list of lists dump equals binder result")]
    [Description("Binds the JaggedList collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionJaggedListDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["JaggedList:0:0"] = "1",
            ["JaggedList:0:1"] = "2",
            ["JaggedList:1:0"] = "3",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection jagged array dump equals binder result")]
    [Description("Binds the JaggedArray collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionJaggedArrayDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["JaggedArray:0:0"] = "1",
            ["JaggedArray:0:1"] = "2",
            ["JaggedArray:1:0"] = "3",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection list of objects dump equals binder result")]
    [Description("Binds the Items collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionItemsDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["Items:0:Name"] = "a",
            ["Items:0:Weight"] = "1",
            ["Items:1:Name"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection array of objects dump equals binder result")]
    [Description("Binds the ItemArray collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionItemArrayDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["ItemArray:0:Name"] = "a",
            ["ItemArray:1:Weight"] = "2",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection IList of objects dump equals binder result")]
    [Description("Binds the ItemIList collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionItemIListDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["ItemIList:0:Name"] = "a",
            ["ItemIList:1:Name"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection list of enums dump equals binder result")]
    [Description("Binds the Colors collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionColorsDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["Colors:0"] = "Red",
            ["Colors:1"] = "Blue",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection list of guids dump equals binder result")]
    [Description("Binds the Guids collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionGuidsDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["Guids:0"] = "3f2504e0-4f89-41d3-9a0c-0305e82c3301",
            ["Guids:1"] = "3f2504e0-4f89-41d3-9a0c-0305e82c3302",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection list of nullable ints dump equals binder result")]
    [Description("Binds the NullableInts collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionNullableIntsDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["NullableInts:0"] = "1",
            ["NullableInts:1"] = "2",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection list of decimals dump equals binder result")]
    [Description("Binds the Decimals collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionDecimalsDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["Decimals:0"] = "1.5",
            ["Decimals:1"] = "2.25",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection list of timespans dump equals binder result")]
    [Description("Binds the Spans collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionSpansDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["Spans:0"] = "00:01:00",
            ["Spans:1"] = "01:00:00",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection get-only list dump equals binder result")]
    [Description("Binds the GetOnlyList collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionGetOnlyListDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["GetOnlyList:0"] = "a",
            ["GetOnlyList:1"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Collection get-only list of objects dump equals binder result")]
    [Description("Binds the GetOnlyItems collection property and verifies the generated dump reports the same entries.")]
    public async Task CollectionGetOnlyItemsDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["GetOnlyItems:0:Name"] = "a",
            ["GetOnlyItems:1:Name"] = "b",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Documented divergence: binder does not populate ImmutableArray")]
    [Description(
        "ConfigurationBinder cannot bind ImmutableArray<T>, so the dump of the untouched default value is empty and must not throw.")]
    public async Task ImmutableArrayIsNotPopulatedByBinderAndDumpsNothing()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringImmutableArray:0"] = "a",
            ["StringImmutableArray:1"] = "b",
        });

        using (Assert.Multiple())
        {
            await Assert.That(outcome.CompilationErrors)
                .IsEqualTo(string.Empty)
                .Because("the generated assembly should compile without errors");

            await Assert.That(outcome.Missing)
                .IsEquivalentTo(["StringImmutableArray:0", "StringImmutableArray:1"])
                .Because("the binder leaves the default ImmutableArray untouched, so nothing is dumped");

            await Assert.That(outcome.Extra)
                .IsEmpty()
                .Because("a default ImmutableArray should not produce any dumped key");
        }
    }

    [Test]
    [DisplayName("Documented divergence: byte array is dumped as scalar while binder indexes it")]
    [Description(
        "The generator deliberately treats byte[] as one binary scalar, whereas the binder binds it from indexed keys.")]
    public async Task ByteArrayIsDumpedAsScalarWhileBinderUsesIndexedKeys()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["Blob:0"] = "1",
            ["Blob:1"] = "2",
        });

        using (Assert.Multiple())
        {
            await Assert.That(outcome.Missing)
                .IsEquivalentTo(["Blob:0", "Blob:1"])
                .Because("the generator does not emit indexed keys for byte arrays");

            await Assert.That(outcome.Extra)
                .IsEquivalentTo(["Blob"])
                .Because("the generator dumps the whole byte array under the property key");
        }
    }

    [Test]
    [DisplayName("Empty collections dump nothing")]
    [Description("Binds nothing and verifies empty collections produce no dump values.")]
    public async Task EmptyCollectionsDumpNothing()
    {
        ParityOutcome outcome = ExecuteCollections(new Dictionary<string, string?>(StringComparer.Ordinal));

        await AssertParity(outcome).ConfigureAwait(false);
    }

    private static ParityOutcome ExecuteCollections(IReadOnlyDictionary<string, string?> configuration)
    {
        return ConfigurationParityHarness.Execute(
            ParityInput.CollectionTypes,
            ParityInput.AttributeNamespace,
            "Parity.Settings.CollectionRoot",
            configuration);
    }

    [Test]
    [DisplayName("Mapping string to int dump equals binder result")]
    [Description("Binds the StringIntMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingStringIntMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringIntMap:a"] = "1",
            ["StringIntMap:b"] = "2",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping IDictionary string to string dump equals binder result")]
    [Description("Binds the StringIDictionary mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingStringIDictionaryDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringIDictionary:a"] = "x",
            ["StringIDictionary:b"] = "y",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping IReadOnlyDictionary string to string dump equals binder result")]
    [Description("Binds the StringReadOnlyDictionary mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingStringReadOnlyDictionaryDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["StringReadOnlyDictionary:a"] = "x",
            ["StringReadOnlyDictionary:b"] = "y",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping int key dump equals binder result")]
    [Description("Binds the IntKeyMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingIntKeyMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["IntKeyMap:1"] = "x",
            ["IntKeyMap:2"] = "y",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping enum key dump equals binder result")]
    [Description("Binds the EnumKeyMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingEnumKeyMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["EnumKeyMap:Red"] = "x",
            ["EnumKeyMap:Blue"] = "y",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping enum value dump equals binder result")]
    [Description("Binds the EnumValueMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingEnumValueMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["EnumValueMap:a"] = "Red",
            ["EnumValueMap:b"] = "Blue",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping guid value dump equals binder result")]
    [Description("Binds the GuidValueMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingGuidValueMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["GuidValueMap:a"] = "3f2504e0-4f89-41d3-9a0c-0305e82c3301",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping nullable int value dump equals binder result")]
    [Description("Binds the NullableValueMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingNullableValueMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["NullableValueMap:a"] = "1",
            ["NullableValueMap:b"] = "2",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping string to object dump equals binder result")]
    [Description("Binds the ObjectMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingObjectMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["ObjectMap:a:Name"] = "x",
            ["ObjectMap:a:Weight"] = "1",
            ["ObjectMap:b:Name"] = "y",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping IDictionary string to object dump equals binder result")]
    [Description("Binds the ObjectIDictionary mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingObjectIDictionaryDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["ObjectIDictionary:a:Name"] = "x",
            ["ObjectIDictionary:b:Name"] = "y",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping IReadOnlyDictionary string to object dump equals binder result")]
    [Description("Binds the ObjectReadOnlyDictionary mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingObjectReadOnlyDictionaryDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["ObjectReadOnlyDictionary:a:Name"] = "x",
            ["ObjectReadOnlyDictionary:b:Name"] = "y",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping string to list dump equals binder result")]
    [Description("Binds the ListMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingListMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["ListMap:a:0"] = "1",
            ["ListMap:a:1"] = "2",
            ["ListMap:b:0"] = "3",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping string to array dump equals binder result")]
    [Description("Binds the ArrayMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingArrayMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["ArrayMap:a:0"] = "x",
            ["ArrayMap:a:1"] = "y",
            ["ArrayMap:b:0"] = "z",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping string to list of objects dump equals binder result")]
    [Description("Binds the ObjectListMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingObjectListMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["ObjectListMap:a:0:Name"] = "x",
            ["ObjectListMap:a:1:Name"] = "y",
            ["ObjectListMap:b:0:Weight"] = "3",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping string to dictionary dump equals binder result")]
    [Description("Binds the NestedMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingNestedMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["NestedMap:a:x"] = "1",
            ["NestedMap:a:y"] = "2",
            ["NestedMap:b:z"] = "3",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping string to dictionary of objects dump equals binder result")]
    [Description("Binds the NestedObjectMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingNestedObjectMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["NestedObjectMap:a:x:Name"] = "1",
            ["NestedObjectMap:b:z:Weight"] = "3",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping get-only dump equals binder result")]
    [Description("Binds the GetOnlyMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingGetOnlyMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["GetOnlyMap:a"] = "1",
            ["GetOnlyMap:b"] = "2",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping get-only object dump equals binder result")]
    [Description("Binds the GetOnlyObjectMap mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingGetOnlyObjectMapDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["GetOnlyObjectMap:a:Name"] = "x",
            ["GetOnlyObjectMap:b:Name"] = "y",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Mapping case-insensitive dump equals binder result")]
    [Description("Binds the CaseInsensitiveKeys mapping case and verifies the generated dump reports the same entries.")]
    public async Task MappingCaseInsensitiveKeysDumpEqualsBinderResult()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["stringintmap:A"] = "1",
            ["STRINGINTMAP:b"] = "2",
        });

        await AssertParity(outcome).ConfigureAwait(false);
    }

    [Test]
    [DisplayName("Empty mappings dump nothing")]
    [Description("Binds nothing and verifies empty mappings produce no dump values.")]
    public async Task EmptyMappingsDumpNothing()
    {
        ParityOutcome outcome = ExecuteMappings(new Dictionary<string, string?>(StringComparer.Ordinal));

        await AssertParity(outcome).ConfigureAwait(false);
    }

    private static ParityOutcome ExecuteMappings(IReadOnlyDictionary<string, string?> configuration)
    {
        return ConfigurationParityHarness.Execute(
            ParityInput.MappingTypes,
            ParityInput.AttributeNamespace,
            "Parity.Settings.MappingRoot",
            configuration);
    }

    private static ParityOutcome ExecuteDeep(IReadOnlyDictionary<string, string?> configuration)
    {
        return ConfigurationParityHarness.Execute(
            ParityInput.DeepNesting,
            ParityInput.AttributeNamespace,
            "Parity.Settings.DeepRoot",
            configuration);
    }

    private static async Task AssertParity(ParityOutcome outcome)
    {
        using (Assert.Multiple())
        {
            await Assert.That(outcome.CompilationErrors)
                .IsEqualTo(string.Empty)
                .Because("the generated assembly should compile without errors");

            await Assert.That(outcome.Missing)
                .IsEmpty()
                .Because("every key bound by the binder should be present in the generated dump");

            await Assert.That(outcome.Extra)
                .IsEmpty()
                .Because("the generated dump should not report values the binder did not bind");

            await Assert.That(outcome.Mismatched)
                .IsEmpty()
                .Because("dumped values should equal the configured values");
        }
    }
}
// AI GENERATED END
