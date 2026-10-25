[![Continuous Integration](https://github.com/carstencodes/Elements/actions/workflows/ci.yaml/badge.svg?branch=main)](https://github.com/carstencodes/Elements/actions/workflows/ci.yaml)
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

## Build recipes and dependency management

The repository uses central build configuration and task automation to keep packaging, validation, and dependency management consistent across projects.

- `build/` contains repository-specific build logic and MSBuild targets for tasks such as release note generation, SBOM creation, license checks, and other quality and release automation.
- Root-level `Directory.*.props` and `Directory.*.targets` files centralize common configuration and package versions.
- `deps/` contains dependency management helpers and configuration used to coordinate external tooling and Rust/Cargo-related integration.
- `Cargo.toml`, `Cargo.lock`, and the related Rust files show that native dependencies are managed alongside the .NET build.

## Quality gates

Quality checks are enforced in CI and locally:

- `.editorconfig` defines formatting and naming conventions, line rules, and code-style expectations across the repository.
- `.pre-commit-config.yaml` runs pre-commit validation such as conventional commit checks, `dotnet format`, `rustfmt`, `cargo clippy`, and `editorconfig` verification.
- `.github/workflows/codeql.yml` runs CodeQL-based Security and Quality (SAST) scanning for Actions, C#, and Rust code.
- `.github/workflows/ci.yaml` includes the build pipeline, test execution, report generation, and code coverage reporting using `dotnet test` and ReportGenerator.

## Contributing

Contributions are welcome. To keep the project consistent and maintainable, please follow the repository conventions and validation steps before opening a pull request.

1. Fork the repository and create a feature branch.
2. Keep changes focused and aligned with the existing project structure.
    - Do not create dependencies between the projects.
    - A source generator **MUST NOT** reference other source generators.
    - A source generator **MUST NOT** reference libraries.
    - A library **MAY** reference source generators.
    - A library **MUST NOT** reference other libraries.
    - A test **MUST** reference at least one source generator or library.
3. Follow the repository formatting and naming rules defined in `.editorconfig`.
4. Run the relevant local checks before submitting changes:
    - `pre-commit run --all-files` (or the equivalent checks defined in `.pre-commit-config.yaml`)
    - `dotnet test` for the affected projects
    - any Rust validation required for native asset changes
5. Please follow [conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) using header and body.
    - In case of AI assisted commits please add the `Co-Authored-By` or `Assisted-By` trailer.
6. Keep the [changelog](./ChangeLog.md) up to date.
7. Ensure that new or changed behavior is covered by tests where appropriate.
8. Submit a pull request with a clear description of the change, motivation, and validation performed.

The CI pipeline is designed to validate build correctness, static analysis, code coverage, and packaging quality. If you see a failing check, review the relevant workflow and fix the issue before merging.

## Summary

Elements is a compact but extensible set of .NET libraries designed to fill gaps in the platform and provide robust, cross-platform building blocks. The project combines .NET assemblies, source-generated code, and native Rust-backed interop in a single repository structure that remains easy to navigate and maintain.

For more details, see the individual projects under `src/`, the generator tooling under `gen/`, the build and dependency configuration in the root directory, and the automation in `.github/workflows/`.
