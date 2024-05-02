namespace Atc.Azure.DigitalTwin.Parsers;

public interface IDigitalTwinParser
{
    Task<(bool, IReadOnlyDictionary<Dtmi, DTEntityInfo>?)> ParseAsync(
        IEnumerable<string> jsonModelTexts);
}