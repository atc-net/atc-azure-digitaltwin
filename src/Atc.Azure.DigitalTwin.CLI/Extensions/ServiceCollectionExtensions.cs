namespace Atc.Azure.DigitalTwin.CLI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDigitalTwinsClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<DigitalTwinsClient>(provider =>
        {
            var adtInstanceUrl = configuration["AdtInstanceUrl"]; // Ensure this is in your appsettings.json or a secure place
            if (string.IsNullOrEmpty(adtInstanceUrl))
            {
                throw new InvalidOperationException("Digital Twins URL is not configured.");
            }

            // TODO: Extend to check local credentials...
            var credential = new DefaultAzureCredential();
            var client = new DigitalTwinsClient(new Uri(adtInstanceUrl), credential);
            return client;
        });

        return services;
    }
}