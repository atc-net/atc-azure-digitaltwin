[![NuGet Version](https://img.shields.io/nuget/v/atc.azure.digitaltwin.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/atc.azure.digitaltwin)

# Atc.Azure.DigitalTwin

A .NET library and CLI tool for managing Azure Digital Twins — providing DTDL model validation, twin lifecycle management, relationship handling, and event route configuration.

- 🏗️ **Model Management** — create, retrieve, decommission, and delete DTDL models with full validation
- 🔗 **Twin & Relationship CRUD** — create, query, update, and delete digital twins and their relationships
- 📡 **Event Routes** — create, delete, and list event routes for Digital Twin endpoints
- 🔍 **Query Engine** — execute ADT queries with pagination support
- 📋 **DTDL Parser** — validate and parse JSON models into Digital Twin interface definitions
- 🖥️ **Cross-Platform CLI** — global .NET tool for command-line management of Azure Digital Twins
- 💉 **DI Integration** — `ServiceCollection` extensions for seamless dependency injection setup

## 📋 Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

## 🚀 Getting Started

### Installation

Install the NuGet package:

```bash
dotnet add package Atc.Azure.DigitalTwin
```

### Basic Usage

```csharp
var digitalTwinService = serviceProvider.GetRequiredService<IDigitalTwinService>();

// Retrieve a twin
var twin = await digitalTwinService.GetTwin("my-twin-id");

// Create a relationship
var (succeeded, errorMessage) = await digitalTwinService.CreateRelationship(
    "source-twin-id",
    "target-twin-id",
    "relatesTo");
```

### Configuring with ServiceCollection Extensions

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton(new DigitalTwinOptions
    {
        TenantId = "your_tenant_id",
        InstanceUrl = "your_instance_url",
    });

    services.ConfigureDigitalTwinsClient();
}
```

## ✨ Features

### 🏗️ IDigitalTwinService

Comprehensive CRUD operations for models, twins, relationships, and event routes within an Azure Digital Twins instance.

```csharp
var digitalTwinService = serviceProvider.GetRequiredService<IDigitalTwinService>();

// Query twins
var twins = await digitalTwinService.GetTwins("SELECT * FROM DIGITALTWINS", cancellationToken);

// Update a twin with JSON Patch
var patchDocument = new JsonPatchDocument();
patchDocument.AppendReplace("/temperature", 25.0);
await digitalTwinService.UpdateTwin("my-twin-id", patchDocument, cancellationToken: cancellationToken);

// Manage event routes
await digitalTwinService.CreateOrReplaceEventRoute(
    "my-route",
    "my-endpoint",
    filter: "type = 'Microsoft.DigitalTwins.Twin.Update'",
    cancellationToken: cancellationToken);
```

### 📋 IModelRepositoryService

Local DTDL model storage, loading from directories, and validation.

```csharp
var modelRepositoryService = serviceProvider.GetRequiredService<IModelRepositoryService>();
var modelsPath = new DirectoryInfo("path/to/models");

var isValid = await modelRepositoryService.ValidateModels(modelsPath, cancellationToken);

if (isValid)
{
    await modelRepositoryService.LoadModelContent(modelsPath, cancellationToken);
    var models = modelRepositoryService.GetModels();
}
```

### 🔍 IDigitalTwinParser

Parses JSON DTDL models into Digital Twin interface definitions with validation.

```csharp
var parser = serviceProvider.GetRequiredService<IDigitalTwinParser>();
var (succeeded, interfaces) = await parser.Parse(jsonModels);

if (succeeded)
{
    foreach (var (dtmi, entity) in interfaces!)
    {
        Console.WriteLine($"Parsed: {dtmi}");
    }
}
```

## 🖥️ CLI Tool

[![NuGet Version](https://img.shields.io/nuget/v/atc-azure-digitaltwin.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/atc-azure-digitaltwin)

### Install

```bash
dotnet tool install --global atc-azure-digitaltwin
```

### Update

```bash
dotnet tool update --global atc-azure-digitaltwin
```

### Commands

The CLI is organized into three command groups:

#### Model Commands

```bash
# Validate models from a directory
atc-azure-digitaltwin model validate -d <directory-path>

# Create all models
atc-azure-digitaltwin model create all --tenantId -a <adt-instance-url> -d <directory-path>

# Create single model
atc-azure-digitaltwin model create single --tenantId -a <adt-instance-url> -d <directory-path> -m <model-id>

# Get all models
atc-azure-digitaltwin model get all --tenantId -a <adt-instance-url>

# Decommission a model
atc-azure-digitaltwin model decommission --tenantId -a <adt-instance-url> -m <model-id>

# Delete all models
atc-azure-digitaltwin model delete all --tenantId -a <adt-instance-url>
```

#### Twin Commands

```bash
# Count twins by model
atc-azure-digitaltwin twin count --tenantId -a <adt-instance-url>

# Create a twin
atc-azure-digitaltwin twin create --tenantId -a <adt-instance-url> -t <twin-id> -m <model-id> -modelVersion <version> --jsonPayload <json>

# Get a twin
atc-azure-digitaltwin twin get --tenantId -a <adt-instance-url> -t <twin-id>

# Update a twin with JSON Patch
atc-azure-digitaltwin twin update --tenantId -a <adt-instance-url> -t <twin-id> --jsonPatch <json-patch>

# Delete all twins
atc-azure-digitaltwin twin delete all --tenantId -a <adt-instance-url>

# Manage relationships
atc-azure-digitaltwin twin relationship create --tenantId -a <adt-instance-url> --source-twinId <src> --target-twinId <tgt> --relationshipName <name>
atc-azure-digitaltwin twin relationship get all --tenantId -a <adt-instance-url> -t <twin-id>
```

#### Event Route Commands

```bash
# Create an event route
atc-azure-digitaltwin route create --tenantId -a <adt-instance-url> -e <route-id> --endpointName <endpoint>

# Get all event routes
atc-azure-digitaltwin route get all --tenantId -a <adt-instance-url>

# Delete an event route
atc-azure-digitaltwin route delete --tenantId -a <adt-instance-url> -e <route-id>
```

Use `--help` on any command for detailed options:

```bash
atc-azure-digitaltwin --help
atc-azure-digitaltwin model --help
atc-azure-digitaltwin twin relationship get --help
```

## 📂 Sample

See the [sample console application](./sample/Atc.Azure.DigitalTwin.Console.Sample/) for a complete example demonstrating model validation, twin creation, querying, and cleanup using the DTDL models in the [models folder](./sample/models/).

## 🤝 How to contribute

[Contribution Guidelines](https://atc-net.github.io/introduction/about-atc#how-to-contribute)

[Coding Guidelines](https://atc-net.github.io/introduction/about-atc#coding-guidelines)
