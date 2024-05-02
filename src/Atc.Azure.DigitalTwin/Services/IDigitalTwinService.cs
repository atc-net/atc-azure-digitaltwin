namespace Atc.Azure.DigitalTwin.Services;

public interface IDigitalTwinService
{
    Task<DigitalTwinsModelData?> GetModel(
        string modelId,
        CancellationToken cancellationToken = default);

    AsyncPageable<DigitalTwinsModelData>? GetModels(
        GetModelsOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<BasicDigitalTwin?> GetTwin(
        string twinId,
        CancellationToken cancellationToken = default);

    Task<T?> GetTwin<T>(
        string twinId,
        CancellationToken cancellationToken = default)
        where T : notnull;

    Task<List<string>?> GetTwinIds(
        string query,
        CancellationToken cancellationToken = default);

    Task<List<BasicDigitalTwin>?> GetTwins(
        string query,
        CancellationToken cancellationToken = default);

    AsyncPageable<IncomingRelationship>? GetIncomingRelationships(
        string twinId,
        CancellationToken cancellationToken = default);

    Task<BasicRelationship?> GetRelationship(
        string twinId,
        string relationshipName,
        CancellationToken cancellationToken = default);

    AsyncPageable<BasicRelationship>? GetRelationships(
        string twinId,
        string? relationshipName = null,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? ErrorMessage)> CreateModels(
        IEnumerable<string> dtdlModels,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? ErrorMessage)> CreateRelationship(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName,
        bool isActive = true,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? ErrorMessage)> CreateOrUpdateRelationship(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName,
        bool isActive = true,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? ErrorMessage)> DecommissionModel(
        string modelId,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? ErrorMessage)> DeleteModel(
        string modelId,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? ErrorMessage)> DeleteTwinRelationship(
        string twinId,
        string relationShipName,
        CancellationToken cancellationToken = default);

    Task DeleteTwinRelationships(
        string twinId,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? ErrorMessage)> DeleteTwin(
        string twinId,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? ErrorMessage)> UpdateRelationship(
        string twinId,
        string relationshipId,
        JsonPatchDocument jsonPatchDocument,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? ErrorMessage)> UpdateTwin(
        string twinId,
        JsonPatchDocument jsonPatchDocument,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default);

    AsyncPageable<T>? QueryAsync<T>(
        string query,
        CancellationToken cancellationToken = default)
        where T : notnull;

    Task<Page<T>?> QueryAsync<T>(
        string query,
        int pageSize,
        string? continuationToken = null,
        CancellationToken cancellationToken = default)
        where T : notnull;
}