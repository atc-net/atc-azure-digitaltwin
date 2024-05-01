namespace Atc.Azure.DigitalTwin.DigitalTwin.Parsers;

public interface IDigitalTwinParser
{
    Task<(bool, IReadOnlyDictionary<Dtmi, DTEntityInfo>?)> ParseAsync(
        IEnumerable<string> jsonModelTexts);
}