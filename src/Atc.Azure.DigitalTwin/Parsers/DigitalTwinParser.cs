namespace Atc.Azure.DigitalTwin.Parsers;

/// <summary>
/// Defines a parser that handles the transformation of JSON models into digital twin interface definitions.
/// </summary>
public sealed partial class DigitalTwinParser : IDigitalTwinParser
{
    private readonly ModelParser parser = new();

    public DigitalTwinParser(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<DigitalTwinParser>();
    }

    public async Task<(bool Succeeeded, IReadOnlyDictionary<Dtmi, DTEntityInfo>? Interfaces)> Parse(
        IEnumerable<string> jsonModels)
    {
        try
        {
            var interfaces = await parser.ParseAsync(jsonModels);

            return (true, interfaces);
        }
        catch (ParsingException ex)
        {
            LogParseFailed(ex.GetLastInnerMessage());
            foreach (var error in ex.Errors)
            {
                LogParseError($"Message: {error.Message}, PrimaryID: {error.PrimaryID}, SecondaryID: {error.SecondaryID}, Property: {error.Property}");
            }

            return (false, null);
        }
    }
}