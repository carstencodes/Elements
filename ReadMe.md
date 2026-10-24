![Continuous Integration](https://github.com/carstencodes/Elements/actions/workflows/ci.yaml/badge.svg?branch=main)
[![CodeQL](https://github.com/carstencodes/Elements/actions/workflows/github-code-scanning/codeql/badge.svg?branch=main)](https://github.com/carstencodes/Elements/actions/workflows/github-code-scanning)
![GitHub commits since latest release (branch)](https://img.shields.io/github/commits-since/carstencodes/Elements/latest/main?sort=semver&logo=github&color=blue)
![GitHub Tag](https://img.shields.io/github/v/tag/carstencodes/Elements)
![GitHub License](https://img.shields.io/github/license/carstencodes/Elements)
![GitHub Repo stars](https://img.shields.io/github/stars/carstencodes/Elements)
[![OpenSSF Best Practices](https://www.bestpractices.dev/projects/15631/badge)](https://www.bestpractices.dev/projects/15631)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/carstencodes/Elements/badge)](https://scorecard.dev/viewer/?uri=github.com/carstencodes/Elements)

# Elements

Elements is a .NET solution that extends the base platform with focused libraries, reusable infrastructure, and cross-platform interoperability helpers. It combines standard .NET libraries, source-generated APIs, and Rust-backed native assets for scenarios where the runtime does not yet provide the needed functionality or where a more efficient native implementation is preferable.

## Repository structure

The repository is organized around a small set of clearly separated concerns:

- `docs/` — documentation sources and DocFX content.
- `src/` — the main library projects that define the reusable API surface.
- `tests/` — automated tests for the library code.
- `gen/` — Roslyn source generators and related generator infrastructure.
- `build/` — shared MSBuild build recipes, props, and targets used across the solution.
- `deps/` — dependency management helpers and tooling around external package dependencies.
- `shared/` — shared assets used by multiple projects and tests.
- `resources/` — branding and project assets.

## Source projects in `src/`

The `src` directory contains the actual .NET libraries that make up the Elements ecosystem. These projects are intentionally small and focused, and they follow a layered approach where lower-level utilities are kept separate from higher-level integration helpers.

- `Elements.CommandLine`
  - Provides command-line abstractions and helper APIs built on top of `System.CommandLine`.
  - Supports composition of arguments and options, with source generation used to reduce boilerplate when building command models.

- `Elements.ComponentModel.Patterns`
  - Contains reusable behavioral patterns, such as the chain-of-responsibility pattern, that can be used to compose workflows and processing pipelines.

- `Elements.Extensions.Configuration.CommandLine`
  - Integrates command-line configuration with .NET configuration abstractions.
  - Useful when options should be supplied from command-line arguments and bound into configuration objects.

- `Elements.Extensions.Configuration.DependencyInjection`
  - Helps bind configuration values and options to services in dependency injection scenarios.

- `Elements.Extensions.DependencyInjection`
  - Adds usable service registration patterns and helpers for composite, decorator, keyed, and scoped service scenarios.

- `Elements.Functional`
  - Hosts functional programming utilities and small abstractions used to express operations in a more declarative style.

- `Elements.IO`
  - Provides general filesystem and path-related helpers and abstractions, especially around platform-neutral file operations.

- `Elements.IO.Compression`
  - Adds compression-related extensions and convenience APIs, with handling tailored to platform-specific needs.

- `Elements.IO.Posix`
  - Adds POSIX-specific IO functionality and platform interop support.
  - This project is wired to native assets built from Rust code.

- `Elements.IO.Posix.native-assets`
  - Contains the Rust-based native implementation that supports POSIX interop and low-level calls required by the managed layer.
  - This is a good example of the repository’s hybrid .NET + native strategy.

These projects are not a monolithic framework; instead, they are reusable building blocks that can be consumed individually depending on the requirements of a given application.

## Roslyn source generators in `gen/`

The `gen` folder contains Roslyn source generator projects used to generate code at compile time. This keeps repetitive code out of hand-written source files while preserving a clean, strongly-typed API surface.

- `Elements.SourceGenerators.Scriban`
  - Uses Scriban templates to generate C# source files from templates.
  - This is useful for reducing boilerplate and generating consistent code for APIs such as command-line parsing and other repetitive patterns.

- `Elements.SourceGenerators.ToolShim`
  - Provides the generator integration layer required for source generation to work cleanly in the build pipeline.

The source generators are intentionally separated from the main runtime libraries so that generated code can be treated as tooling infrastructure rather than part of the application runtime itself.

## Build recipes and dependency management

The repository uses central build configuration and task automation to keep packaging, validation, and dependency management consistent across projects.

- `build/` contains repo-specific build logic and MSBuild targets for tasks such as release note generation, SBOM creation, license checks, and other quality/release automation.
- Root-level `Directory.*.props` and `Directory.*.targets` files centralize common configuration and package versions.
- `deps/` contains dependency management helpers and configuration used to coordinate external tooling and Rust/Cargo-related integration.
- `Cargo.toml`, `Cargo.lock`, and related Rust files show that native dependencies are managed alongside the .NET build.

## Testing

The `tests` directory contains project-level automated tests, with each test project focused on a particular library area. This keeps verification close to the code it validates and allows targeted test execution for the relevant package or feature.

## Summary

Elements is a compact but extensible set of .NET libraries designed to fill gaps in the platform and provide robust, cross-platform building blocks. The project combines .NET assemblies, source-generated code, and native Rust-backed interop in a single repository structure that remains easy to navigate and maintain.

For more details, see the individual projects under `src/`, the generator tooling under `gen/`, and the build/dependency configuration in the root-level build files.
