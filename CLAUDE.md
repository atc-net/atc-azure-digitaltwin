# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Azure Digital Twin library providing services for Azure Digital Twins management, DTDL model validation, and parsing. Part of the [atc-net](https://github.com/atc-net) organization. Published as two NuGet packages: `Atc.Azure.DigitalTwin` (library) and `atc-azure-digitaltwin` (CLI tool).

## Build & Test Commands

```bash
# Build
dotnet build -c Release

# Run unit tests (excludes integration tests)
dotnet test -c Release --filter "Category!=Integration"

# Clean and restore
dotnet clean -c Release && dotnet nuget locals all --clear
dotnet restore

# Run the CLI tool locally
dotnet run --project src/Atc.Azure.DigitalTwin.CLI -- --help
```

## Solution Structure

- **`src/Atc.Azure.DigitalTwin/`** — Core library (NuGet package)
- **`src/Atc.Azure.DigitalTwin.CLI/`** — .NET global tool, packable via `PackAsTool`, assembly name `atc-azure-digitaltwin`
- **`test/Atc.Azure.DigitalTwin.Tests/`** — Unit tests (xUnit, AutoFixture, NSubstitute)
- **`sample/Atc.Azure.DigitalTwin.Console.Sample/`** — Example console app with DTDL models in `sample/models/`

Solution uses `.slnx` format (not `.sln`).

## Architecture

### Core Service Interfaces

- **`IDigitalTwinService`** — CRUD for models, twins, relationships, and event routes; query execution with pagination; JSON Patch updates. Implementation uses `DigitalTwinsClient` from the Azure SDK.
- **`IModelRepositoryService`** — Local DTDL model storage, loading from directories, validation, and cache management. Maintains internal model content list and dictionary.
- **`IDigitalTwinParser`** — Wraps `Microsoft.Azure.DigitalTwins.Parser.ModelParser` for JSON-to-DTDL conversion and validation.

All service implementations are `sealed partial` classes that use a companion `*LoggerMessages` partial class for source-generated logging.

### DI Registration

`ServiceCollectionExtensions.ConfigureDigitalTwinsClient()` registers services with `DigitalTwinOptions` (TenantId, InstanceUrl).

### Factory Classes

The CLI bypasses DI and uses factory classes directly: `DigitalTwinServiceFactory`, `ModelRepositoryServiceFactory`, `DigitalTwinParserFactory` (all in `src/Atc.Azure.DigitalTwin.CLI/Factories/`).

### CLI Command Structure

Built on Spectre.Console.Cli with `AsyncCommand<TSettings>` pattern. All commands accept `CancellationToken` from `ExecuteAsync` and propagate it through to service calls. The `DigitalTwinServiceFactory.Create` accepts `Uri instanceUrl`. Commands organized by domain:
- `model` — create (single/all), decommission, delete (single/all), get (single/all), validate
- `route` — create, delete, get (single/all)
- `twin` — count, create, delete (single/all/allbymodel), get, update
  - `twin relationship` — create, delete, get (single/all/incoming)

Settings inherit from `ConnectionBaseCommandSettings` (TenantId, AdtInstanceUrl) with domain-specific subclasses.

### Comparers

`Comparisons/` contains `IEqualityComparer<T>` implementations for `DTInterfaceInfo`, `DTPropertyInfo`, and `DTRelationshipInfo` — used for deduplication and comparison of DTDL model elements.

## Build Configuration

- **Target:** net10.0, C# 14.0, nullable enabled, implicit usings
- **Version:** 1.1.0 managed via release-please markers in `Directory.Build.props`; also uses Nerdbank.GitVersioning (`version.json`) for CI package versioning
- **Release builds:** warnings as errors
- **Analyzers** (all in root `Directory.Build.props`): Atc.Analyzer, AsyncFixer, Asyncify, Meziantou, SecurityCodeScan, StyleCop, SonarAnalyzer
- **Analysis level:** `latest-All` with `EnforceCodeStyleInBuild`
- **Source builds:** SourceLink enabled, deterministic builds in CI (`GITHUB_ACTIONS`), snupkg symbol packages

## Code Style

Enforced via `.editorconfig` and analyzers:
- File-scoped namespaces
- 4-space indentation for C# (2-space for JSON, YAML, XML project files)
- No `this.` qualifiers
- Expression-bodied members preferred
- Pattern matching and switch expressions preferred
- `CA2007` (ConfigureAwait) disabled
- `CA1860` (Avoid Enumerable.Any()) disabled

## Branch Strategy & CI

- `main` — active development, PR target
- `stable` — auto-merged from main by post-integration CI
- `release` — created by manual release workflow, publishes to NuGet.org

CI runs SonarCloud analysis on main pushes. Pre-release packages go to GitHub Package Registry; release packages go to NuGet.org.

## Key Dependencies

- `Atc` / `Atc.Console.Spectre` — ATC framework utilities and Spectre.Console integration
- `Azure.DigitalTwins.Core` — Azure Digital Twins SDK
- `Microsoft.Azure.DigitalTwins.Parser` — DTDL parsing
- `Azure.Identity` — Azure authentication
- `System.Linq.Async` — Async LINQ for pageable results
