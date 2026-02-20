namespace Atc.Azure.DigitalTwin.Tests.CLI;

public sealed class SettingsValidationTests
{
    [Fact]
    public void ConnectionBase_MissingTenantId_ReturnsError()
    {
        // Arrange
        var settings = new ConnectionBaseCommandSettings
        {
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ConnectionBase_MissingAdtInstanceUrl_ReturnsError()
    {
        // Arrange
        var settings = new ConnectionBaseCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ConnectionBase_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new ConnectionBaseCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void TwinCommand_MissingTwinId_ReturnsError()
    {
        // Arrange
        var settings = new TwinCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void TwinCommand_MissingConnectionFields_ReturnsError()
    {
        // Arrange
        var settings = new TwinCommandSettings
        {
            TwinId = "twin-001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void TwinCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new TwinCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void TwinCreateCommand_MissingModelId_ReturnsError()
    {
        // Arrange
        var settings = new TwinCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            ModelVersion = 1,
            JsonPayload = "{}",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void TwinCreateCommand_InvalidModelVersion_ReturnsError()
    {
        // Arrange
        var settings = new TwinCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            ModelId = "dtmi:com:example:Thermostat;1",
            ModelVersion = 0,
            JsonPayload = "{}",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void TwinCreateCommand_MissingJsonPayload_ReturnsError()
    {
        // Arrange
        var settings = new TwinCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            ModelId = "dtmi:com:example:Thermostat;1",
            ModelVersion = 1,
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void TwinCreateCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new TwinCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            ModelId = "dtmi:com:example:Thermostat;1",
            ModelVersion = 1,
            JsonPayload = """{"temperature": 25.0}""",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void TwinUpdateCommand_MissingJsonPatch_ReturnsError()
    {
        // Arrange
        var settings = new TwinUpdateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void TwinUpdateCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new TwinUpdateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            JsonPatch = """[{"op": "replace", "path": "/temperature", "value": 30}]""",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void ModelCommand_MissingModelId_ReturnsError()
    {
        // Arrange
        var settings = new ModelCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ModelCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new ModelCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            ModelId = "dtmi:com:example:Thermostat;1",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void ModelPathSettings_MissingDirectoryPath_ReturnsError()
    {
        // Arrange
        var settings = new ModelPathSettings();

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ModelPathSettings_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new ModelPathSettings
        {
            DirectoryPath = "/tmp/models",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void ModelUploadSingleSettings_MissingDirectoryPath_ReturnsError()
    {
        // Arrange
        var settings = new ModelUploadSingleSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            ModelId = "dtmi:com:example:Thermostat;1",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ModelUploadSingleSettings_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new ModelUploadSingleSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            ModelId = "dtmi:com:example:Thermostat;1",
            DirectoryPath = "/tmp/models",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void ModelUploadMultipleSettings_MissingDirectoryPath_ReturnsError()
    {
        // Arrange
        var settings = new ModelUploadMultipleSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ModelUploadMultipleSettings_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new ModelUploadMultipleSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            DirectoryPath = "/tmp/models",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void EventRouteCommand_MissingEventRouteId_ReturnsError()
    {
        // Arrange
        var settings = new EventRouteCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void EventRouteCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new EventRouteCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            EventRouteId = "route-001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void EventRouteCreateCommand_MissingEndpointName_ReturnsError()
    {
        // Arrange
        var settings = new EventRouteCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            EventRouteId = "route-001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void EventRouteCreateCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new EventRouteCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            EventRouteId = "route-001",
            EndpointName = "my-endpoint",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void QueryCommand_MissingQuery_ReturnsError()
    {
        // Arrange
        var settings = new QueryCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void QueryCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new QueryCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            Query = "SELECT * FROM DIGITALTWINS",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void ImportJobCommand_MissingJobId_ReturnsError()
    {
        // Arrange
        var settings = new ImportJobCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ImportJobCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new ImportJobCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            JobId = "job-001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void ImportJobCreateCommand_MissingInputBlobUri_ReturnsError()
    {
        // Arrange
        var settings = new ImportJobCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            JobId = "job-001",
            OutputBlobUri = "https://storage.blob.core.windows.net/c/out.ndjson",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ImportJobCreateCommand_MissingOutputBlobUri_ReturnsError()
    {
        // Arrange
        var settings = new ImportJobCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            JobId = "job-001",
            InputBlobUri = "https://storage.blob.core.windows.net/c/in.ndjson",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ImportJobCreateCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new ImportJobCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            JobId = "job-001",
            InputBlobUri = "https://storage.blob.core.windows.net/c/in.ndjson",
            OutputBlobUri = "https://storage.blob.core.windows.net/c/out.ndjson",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void ComponentCommand_MissingComponentName_ReturnsError()
    {
        // Arrange
        var settings = new ComponentCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ComponentCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new ComponentCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            ComponentName = "thermostat",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void ComponentUpdateCommand_MissingJsonPatch_ReturnsError()
    {
        // Arrange
        var settings = new ComponentUpdateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            ComponentName = "thermostat",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void ComponentUpdateCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new ComponentUpdateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            ComponentName = "thermostat",
            JsonPatch = """[{"op": "replace", "path": "/temperature", "value": 30}]""",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void TelemetryPublishCommand_MissingPayload_ReturnsError()
    {
        // Arrange
        var settings = new TelemetryPublishCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void TelemetryPublishCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new TelemetryPublishCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            Payload = "{\"temperature\": 42.5}",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void TelemetryPublishCommand_WithComponentName_ReturnsSuccess()
    {
        // Arrange
        var settings = new TelemetryPublishCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            Payload = "{\"temperature\": 42.5}",
            ComponentName = "thermostat",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void RelationshipCommand_MissingTwinId_ReturnsError()
    {
        // Arrange
        var settings = new RelationshipCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            RelationshipName = "relatesTo",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void RelationshipCommand_MissingRelationshipName_ReturnsError()
    {
        // Arrange
        var settings = new RelationshipCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void RelationshipCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new RelationshipCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TwinId = "twin-001",
            RelationshipName = "relatesTo",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }

    [Fact]
    public void RelationshipCreateCommand_MissingSourceTwinId_ReturnsError()
    {
        // Arrange
        var settings = new RelationshipCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            TargetTwinId = "twin-002",
            RelationshipName = "relatesTo",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void RelationshipCreateCommand_MissingTargetTwinId_ReturnsError()
    {
        // Arrange
        var settings = new RelationshipCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            SourceTwinId = "twin-001",
            RelationshipName = "relatesTo",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void RelationshipCreateCommand_MissingRelationshipName_ReturnsError()
    {
        // Arrange
        var settings = new RelationshipCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            SourceTwinId = "twin-001",
            TargetTwinId = "twin-002",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeFalse();
    }

    [Fact]
    public void RelationshipCreateCommand_AllFieldsPresent_ReturnsSuccess()
    {
        // Arrange
        var settings = new RelationshipCreateCommandSettings
        {
            TenantId = "00000000-0000-0000-0000-000000000001",
            AdtInstanceUrl = "https://test.api.eus.digitaltwins.azure.net",
            SourceTwinId = "twin-001",
            TargetTwinId = "twin-002",
            RelationshipName = "relatesTo",
        };

        // Act
        var result = settings.Validate();

        // Assert
        result.Successful.Should().BeTrue();
    }
}