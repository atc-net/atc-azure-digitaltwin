namespace Atc.Azure.DigitalTwin.DigitalTwin.Services;

public interface ITwinService
{
    Task<BasicDigitalTwin?> GetTwinById(
        string twinId);

    Task<List<string>?> GetTwinIdsFromQuery(
        string query);

    Task<List<BasicDigitalTwin>?> GetTwinsFromQuery(
        string query);

    Task DeleteTwinRelationshipsByTwinId(
        string twinId);

    Task<bool> DeleteTwinById(
        string twinId);
}