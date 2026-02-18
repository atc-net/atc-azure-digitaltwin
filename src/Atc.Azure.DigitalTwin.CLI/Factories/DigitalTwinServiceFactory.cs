namespace Atc.Azure.DigitalTwin.CLI.Factories;

public static class DigitalTwinServiceFactory
{
    public static DigitalTwinService Create(
        ILoggerFactory loggerFactory,
        string tenantId,
        Uri instanceUrl)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        ArgumentNullException.ThrowIfNull(instanceUrl);

        var digitalTwinOptions = new DigitalTwinOptions
        {
            TenantId = tenantId,
            InstanceUrl = instanceUrl.ToString(),
        };

        return Create(loggerFactory, digitalTwinOptions);
    }

    public static DigitalTwinService Create(
        ILoggerFactory loggerFactory,
        DigitalTwinOptions digitalTwinOptions)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentNullException.ThrowIfNull(digitalTwinOptions);

        var client = new DigitalTwinsClient(
            new Uri(digitalTwinOptions.InstanceUrl),
            digitalTwinOptions.GetTokenCredential());

        return new DigitalTwinService(
            loggerFactory,
            client);
    }
}