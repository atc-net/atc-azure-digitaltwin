namespace Atc.Iot.DigitalTwin.Cli.Domain.DigitalTwin.Parsers;

public interface IDigitalTwinParser
{
    Task<(bool, IReadOnlyDictionary<Dtmi, DTEntityInfo>?)> ParseAsync(IEnumerable<string> jsonModelTexts);
}