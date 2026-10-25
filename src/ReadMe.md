# Libraries

This directory contains the .NET libraries that make up the Elements ecosystem. These projects are intentionally small and focused, and they follow a layered approach where lower-level utilities are kept separate from higher-level integration helpers.

## General

Consider the libraries a set of closed functionality. They **MUST NOT** depend on each other to avoid a dependency hell. As this project aims to extend the .NET basic libraries, the number of additional third party dependencies **SHOULD** be kept small.

## Libraries

### `Elements.CommandLine`

- Provides command-line abstractions and helper APIs built on top of `System.CommandLine`.
- Supports composition of arguments and options, with source generation used to reduce boilerplate when building command models.

### `Elements.ComponentModel.Patterns`

- Contains reusable behavioral patterns, such as the chain-of-responsibility pattern, which can be used to compose workflows and processing pipelines.

### `Elements.Extensions.Configuration.CommandLine`

- Integrates command-line configuration with .NET configuration abstractions.
- Useful when options should be supplied from command-line arguments and bound into configuration objects.

### `Elements.Extensions.Configuration.DependencyInjection`

- Helps bind configuration values and options to services in dependency injection scenarios.

### `Elements.Extensions.DependencyInjection`

- Adds useful service registration patterns and helpers for composite, decorator, keyed, and scoped service scenarios.

### `Elements.Functional`

- Hosts functional programming utilities and small abstractions used to express operations in a more declarative style.

### `Elements.IO`

- Provides general filesystem and path-related helpers and abstractions, especially around platform-neutral file operations.

### `Elements.IO.Compression`

- Adds compression-related extensions and convenience APIs, with handling tailored to platform-specific needs.

### `Elements.IO.Posix`

- Adds POSIX-specific I/O functionality and platform interop support.
- This project is backed by native assets built from Rust code.

####  `Elements.IO.Posix.native-assets`

- Contains the Rust-based native implementation that supports POSIX interop and low-level calls required by the managed layer.
- This is a clear example of the repository’s hybrid .NET + native strategy.

## Closing words

These projects are not a monolithic framework; instead, they are reusable building blocks that can be consumed individually depending on the needs of a given application.
