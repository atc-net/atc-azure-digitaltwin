namespace Atc.Azure.DigitalTwin.Parsers;

// TODO: Logger generated
public sealed partial class DigitalTwinParser : IDigitalTwinParser
{
    private readonly ILogger<DigitalTwinParser> logger;
    private readonly ModelParser parser = new ();

    public DigitalTwinParser(
        ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<DigitalTwinParser>();
    }

    public async Task<(bool Succeeeded, IReadOnlyDictionary<Dtmi, DTEntityInfo>? Interfaces)> ParseAsync(
        IEnumerable<string> jsonModels)
    {
        try
        {
            var interfaces = await parser.ParseAsync(jsonModels);

            return (true, interfaces);
        }
        catch (ParsingException ex)
        {
            logger.LogError("Error parsing models");
            var errorCount = 1;
            foreach (var error in ex.Errors)
            {
                logger.LogError($"Error {errorCount}:");
                logger.LogError($"{error.Message}");
                logger.LogError($"Primary ID: {error.PrimaryID}");
                logger.LogError($"Secondary ID: {error.SecondaryID}");
                logger.LogError($"Property: {error.Property}");
                errorCount++;
            }

            return (false, null);
        }
    }
}