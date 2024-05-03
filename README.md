# Introduction

Azure Digital Twin library, which contains common services for Azure Digital Twins, Model Validation and parsing.

# Table of Content

- [Introduction](#introduction)
- [Table of Content](#table-of-content)
- [Atc.Azure.DigitalTwin](#atcazuredigitaltwin)
  - [IDigitalTwinService](#idigitaltwinservice)
    - [Features](#features)
    - [Usage Example](#usage-example)
    - [Configuring IDigitalTwinService](#configuring-idigitaltwinservice)
      - [Example Usage](#example-usage)
  - [IModelRepositoryService](#imodelrepositoryservice)
    - [Features](#features-1)
    - [Usage Example](#usage-example-1)
  - [IDigitalTwinParser](#idigitaltwinparser)
    - [Features](#features-2)
    - [Usage Example](#usage-example-2)
- [CLI](#cli)
  - [Installation](#installation)
  - [Update](#update)
  - [Usage](#usage)
    - [Option --help](#option---help)
- [Requirements](#requirements)
- [How to contribute](#how-to-contribute)

# Atc.Azure.DigitalTwin

[![NuGet Version](https://img.shields.io/nuget/v/atc.azure.digitaltwin.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/atc.azure.digitaltwin)

The `Atc.Azure.DigitalTwin` library provides a suite of services designed to facilitate the development, deployment, and management of Azure Digital Twins solutions. This library encompasses tools for robust model validation, efficient parsing, and comprehensive management of digital twins and their relationships. It supports seamless integration with existing Azure services, offering scalable options for managing complex digital twins environments. The library is essential for developers looking to leverage Azure Digital Twins in their IoT, AI, and data analytics applications, ensuring both flexibility and precision in digital twin interactions.

## IDigitalTwinService

The `IDigitalTwinService` offers comprehensive functionalities for managing and interacting with digital twins and their models, including CRUD operations, relationship handling, and querying capabilities within an Azure Digital Twins instance.

### Features

- **Model Management**: Create, retrieve, update, and delete digital twin models.
- **Twin Management**: Perform operations on digital twins such as creation, retrieval, update, and deletion
- **Relationship Management**: Manage relationships between digital twins, including creation, updates, and deletions.
- **Querying Capabilities**: Execute queries to retrieve twins, relationships, and other data points based on specific criteria.

### Usage Example

Below is a usage example of how to interact with digital twins using `IDigitalTwinService`:

```csharp
var digitalTwinService = serviceProvider.GetRequiredService<IDigitalTwinService>();

var twinId = "your_twin_id";
var twinData = await digitalTwinService.GetTwin(twinId);
if (twinData is not null)
{
    Console.WriteLine($"Retrieved Twin: {twinData.Id}");
}

// Create a new relationship between two twins
var sourceTwinId = "source_twin_id";
var targetTwinId = "target_twin_id";
var relationshipName = "relatesTo";
var createResult = await digitalTwinService.CreateRelationship(sourceTwinId, targetTwinId, relationshipName);

if (createResult.Succeeded)
{
    Console.WriteLine("Relationship created successfully.");
}
else
{
    Console.WriteLine($"Failed to create relationship: {createResult.ErrorMessage}");
}
```

### Configuring IDigitalTwinService

The `ConfigureDigitalTwinsClient` extension method from `ServiceCollectionExtensions` helps configure and register Digital Twin related services with your application's `IServiceCollection`.

#### Example Usage

Here's how you can configure your services using this extension method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    var digitalTwinOptions = new DigitalTwinOptions
    {
        TenantId = "your_tenant_id",
        InstanceUrl = "your_instance_url",
    };

    // Ensure these options are added to the DI container
    services.AddSingleton<DigitalTwinOptions>(digitalTwinOptions);

    services.ConfigureDigitalTwinsClient();
}
```

## IModelRepositoryService

The `IModelRepositoryService` manages local repositories of digital twin models, providing functionality for adding, retrieving, and clearing models from the local store.

### Features

- **Model Storage**: Manage a local repository of digital twin models.
- **Content Management**: Handle the content of model files, including loading and storing JSON representations.
- **Validation**: Validate the models against a set of predefined rules and definitions.

### Usage Example

Below is an example demonstrating how to utilize the `IModelRepositoryService` to manage DTDL models:

```csharp
var modelRepositoryService = serviceProvider.GetRequiredService<IModelRepositoryService>();
var modelDirectoryInfo = new DirectoryInfo("path_to_model_files"); // Specify the path to your model files

var loadedSuccessfully = await modelRepositoryService.LoadModelContent(modelDirectoryInfo);

if (loadedSuccessfully)
{
    var validationSucceeded = await modelRepositoryService.ValidateModels(modelDirectoryInfo);
    if (validationSucceeded)
    {
        Console.WriteLine("Models loaded and validated successfully.");
    }
    else
    {
        Console.WriteLine("Model validation failed.");
    }
}
else
{
    Console.WriteLine("Failed to load model content.");
}
```

## IDigitalTwinParser

The `IDigitalTwinParser` provides functionality for parsing JSON models into Digital Twin Interface definitions, aiding in the transformation and validation of digital twin data schemas.

### Features

- **Model Transformation**: Convert JSON formatted digital twin models into DTDL (Digital Twins Definition Language) interface definitions.
- **Validation**: Ensure the integrity and correctness of digital twin models through rigorous validation checks.
- - **Error Handling**: Efficiently manage parsing errors, providing detailed insight into the issues encountered during the parsing process.

### Usage Example

Below is an example demonstrating how to utilize the `IDigitalTwinParser` to parse JSON DTDL models:

```csharp
var digitalTwinParser = serviceProvider.GetRequiredService<IDigitalTwinParser>();
var jsonModels = new[] { "{...JSON data...}" }; // Replace with actual JSON strings
var parsingResult = await digitalTwinParser.Parse(jsonModels);

if (parsingResult.Succeeeded)
{
    foreach (var dtInterface in parsingResult.Interfaces)
    {
        Console.WriteLine($"Parsed Interface: {dtInterface.Key}, Info: {dtInterface.Value}");
    }
}
else
{
    Console.WriteLine("Failed to parse models.");
}
```

# CLI

[![NuGet Version](https://img.shields.io/nuget/v/atc-azure-digitaltwin.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/atc-azure-digitaltwin)

The `Atc.Azure.DigitalTwin.CLI` tool is available through a cross platform command line application.

## Installation

The tool can be installed as a .NET global tool by the following command

```powershell
dotnet tool install --global atc-azure-digitaltwin
```

or by following the instructions [here](https://www.nuget.org/packages/atc-azure-digitaltwin/) to install a specific version of the tool.

A successful installation will output something like

```powershell
The tool can be invoked by the following command: atc-azure-digitaltwin
Tool 'atc-azure-digitaltwin' (version '1.0.xxx') was successfully installed.`
```

## Update

The tool can be updated by the following command

```powershell
dotnet tool update --global atc-azure-digitaltwin
```

## Usage

Since the tool is published as a .NET Tool, it can be launched from anywhere using any shell or command-line interface by calling **atc-azure-digitaltwin**. The help information is displayed when providing the `--help` argument to **atc-azure-digitaltwin**

### Option <span style="color:yellow">--help</span>

```powershell
atc-azure-digitaltwin --help

USAGE:
    Atc.Azure.DigitalTwin.CLI.exe [OPTIONS] <COMMAND>

EXAMPLES:
    Atc.Azure.DigitalTwin.CLI.exe model decommission --tenantId -a <adt-instance-url> -m <model-id>
    Atc.Azure.DigitalTwin.CLI.exe model validate -d <directory-path>
    Atc.Azure.DigitalTwin.CLI.exe route create
    Atc.Azure.DigitalTwin.CLI.exe route delete
    Atc.Azure.DigitalTwin.CLI.exe twin count --tenantId -a <adt-instance-url>

OPTIONS:
    -h, --help       Prints help information
    -v, --version    Prints version information

COMMANDS:
    model    Operations related to models
    route    Operations related to event routes
    twin     Operations related to twins
```

# Requirements

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

# How to contribute

[Contribution Guidelines](https://atc-net.github.io/introduction/about-atc#how-to-contribute)

[Coding Guidelines](https://atc-net.github.io/introduction/about-atc#coding-guidelines)
