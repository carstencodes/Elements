# Roslyn source generators

## General

This folder contains Roslyn source generator projects used to generate code at compile time. This keeps repetitive code out of hand-written source files while preserving a clean, strongly typed API surface.

## Source Generators

### `Elements.SourceGenerators.Scriban`

- Uses Scriban templates to generate C# source files from templates.
- This is useful for reducing boilerplate and generating consistent code for APIs such as command-line parsing and other repetitive patterns.

### `Elements.SourceGenerators.ToolShim`

- Provides the generator integration layer required for source generation to work cleanly in the build pipeline.

## Closing Words

The source generators are intentionally separated from the main runtime libraries so that generated code can be treated as tooling infrastructure rather than part of the application runtime itself.
