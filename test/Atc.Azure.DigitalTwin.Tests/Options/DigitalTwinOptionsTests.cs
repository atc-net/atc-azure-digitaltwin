namespace Atc.Azure.DigitalTwin.Tests.Options;

public sealed class DigitalTwinOptionsTests
{
    [Fact]
    public void DefaultValues_AreEmptyStrings()
    {
        // Arrange
        var sut = new DigitalTwinOptions();

        // Act & Assert
        sut.TenantId.Should().BeEmpty();
        sut.InstanceUrl.Should().BeEmpty();
    }

    [Fact]
    public void TenantId_CanBeSetAndRetrieved()
    {
        // Arrange
        var sut = new DigitalTwinOptions
        {
            TenantId = "my-tenant-id",
        };

        // Act & Assert
        sut.TenantId.Should().Be("my-tenant-id");
    }

    [Fact]
    public void InstanceUrl_CanBeSetAndRetrieved()
    {
        // Arrange
        var sut = new DigitalTwinOptions
        {
            InstanceUrl = "https://my-instance.api.weu.digitaltwins.azure.net",
        };

        // Act & Assert
        sut.InstanceUrl.Should().Be("https://my-instance.api.weu.digitaltwins.azure.net");
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat()
    {
        // Arrange
        var sut = new DigitalTwinOptions
        {
            TenantId = "tenant-123",
            InstanceUrl = "https://example.digitaltwins.azure.net",
        };

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("TenantId: tenant-123, InstanceUrl: https://example.digitaltwins.azure.net");
    }

    [Fact]
    public void ToString_WithDefaultValues_ReturnsEmptyValues()
    {
        // Arrange
        var sut = new DigitalTwinOptions();

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("TenantId: , InstanceUrl: ");
    }
}