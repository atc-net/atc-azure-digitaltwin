namespace Atc.Azure.DigitalTwin.DigitalTwin.Parsers;

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

    public async Task<(bool, IReadOnlyDictionary<Dtmi, DTEntityInfo>?)> ParseAsync(IEnumerable<string> jsonModelTexts)
    {
        try
        {
            var interfaces = await parser.ParseAsync(jsonModelTexts);
            return (true, interfaces);
        }
        catch (ParsingException pe)
        {
            logger.LogError("*** Error parsing models");
            var errorCount = 1;
            foreach (var err in pe.Errors)
            {
                logger.LogError($"Error {errorCount}:");
                logger.LogError($"{err.Message}");
                logger.LogError($"Primary ID: {err.PrimaryID}");
                logger.LogError($"Secondary ID: {err.SecondaryID}");
                logger.LogError($"Property: {err.Property}");
                errorCount++;
            }

            return (false, null);
        }
    }
}