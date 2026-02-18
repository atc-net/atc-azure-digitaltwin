namespace Atc.Azure.DigitalTwin.CLI.Factories;

public static class DigitalTwinParserFactory
{
    public static DigitalTwinParser Create(ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);

        return new DigitalTwinParser(loggerFactory);
    }
}