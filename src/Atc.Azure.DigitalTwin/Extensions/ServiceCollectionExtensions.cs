namespace Atc.Azure.DigitalTwin.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDigitalTwinsClient(
        this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var digitalTwinOptions = sp.GetRequiredService<DigitalTwinOptions>();

            var client = new DigitalTwinsClient(
                new Uri(digitalTwinOptions.InstanceUrl),
                digitalTwinOptions.GetTokenCredential());

            return client;
        });

        services.AddSingleton<IDigitalTwinParser, DigitalTwinParser>();
        services.AddSingleton<Services.IModelRepositoryService, Services.ModelRepositoryService>();
        services.AddSingleton<Services.IDigitalTwinService, Services.DigitalTwinService>();

        return services;
    }
}