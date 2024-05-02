namespace Atc.Azure.DigitalTwin.Parsers;

public interface IDigitalTwinParser
{
    Task<(bool Succeeeded, IReadOnlyDictionary<Dtmi, DTEntityInfo>? Interfaces)> ParseAsync(
        IEnumerable<string> jsonModels);
}