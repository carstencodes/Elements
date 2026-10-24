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

The repository is designed to be approachable for both users of the libraries and contributors working on the codebase. It keeps the runtime libraries, generator tooling, build automation, and native interop concerns clearly separated so that each part can be understood and evolved independently.

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

The `src` directory contains the .NET libraries that make up the Elements ecosystem. These projects are intentionally small and focused, and they follow a layered approach where lower-level utilities are kept separate from higher-level integration helpers.

- `Elements.CommandLine`
  - Provides command-line abstractions and helper APIs built on top of `System.CommandLine`.
  - Supports composition of arguments and options, with source generation used to reduce boilerplate when building command models.

- `Elements.ComponentModel.Patterns`
  - Contains reusable behavioral patterns, such as the chain-of-responsibility pattern, which can be used to compose workflows and processing pipelines.

- `Elements.Extensions.Configuration.CommandLine`
  - Integrates command-line configuration with .NET configuration abstractions.
  - Useful when options should be supplied from command-line arguments and bound into configuration objects.

- `Elements.Extensions.Configuration.DependencyInjection`
  - Helps bind configuration values and options to services in dependency injection scenarios.

- `Elements.Extensions.DependencyInjection`
  - Adds useful service registration patterns and helpers for composite, decorator, keyed, and scoped service scenarios.

- `Elements.Functional`
  - Hosts functional programming utilities and small abstractions used to express operations in a more declarative style.

- `Elements.IO`
  - Provides general filesystem and path-related helpers and abstractions, especially around platform-neutral file operations.

- `Elements.IO.Compression`
  - Adds compression-related extensions and convenience APIs, with handling tailored to platform-specific needs.

- `Elements.IO.Posix`
  - Adds POSIX-specific I/O functionality and platform interop support.
  - This project is backed by native assets built from Rust code.

- `Elements.IO.Posix.native-assets`
  - Contains the Rust-based native implementation that supports POSIX interop and low-level calls required by the managed layer.
  - This is a clear example of the repository’s hybrid .NET + native strategy.

These projects are not a monolithic framework; instead, they are reusable building blocks that can be consumed individually depending on the needs of a given application.

## Roslyn source generators in `gen/`

The `gen` folder contains Roslyn source generator projects used to generate code at compile time. This keeps repetitive code out of hand-written source files while preserving a clean, strongly typed API surface.

- `Elements.SourceGenerators.Scriban`
  - Uses Scriban templates to generate C# source files from templates.
  - This is useful for reducing boilerplate and generating consistent code for APIs such as command-line parsing and other repetitive patterns.

- `Elements.SourceGenerators.ToolShim`
  - Provides the generator integration layer required for source generation to work cleanly in the build pipeline.

The source generators are intentionally separated from the main runtime libraries so that generated code can be treated as tooling infrastructure rather than part of the application runtime itself.

## Build recipes and dependency management

The repository uses central build configuration and task automation to keep packaging, validation, and dependency management consistent across projects.

- `build/` contains repository-specific build logic and MSBuild targets for tasks such as release note generation, SBOM creation, license checks, and other quality and release automation.
- Root-level `Directory.*.props` and `Directory.*.targets` files centralize common configuration and package versions.
- `deps/` contains dependency management helpers and configuration used to coordinate external tooling and Rust/Cargo-related integration.
- `Cargo.toml`, `Cargo.lock`, and the related Rust files show that native dependencies are managed alongside the .NET build.

## Testing and quality gates

The `tests` directory contains project-level automated tests, with each test project focused on a particular library area. This keeps verification close to the code it validates and allows targeted execution for the relevant package or feature.

Quality checks are enforced in CI and locally:

- `.editorconfig` defines formatting and naming conventions, line rules, and code-style expectations across the repository.
- `.pre-commit-config.yaml` runs pre-commit validation such as conventional commit checks, `dotnet format`, `rustfmt`, `cargo clippy`, and `editorconfig` verification.
- `.github/workflows/codeql.yml` runs CodeQL-based Security and Quality (SAST) scanning for Actions, C#, and Rust code.
- `.github/workflows/ci.yaml` includes the build pipeline, test execution, report generation, and code coverage reporting using `dotnet test` and ReportGenerator.

## Contributing

Contributions are welcome. To keep the project consistent and maintainable, please follow the repository conventions and validation steps before opening a pull request.

1. Fork the repository and create a feature branch.
2. Keep changes focused and aligned with the existing project structure.
3. Follow the repository formatting and naming rules defined in `.editorconfig`.
4. Run the relevant local checks before submitting changes:
   - `pre-commit run --all-files` (or the equivalent checks defined in `.pre-commit-config.yaml`)
   - `dotnet test` for the affected projects
   - any Rust validation required for native asset changes
5. Ensure that new or changed behavior is covered by tests where appropriate.
6. Submit a pull request with a clear description of the change, motivation, and validation performed.

The CI pipeline is designed to validate build correctness, static analysis, code coverage, and packaging quality. If you see a failing check, review the relevant workflow and fix the issue before merging.

## Summary

Elements is a compact but extensible set of .NET libraries designed to fill gaps in the platform and provide robust, cross-platform building blocks. The project combines .NET assemblies, source-generated code, and native Rust-backed interop in a single repository structure that remains easy to navigate and maintain.

For more details, see the individual projects under `src/`, the generator tooling under `gen/`, the build and dependency configuration in the root directory, and the automation in `.github/workflows/`.
