namespace Atc.Azure.DigitalTwin.Tests.Extensions;

public sealed class DigitalTwinOptionsExtensionsTests
{
    [Fact]
    public void GetCredentialOptions_NullOptions_ThrowsArgumentNullException()
    {
        // Act
        var act = () => DigitalTwinOptionsExtensions.GetCredentialOptions(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetCredentialOptions_ValidOptions_SetsTenantIdFields()
    {
        // Arrange
        var options = new DigitalTwinOptions
        {
            TenantId = "my-tenant-id",
            InstanceUrl = "https://my-instance.api.weu.digitaltwins.azure.net",
        };

        // Act
        var result = DigitalTwinOptionsExtensions.GetCredentialOptions(options);

        // Assert
        result.TenantId.Should().Be("my-tenant-id");
        result.SharedTokenCacheTenantId.Should().Be("my-tenant-id");
        result.VisualStudioTenantId.Should().Be("my-tenant-id");
        result.VisualStudioCodeTenantId.Should().Be("my-tenant-id");
    }

    [Fact]
    public void GetCredentialOptions_ValidOptions_ExcludesExpectedCredentials()
    {
        // Arrange
        var options = new DigitalTwinOptions
        {
            TenantId = "tenant-id",
            InstanceUrl = "https://instance.digitaltwins.azure.net",
        };

        // Act
        var result = DigitalTwinOptionsExtensions.GetCredentialOptions(options);

        // Assert - Only verify exclusions explicitly set in GetCredentialOptions
        result.ExcludeAzurePowerShellCredential.Should().BeTrue();
        result.ExcludeInteractiveBrowserCredential.Should().BeTrue();
        result.ExcludeEnvironmentCredential.Should().BeTrue();
        result.ExcludeVisualStudioCodeCredential.Should().BeTrue();
    }

    [Fact]
    public void GetTokenCredential_ReturnsDefaultAzureCredential()
    {
        // Arrange
        var options = new DigitalTwinOptions
        {
            TenantId = "my-tenant-id",
            InstanceUrl = "https://my-instance.api.weu.digitaltwins.azure.net",
        };

        // Act
        var result = options.GetTokenCredential();

        // Assert
        result.Should().BeOfType<DefaultAzureCredential>();
    }
}