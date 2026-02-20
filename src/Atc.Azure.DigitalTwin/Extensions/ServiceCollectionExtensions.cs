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

        services.AddTransient<IDigitalTwinParser, DigitalTwinParser>();
        services.AddTransient<IModelRepositoryService, ModelRepositoryService>();
        services.AddSingleton<IDigitalTwinService, DigitalTwinService>();

        return services;
    }
}