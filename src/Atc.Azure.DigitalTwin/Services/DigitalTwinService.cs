namespace Atc.Azure.DigitalTwin.Services;

/// <summary>
/// Service for managing and interacting with digital twins and their models.
/// </summary>
public sealed partial class DigitalTwinService : IDigitalTwinService
{
    private readonly DigitalTwinsClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public DigitalTwinService(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client)
    {
        logger = loggerFactory.CreateLogger<DigitalTwinService>();
        this.client = client;
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public async Task<DigitalTwinsModelData?> GetModel(
        string modelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogRetrieving("model", modelId);

            var response = await client.GetModelAsync(
                modelId,
                cancellationToken);

            if (response is null)
            {
                LogNotFound("model", modelId);
                return default;
            }

            LogRetrieved("model", modelId);
            return response.Value;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return default;
        }
    }

    public AsyncPageable<DigitalTwinsModelData>? GetModels(
        GetModelsOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogRetrievingModels();
            var response = client.GetModelsAsync(
                options,
                cancellationToken);

            if (response is null)
            {
                LogModelsNotFound();
                return default;
            }

            LogRetrievedModels();
            return response;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return default;
        }
    }

    public Task<BasicDigitalTwin?> GetTwin(
        string twinId,
        CancellationToken cancellationToken = default)
        => GetTwin<BasicDigitalTwin>(
            twinId,
            cancellationToken);

    public async Task<T?> GetTwin<T>(
        string twinId,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        try
        {
            LogRetrieving("twin", twinId);
            var response = await client.GetDigitalTwinAsync<T>(
                twinId,
                cancellationToken);

            if (response is null)
            {
                LogNotFound("twin", twinId);
                return default;
            }

            LogRetrieved("twin", twinId);
            return response.Value;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return default;
        }
    }

    public async Task<List<string>?> GetTwinIds(
        string query,
        CancellationToken cancellationToken = default)
    {
        var twinList = new List<string>();

        try
        {
            LogRetrievingTwinIds(query);
            var response = client.QueryAsync<BasicDigitalTwin>(query, cancellationToken);
            await foreach (var twin in response)
            {
                twinList.Add(twin.Id);
            }
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return null;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return null;
        }

        LogRetrievedTwinIds(query);
        return twinList;
    }

    public async Task<List<BasicDigitalTwin>?> GetTwins(
        string query,
        CancellationToken cancellationToken = default)
    {
        var twinList = new List<BasicDigitalTwin>();

        try
        {
            LogRetrievingTwins(query);
            var response = client.QueryAsync<BasicDigitalTwin>(query, cancellationToken);
            await foreach (var twin in response)
            {
                twinList.Add(twin);
            }
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return null;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return null;
        }

        LogRetrievedTwins(query);
        return twinList;
    }

    public AsyncPageable<IncomingRelationship>? GetIncomingRelationships(
        string twinId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogRetrieving("incoming-relationships", twinId);
            var response = client.GetIncomingRelationshipsAsync(twinId, cancellationToken);

            if (response is null)
            {
                LogNotFound("incoming-relationships", twinId);
                return default;
            }

            LogRetrieved("incoming-relationships", twinId);
            return response;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return default;
        }
    }

    public async Task<BasicRelationship?> GetRelationship(
        string twinId,
        string relationshipName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogRetrievingRelationship(twinId, relationshipName);
            var response = GetRelationships(twinId, relationshipName, cancellationToken);

            if (response is null)
            {
                LogRelationshipsNotFound(twinId);
                return null;
            }

            var relationship = await response.SingleOrDefaultAsync(cancellationToken);
            if (relationship is null)
            {
                LogRelationshipNotFound(twinId, relationshipName);
                return null;
            }

            LogRetrievedRelationship(twinId, relationshipName);
            return relationship;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return default;
        }
    }

    public AsyncPageable<BasicRelationship>? GetRelationships(
        string twinId,
        string? relationshipName = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogRetrievingRelationships(twinId);
            var response = client.GetRelationshipsAsync<BasicRelationship>(
                twinId,
                relationshipName,
                cancellationToken);

            if (response is null)
            {
                LogRelationshipsNotFound(twinId);
                return default;
            }

            LogRetrievedRelationships(twinId);
            return response;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return default;
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> CreateOrReplaceDigitalTwin<T>(
        string twinId,
        T twin,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogCreatingOrReplacingTwin(twinId, JsonSerializer.Serialize(twin, jsonSerializerOptions));
            var response = await client.CreateOrReplaceDigitalTwinAsync(twinId, twin, cancellationToken: cancellationToken);

            if (response is null)
            {
                LogCreateOrReplaceTwinFailed(twinId, JsonSerializer.Serialize(twin, jsonSerializerOptions));
                return (false, "Failed to create twin");
            }

            LogCreatedOrReplacedTwin(twinId, JsonSerializer.Serialize(twin, jsonSerializerOptions));
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> CreateModels(
        IEnumerable<string> dtdlModels,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogCreatingModels();
            var response = await client.CreateModelsAsync(dtdlModels, cancellationToken);

            if (response is null)
            {
                LogCreateModelsFailed();
                return (false, "Failed to create models");
            }

            LogCreatedModels();
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> CreateRelationship(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName,
        bool isActive = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogCreatingOrUpdatingRelationship(sourceTwinId, targetTwinId, relationshipName);
            var relationship = new BasicRelationship
            {
                Name = relationshipName,
                Properties = { { "isActive", isActive } },
                SourceId = sourceTwinId,
                TargetId = targetTwinId,
            };

            var relationShipId = $"{sourceTwinId}-{relationshipName}->{targetTwinId}";
            var response = await client.CreateOrReplaceRelationshipAsync(sourceTwinId, relationShipId, relationship, cancellationToken: cancellationToken);

            if (response is null)
            {
                LogCreateOrUpdateRelationshipFailed(sourceTwinId, targetTwinId, relationshipName);
                return (false, $"Failed to create Relationship '{relationShipId}' on twin '{sourceTwinId}'");
            }

            LogCreatedOrUpdatedRelationship(sourceTwinId, targetTwinId, relationshipName);
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> CreateOrUpdateRelationship(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName,
        bool isActive = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogCreatingOrUpdatingRelationship(sourceTwinId, targetTwinId, relationshipName);
            var relationship = new BasicRelationship
            {
                Name = relationshipName,
                Properties = { { "isActive", isActive } },
                SourceId = sourceTwinId,
                TargetId = targetTwinId,
            };

            var relationShipId = $"{sourceTwinId}-{relationshipName}->{targetTwinId}";

            var existingRelationShip = await GetRelationship(sourceTwinId, relationShipId, cancellationToken);
            if (existingRelationShip is not null)
            {
                var patch = new JsonPatchDocument();
                if (existingRelationShip.Properties.Any(p => p.Key == "isActive"))
                {
                    patch.AppendReplace("/isActive", isActive);
                }
                else
                {
                    patch.AppendAdd("/isActive", isActive);
                }

                await UpdateRelationship(sourceTwinId, existingRelationShip.Id, patch, existingRelationShip.ETag, cancellationToken);
            }
            else
            {
                await client.CreateOrReplaceRelationshipAsync(sourceTwinId, relationShipId, relationship, cancellationToken: cancellationToken);
            }

            LogCreatedOrUpdatedRelationship(sourceTwinId, targetTwinId, relationshipName);
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> DecommissionModel(
        string modelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogDecommissioningModel(modelId);
            var response = await client.DecommissionModelAsync(
                modelId,
                cancellationToken);

            if (response is null ||
                response.IsError)
            {
                LogDecommissionModelFailed(modelId);
                return (false, "Failed to decommission model");
            }

            LogDecommissionedModel(modelId);
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> DeleteModel(
        string modelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogDeletingModel(modelId);
            var response = await client.DeleteModelAsync(
                modelId,
                cancellationToken);

            if (response is null ||
                response.IsError)
            {
                LogDeleteModelFailed(modelId);
            }

            LogDeletedModel(modelId);
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> DeleteRelationship(
        string twinId,
        string relationshipName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogRetrievingRelationship(twinId, relationshipName);
            var getResponse = await GetRelationship(twinId, relationshipName, cancellationToken);
            if (getResponse is null)
            {
                LogRelationshipNotFound(twinId, relationshipName);
                return (false, "RelationShip not found");
            }

            LogDeletingRelationship(twinId, relationshipName);
            var deleteResponse = await client.DeleteRelationshipAsync(
                twinId,
                getResponse.Id,
                getResponse.ETag,
                cancellationToken);

            if (deleteResponse is null ||
                deleteResponse.IsError)
            {
                LogDeleteRelationshipFailed(twinId, relationshipName);
                return (false, "Failed to delete relationship");
            }

            LogDeletedRelationship(twinId, relationshipName);
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task DeleteRelationships(
        string twinId,
        CancellationToken cancellationToken = default)
    {
        await FindAndDeleteOutgoingRelationshipsForTwin(twinId, cancellationToken);
        await FindAndDeleteIncomingRelationshipsForTwin(twinId, cancellationToken);
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> DeleteTwin(
        string twinId,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogDeletingTwin(twinId);
            var response = await client.DeleteDigitalTwinAsync(
                twinId,
                ifMatch,
                cancellationToken);

            if (response is null ||
                response.IsError)
            {
                LogDeleteTwinFailed(twinId);
                return (false, "Failed to delete twin");
            }

            LogDeletedTwin(twinId);
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> UpdateRelationship(
        string twinId,
        string relationshipId,
        JsonPatchDocument jsonPatchDocument,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogUpdatingRelationship(twinId, relationshipId);
            var response = await client.UpdateRelationshipAsync(
                twinId,
                relationshipId,
                jsonPatchDocument,
                ifMatch,
                cancellationToken);

            if (response is null ||
                response.IsError)
            {
                LogUpdateRelationshipFailed(twinId, relationshipId);
                return (false, "Failed to update relationship");
            }

            LogUpdatedRelationship(twinId, relationshipId);
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> UpdateTwin(
        string twinId,
        JsonPatchDocument jsonPatchDocument,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            LogUpdatingTwin(twinId);
            var response = await client.UpdateDigitalTwinAsync(
                twinId,
                jsonPatchDocument,
                ifMatch,
                cancellationToken);

            if (response is null ||
                response.IsError)
            {
                LogUpdateTwinFailed(twinId);
                return (false, "Failed to update twin");
            }

            LogUpdatedTwin(twinId);
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(
                ex.GetType().ToString(),
                errorMessage);
            return (false, errorMessage);
        }
    }

    public AsyncPageable<T>? Query<T>(
        string query,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        try
        {
            LogQuerying(query);
            var response = client.QueryAsync<T>(query, cancellationToken);
            if (response is null)
            {
                LogQueryingFailed(query);
                return default;
            }

            LogQueried(query);
            return response;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return default;
        }
    }

    public async Task<Page<T>?> Query<T>(
        string query,
        int pageSize,
        string? continuationToken = null,
        CancellationToken cancellationToken = default)
        where T : notnull
    {
        try
        {
            LogQuerying(query);
            var response = client.QueryAsync<T>(query, cancellationToken);
            var pagedResult = response.AsPages(continuationToken, pageSize);
            var result = await pagedResult.FirstOrDefaultAsync(cancellationToken);

            if (result is null)
            {
                LogQueryingFailed(query);
                return default;
            }

            LogQueried(query);
            return result;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(
                ex.Status,
                ex.ErrorCode,
                ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
            return default;
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> CreateOrReplaceEventRoute(
        string eventRouteId,
        string endpointName,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var eventRoute = new DigitalTwinsEventRoute(endpointName, filter ?? "true");

            var response = await client.CreateOrReplaceEventRouteAsync(
                eventRouteId,
                eventRoute,
                cancellationToken);

            if (response is null ||
                response.IsError)
            {
                return (false, $"Failed to create event route '{eventRouteId}'");
            }

            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(ex.Status, ex.ErrorCode, errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(ex.GetType().ToString(), errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> DeleteEventRoute(
        string eventRouteId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.DeleteEventRouteAsync(
                eventRouteId,
                cancellationToken);

            if (response is null ||
                response.IsError)
            {
                return (false, $"Failed to delete event route '{eventRouteId}'");
            }

            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogRequestFailed(ex.Status, ex.ErrorCode, errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            LogFailure(ex.GetType().ToString(), errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<DigitalTwinsEventRoute?> GetEventRoute(
        string eventRouteId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.GetEventRouteAsync(
                eventRouteId,
                cancellationToken);

            if (response is null)
            {
                return default;
            }

            return response.Value;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(ex.Status, ex.ErrorCode, ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(ex.GetType().ToString(), ex.GetLastInnerMessage());
            return default;
        }
    }

    public AsyncPageable<DigitalTwinsEventRoute>? GetEventRoutes(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = client.GetEventRoutesAsync(cancellationToken);
            return response;
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(ex.Status, ex.ErrorCode, ex.GetLastInnerMessage());
            return default;
        }
        catch (Exception ex)
        {
            LogFailure(ex.GetType().ToString(), ex.GetLastInnerMessage());
            return default;
        }
    }

    private async Task FindAndDeleteOutgoingRelationshipsForTwin(
        string twinId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var relationships = client.GetRelationshipsAsync<BasicRelationship>(
                twinId,
                cancellationToken: cancellationToken);

            await foreach (var relationship in relationships)
            {
                await client.DeleteRelationshipAsync(
                    twinId,
                    relationship.Id,
                    cancellationToken: cancellationToken);

                LogDeletingRelationship(twinId, relationship.Id);
            }
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(ex.Status, ex.ErrorCode, ex.GetLastInnerMessage());
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
        }
    }

    private async Task FindAndDeleteIncomingRelationshipsForTwin(
        string twinId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var incomingRelationships = client.GetIncomingRelationshipsAsync(
                twinId,
                cancellationToken);

            await foreach (var incomingRelationship in incomingRelationships)
            {
                await client.DeleteRelationshipAsync(
                    incomingRelationship.SourceId,
                    incomingRelationship.RelationshipId,
                    cancellationToken: cancellationToken);

                LogDeletingRelationship(twinId, incomingRelationship.RelationshipId);
            }
        }
        catch (RequestFailedException ex)
        {
            LogRequestFailed(ex.Status, ex.ErrorCode, ex.GetLastInnerMessage());
        }
        catch (Exception ex)
        {
            LogFailure(
                ex.GetType().ToString(),
                ex.GetLastInnerMessage());
        }
    }
}