namespace Atc.Azure.DigitalTwin.Parsers;

/// <summary>
/// Defines a parser that handles the transformation of JSON models into digital twin interface definitions.
/// </summary>
/// <remarks>
/// This parser creates a new ModelParser per call to ParseAsync, making it safe for concurrent use.
/// </remarks>
public sealed partial class DigitalTwinParser : IDigitalTwinParser
{
    public DigitalTwinParser(ILoggerFactory loggerFactory)
        => logger = loggerFactory.CreateLogger<DigitalTwinParser>();

    public async Task<(bool Succeeded, IReadOnlyDictionary<Dtmi, DTEntityInfo>? Interfaces)> ParseAsync(
        IEnumerable<string> jsonModels)
    {
        try
        {
            var parser = new ModelParser();
            var interfaces = await parser.ParseAsync(jsonModels);

            return (true, interfaces);
        }
        catch (ParsingException ex)
        {
            LogParseFailed(ex);
            foreach (var error in ex.Errors)
            {
                LogParseError($"Message: {error.Message}, PrimaryID: {error.PrimaryID}, SecondaryID: {error.SecondaryID}, Property: {error.Property}");
            }

            return (false, null);
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            LogParseFailed(ex);
            return (false, null);
        }
    }
}