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
}