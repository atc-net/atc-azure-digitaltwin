namespace Atc.Azure.DigitalTwin.Parsers;

/// <summary>
/// Defines a parser that handles the transformation of JSON models into digital twin interface definitions.
/// </summary>
public interface IDigitalTwinParser
{
    /// <summary>
    /// Parses a collection of JSON models into digital twin interfaces.
    /// </summary>
    /// <param name="jsonModels">The JSON models to parse.</param>
    /// <returns>A tuple indicating whether the parsing was successful and the interfaces if it was; otherwise, null.</returns>
    Task<(bool Succeeeded, IReadOnlyDictionary<Dtmi, DTEntityInfo>? Interfaces)> Parse(
        IEnumerable<string> jsonModels);
}