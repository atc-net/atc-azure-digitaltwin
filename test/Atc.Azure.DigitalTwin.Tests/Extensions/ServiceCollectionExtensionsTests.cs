namespace Atc.Azure.DigitalTwin.Tests.Extensions;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ConfigureDigitalTwinsClient_RegistersSingletonDigitalTwinsClient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.ConfigureDigitalTwinsClient();

        // Assert
        services.Should().ContainSingle(d =>
            d.ServiceType == typeof(DigitalTwinsClient) &&
            d.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void ConfigureDigitalTwinsClient_ReturnsSameServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.ConfigureDigitalTwinsClient();

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void ConfigureDigitalTwinsClient_RegistersAllServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.ConfigureDigitalTwinsClient();

        // Assert - All higher-level services are registered
        services.Should().Contain(d => d.ServiceType == typeof(IDigitalTwinService));
        services.Should().Contain(d => d.ServiceType == typeof(IModelRepositoryService));
        services.Should().Contain(d => d.ServiceType == typeof(IDigitalTwinParser));
    }

    [Fact]
    public void ConfigureDigitalTwinsClient_ResolvingWithoutOptions_ThrowsInvalidOperationException()
    {
        // Arrange - Register DigitalTwinsClient but NOT DigitalTwinOptions
        var services = new ServiceCollection();
        services.ConfigureDigitalTwinsClient();
        var provider = services.BuildServiceProvider();

        // Act
        var act = () => provider.GetRequiredService<DigitalTwinsClient>();

        // Assert - The factory lambda calls GetRequiredService<DigitalTwinOptions>()
        // which throws when DigitalTwinOptions is not registered.
        act.Should().Throw<InvalidOperationException>();
    }
}