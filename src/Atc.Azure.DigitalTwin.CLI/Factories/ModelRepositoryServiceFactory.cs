namespace Atc.Azure.DigitalTwin.CLI.Factories;

public static class ModelRepositoryServiceFactory
{
    public static ModelRepositoryService Create(
        ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);

        var digitalTwinParser = new DigitalTwinParser(loggerFactory);

        return new ModelRepositoryService(
            loggerFactory,
            digitalTwinParser);
    }
}