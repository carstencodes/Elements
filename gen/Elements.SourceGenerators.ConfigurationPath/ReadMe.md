# Elements Configuration Path Source Generator

`HedgeCraft.Elements.SourceGenerators.ConfigurationPath` is a Roslyn source generator that
creates configuration metadata and flattening helpers for marked .NET model types. It is
intended for applications that bind or export configuration objects and need to know which
configuration paths their models expose.

## What it generates

The generator discovers classes and structs marked with `[ConfigurationObject]`. It emits:

- A `ConfigurationObjectAttribute` in the consumer project's `RootNamespace`. If that
  property is unavailable or invalid, it uses
  `HedgeCraft.Extensions.Configuration.UserSettings.Attributes`.
- An internal, sealed `ConfigurationContextOf...` class in the same namespace as each model
  that needs a context. Contexts derive from `ConfigurationObjectContextBase<T>`.
- `GetReadableConfigurationPaths()` and `GetWritableConfigurationPaths()` implementations.
  Each returns the model's paths as arrays of path segments; writable paths omit read-only
  scalar properties.
- `DumpConfigurationObject(...)`, which flattens an instance into a dictionary keyed by
  colon-separated configuration paths.
- Helpers for nested contexts and structural path comparison.

The generator follows public instance properties with public getters, including inherited
properties. It uses `ConfigurationKeyNameAttribute` values when present, traverses arrays and
supported generic enumerable or dictionary values, treats byte arrays as scalar values, and
handles nullable and generic models. Generated source is marked as auto-generated and uses
fully qualified framework and model types.

For example, given a model:

```csharp
using ExampleApp;
using Microsoft.Extensions.Configuration;

namespace ExampleApp.Configuration;

[ConfigurationObject]
public sealed class AppSettings
{
    [ConfigurationKeyName("environment")]
    public string Environment { get; set; } = "Development";

    [ConfigurationKeyName("database")]
    public DatabaseSettings Database { get; set; } = new();
}

public sealed class DatabaseSettings
{
    [ConfigurationKeyName("server")]
    public string Server { get; set; } = "localhost";
}
```

The generator emits an internal context named `ConfigurationContextOfAppSettings` (and a
context for `DatabaseSettings`). Its dump method produces keys such as `environment` and
`database:server`. The generated context also reports readable and writable paths as segment
arrays, such as `["database", "server"]`.

## Why use it

Keeping path discovery and flattening in generated code avoids hand-maintaining a second
description of each model. The emitted metadata stays aligned with the model's public
properties and configuration key attributes, and nested models and collections can be
traversed without reflection-based property discovery at runtime. The path sets can be used
to describe or validate supported configuration keys; the dump method provides a consistent
flattened representation of an object.

## Using it

Reference the source-generator package and its runtime abstractions in the consuming project.
Replace the version below with the version published for your application:

```xml
<ItemGroup>
  <PackageReference Include="HedgeCraft.Elements.SourceGenerators.ConfigurationPath"
                    Version="YOUR_VERSION" />
  <PackageReference Include="HedgeCraft.Elements.Extensions.Configuration.UserSettings"
                    Version="YOUR_VERSION" />
</ItemGroup>
```

Mark a model with `[ConfigurationObject]`. The generator emits that attribute into the
consumer project's `RootNamespace`; if it differs from the model's namespace, add a `using`
for the root namespace or fully qualify the attribute.

```csharp
using ExampleApp;
using Microsoft.Extensions.Configuration;

namespace ExampleApp.Configuration;

[ConfigurationObject]
public sealed class AppSettings
{
    [ConfigurationKeyName("environment")]
    public string Environment { get; set; } = "Development";
}
```

Generated contexts are `internal`, so use them from code in the same assembly. For example,
this retrieves paths and flattens an object:

```csharp
using System.Collections.Generic;
using ExampleApp.Configuration;

AppSettings settings = new();
ConfigurationContextOfAppSettings context = new();

IReadOnlySet<string[]> writablePaths = context.GetWritableConfigurationPaths();
IReadOnlyDictionary<string, object?> values = context.DumpConfigurationObject(settings);
```

`DumpConfigurationObject` includes writable scalar properties by default. Pass
`considerWritablePropertiesOnly: false` to include read-only scalar properties as well;
nested models and collections remain traversable so their child paths are not lost. Optional
parent path segments can be supplied with `parentKeys`.

The context's generated class name is deterministic: `ConfigurationContextOf` followed by
the model name and, for generic models, its type parameter names. Existing suitable context
implementations are reused when unambiguous.

## Generated code

The `examples` folder contains the complete model input and corresponding source generator
output for the example above:

<details>
<summary>Show the example input and generated output files</summary>

The input model is in [`AppSettings.cs.txt`](examples/AppSettings.cs.txt). The complete
generated output—including the marker attribute and both generated contexts—is in
[`AppSettings.g.cs.txt`](examples/AppSettings.g.cs.txt).

</details>

These `.txt` files are reference examples only; the project does not compile or include them.
The generated contexts implement the abstract path and dump members from
`ConfigurationObjectContextBase<T>`. The complete output shows their recursive dump
implementations, nested-context fields, path construction helpers, and structural comparers.
