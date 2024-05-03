namespace Atc.Azure.DigitalTwin.Services;

// TODO: Logger generated + way more logging
public sealed partial class DigitalTwinService : IDigitalTwinService
{
    private readonly ILogger<DigitalTwinService> logger;
    private readonly DigitalTwinsClient client;

    public DigitalTwinService(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client)
    {
        logger = loggerFactory.CreateLogger<DigitalTwinService>();
        this.client = client;
    }

    public async Task<DigitalTwinsModelData?> GetModel(
        string modelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await client.GetModelAsync(
                modelId,
                cancellationToken);

            if (result is null)
            {
                return default;
            }

            logger.LogInformation("Successfully fetched model");
            return result.Value;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
            return default;
        }
    }

    public AsyncPageable<DigitalTwinsModelData>? GetModels(
        GetModelsOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = client.GetModelsAsync(
                options,
                cancellationToken);

            if (response is null)
            {
                return default;
            }

            logger.LogInformation("Successfully fetched models");
            return response;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
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
            var result = await client.GetDigitalTwinAsync<T>(
                twinId,
                cancellationToken);

            if (result is null)
            {
                return default;
            }

            logger.LogInformation("Successfully fetched twin.");
            return result.Value;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
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
            var queryResult = client.QueryAsync<BasicDigitalTwin>(query, cancellationToken);
            await foreach (var item in queryResult)
            {
                twinList.Add(item.Id);
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}/{ex.ErrorCode} retrieving twins due to {ex.GetLastInnerMessage()}");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in query execution: {ex.GetLastInnerMessage()}");
            return null;
        }

        return twinList;
    }

    public async Task<List<BasicDigitalTwin>?> GetTwins(
        string query,
        CancellationToken cancellationToken = default)
    {
        var twinList = new List<BasicDigitalTwin>();

        try
        {
            var queryResult = client.QueryAsync<BasicDigitalTwin>(query, cancellationToken);
            await foreach (var item in queryResult)
            {
                twinList.Add(item);
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}/{ex.ErrorCode} retrieving twins due to {ex.GetLastInnerMessage()}");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in query execution: {ex.GetLastInnerMessage()}");
            return null;
        }

        return twinList;
    }

    public AsyncPageable<IncomingRelationship>? GetIncomingRelationships(
        string twinId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = client.GetIncomingRelationshipsAsync(twinId, cancellationToken);

            if (result is null)
            {
                return default;
            }

            logger.LogInformation("Successfully fetched incoming relationships.");
            return result;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
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
            var result = GetRelationships(twinId, relationshipName, cancellationToken);

            if (result is null)
            {
                return null;
            }

            var relationship = await result.SingleOrDefaultAsync(cancellationToken);
            if (relationship is null)
            {
                return null;
            }

            logger.LogInformation("Successfully fetched relationship.");
            return relationship;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
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
            var result = client.GetRelationshipsAsync<BasicRelationship>(
                twinId,
                relationshipName,
                cancellationToken);

            if (result is null)
            {
                return default;
            }

            logger.LogInformation("Successfully fetched incoming relationships.");
            return result;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
            return default;
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> CreateModels(
        IEnumerable<string> dtdlModels,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await client.CreateModelsAsync(dtdlModels, cancellationToken);

            if (result is null)
            {
                return (false, "Failed to create models");
            }

            logger.LogInformation("Successfully created models");
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = $"Error {ex.Status}: {ex.GetLastInnerMessage()}";
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            logger.LogError(errorMessage);
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
            var relationship = new BasicRelationship
            {
                Name = relationshipName,
                Properties = { { "isActive", isActive } },
                SourceId = sourceTwinId,
                TargetId = targetTwinId,
            };

            var relationShipId = $"{sourceTwinId}-{relationshipName}->{targetTwinId}";
            var result = await client.CreateOrReplaceRelationshipAsync(sourceTwinId, relationShipId, relationship, cancellationToken: cancellationToken);

            if (result is null)
            {
                return (false, $"Failed to create Relationship '{relationShipId}' on twin '{sourceTwinId}'");
            }

            logger.LogInformation($"Successfully created relationship {relationShipId}.");
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = $"Error {ex.Status}: {ex.GetLastInnerMessage()}";
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            logger.LogError(errorMessage);
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

            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = $"Error {ex.Status}: {ex.GetLastInnerMessage()}";
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> DecommissionModel(
        string modelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await client.DecommissionModelAsync(
                modelId,
                cancellationToken);

            logger.LogInformation($"Successfully decommissioned model '{modelId}'");
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = $"Error {ex.Status}/{ex.ErrorCode} Decommissioning model {modelId} due to {ex.GetLastInnerMessage()}";
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> DeleteModel(
        string modelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await client.DeleteModelAsync(
                modelId,
                cancellationToken);

            logger.LogInformation($"Successfully deleted model '{modelId}'");
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = $"Error {ex.Status}/{ex.ErrorCode} deleting model {modelId} due to {ex.GetLastInnerMessage()}";
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> DeleteTwinRelationship(
        string twinId,
        string relationshipName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var relationship = await GetRelationship(twinId, relationshipName, cancellationToken);
            if (relationship is null)
            {
                return (false, "RelationShip not found");
            }

            await client.DeleteRelationshipAsync(
                twinId,
                relationship.Id,
                relationship.ETag,
                cancellationToken);

            logger.LogInformation($"Successfully deleted relationship '{relationship.Id}' for twin '{twinId}'.");
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = $"Error {ex.Status}: {ex.GetLastInnerMessage()}";
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task DeleteTwinRelationships(
        string twinId,
        CancellationToken cancellationToken = default)
    {
        await FindAndDeleteOutgoingRelationshipsForTwinAsync(twinId, cancellationToken);
        await FindAndDeleteIncomingRelationshipsForTwinAsync(twinId, cancellationToken);
    }

    public async Task<(bool Succeeded, string? ErrorMessage)> DeleteTwin(
        string twinId,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await client.DeleteDigitalTwinAsync(
                twinId,
                ifMatch,
                cancellationToken);

            logger.LogInformation($"Successfully deleted twin '{twinId}'");
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = $"Error {ex.Status}/{ex.ErrorCode} deleting twin {twinId} due to {ex.GetLastInnerMessage()}";
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            logger.LogError(errorMessage);
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
            await client.UpdateRelationshipAsync(
                twinId,
                relationshipId,
                jsonPatchDocument,
                ifMatch,
                cancellationToken);

            logger.LogInformation($"Successfully updated relationship '{relationshipId}' on twin '{twinId}'");
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = $"Error {ex.Status}/{ex.ErrorCode} updating relationship '{relationshipId}' on twin '{twinId}' due to {ex.GetLastInnerMessage()}";
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            logger.LogError(errorMessage);
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
            await client.UpdateDigitalTwinAsync(
                twinId,
                jsonPatchDocument,
                ifMatch,
                cancellationToken);

            logger.LogInformation($"Successfully updated twin '{twinId}'");
            return (true, null);
        }
        catch (RequestFailedException ex)
        {
            var errorMessage = $"Error {ex.Status}/{ex.ErrorCode} updating twin '{twinId}' due to {ex.GetLastInnerMessage()}";
            logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.GetLastInnerMessage();
            logger.LogError(errorMessage);
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
            var result = client.QueryAsync<T>(query, cancellationToken);

            if (result is null)
            {
                return default;
            }

            logger.LogInformation($"Successfully queried for '{query}'.");
            return result;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
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
            var response = client.QueryAsync<T>(query, cancellationToken);
            var pagedResult = response.AsPages(continuationToken, pageSize);
            var result = await pagedResult.FirstOrDefaultAsync(cancellationToken);

            if (result is null)
            {
                return default;
            }

            logger.LogInformation($"Successfully queried for '{query}'.");
            return result;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
            return default;
        }
    }

    private async Task FindAndDeleteOutgoingRelationshipsForTwinAsync(
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

                logger.LogInformation($"Deleted relationship {relationship.Id} from {twinId}");
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}/{ex.ErrorCode} retrieving or deleting relationships for {twinId} due to {ex.GetLastInnerMessage()}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
        }
    }

    private async Task FindAndDeleteIncomingRelationshipsForTwinAsync(
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

                logger.LogInformation($"Deleted incoming relationship {incomingRelationship.RelationshipId} from {twinId}");
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}/{ex.ErrorCode} retrieving or deleting incoming relationships for {twinId} due to {ex.GetLastInnerMessage()}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
        }
    }
}