namespace Atc.Azure.DigitalTwin.CLI.Factories;

public static class TwinServiceFactory
{
    public static TwinService Create(
        ILoggerFactory loggerFactory,
        string tenantId,
        string instanceUrl)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceUrl);

        var digitalTwinOptions = new DigitalTwinOptions
        {
            TenantId = tenantId,
            InstanceUrl = instanceUrl,
        };

        return Create(loggerFactory, digitalTwinOptions);
    }

    public static TwinService Create(
        ILoggerFactory loggerFactory,
        DigitalTwinOptions digitalTwinOptions)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentNullException.ThrowIfNull(digitalTwinOptions);

        var client = new DigitalTwinsClient(
            new Uri(digitalTwinOptions.InstanceUrl),
            digitalTwinOptions.GetTokenCredential());

        return new TwinService(
            loggerFactory,
            client);
    }
}