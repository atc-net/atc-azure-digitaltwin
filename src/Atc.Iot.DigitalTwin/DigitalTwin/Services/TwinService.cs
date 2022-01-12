namespace Atc.Iot.DigitalTwin.DigitalTwin.Services;

public class TwinService : ITwinService
{
    private readonly DigitalTwinsClient client;
    private readonly ILogger<TwinService> logger;

    public TwinService(DigitalTwinsClient client, ILogger<TwinService> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<BasicDigitalTwin?> GetTwinById(string twinId)
    {
        try
        {
            var result = await client.GetDigitalTwinAsync<BasicDigitalTwin>(twinId);
            if (result is null)
            {
                return null;
            }

            logger.LogInformation("Successfully fetched twin.");
            return result.Value;
        }
        catch (RequestFailedException e)
        {
            logger.LogError($"Error {e.Status}: {e.Message}");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return null;
        }
    }

    public async Task<List<string>?> GetTwinIdsFromQuery(string query)
    {
        var twinList = new List<string>();

        try
        {
            var queryResult = client.QueryAsync<BasicDigitalTwin>(query);
            await foreach (var item in queryResult)
            {
                twinList.Add(item.Id);
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"*** Error {ex.Status}/{ex.ErrorCode} retrieving twins due to {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in query execution: {ex.Message}");
            return null;
        }

        return twinList;
    }

    public async Task<List<BasicDigitalTwin>?> GetTwinsFromQuery(string query)
    {
        var twinList = new List<BasicDigitalTwin>();

        try
        {
            var queryResult = client.QueryAsync<BasicDigitalTwin>(query);
            await foreach (var item in queryResult)
            {
                twinList.Add(item);
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"*** Error {ex.Status}/{ex.ErrorCode} retrieving twins due to {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in query execution: {ex.Message}");
            return null;
        }

        return twinList;
    }

    public async Task DeleteTwinRelationshipsByTwinId(string twinId)
    {
        // Remove any relationships for the twin
        await FindAndDeleteOutgoingRelationshipsForTwinAsync(twinId);
        await FindAndDeleteIncomingRelationshipsForTwinAsync(twinId);
    }

    public async Task<bool> DeleteTwinById(string twinId)
    {
        try
        {
            await client.DeleteDigitalTwinAsync(twinId);
            logger.LogInformation($"Deleted twin {twinId}");
            return true;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"*** Error {ex.Status}/{ex.ErrorCode} deleting twin {twinId} due to {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return false;
        }
    }

    private async Task FindAndDeleteOutgoingRelationshipsForTwinAsync(string twinId)
    {
        try
        {
            var relationships = client.GetRelationshipsAsync<BasicRelationship>(twinId);

            await foreach (var relationship in relationships)
            {
                await client.DeleteRelationshipAsync(twinId, relationship.Id);
                logger.LogInformation($"Deleted relationship {relationship.Id} from {twinId}");
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"*** Error {ex.Status}/{ex.ErrorCode} retrieving or deleting relationships for {twinId} due to {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }

    private async Task FindAndDeleteIncomingRelationshipsForTwinAsync(string twinId)
    {
        try
        {
            var incomingRelationships = client.GetIncomingRelationshipsAsync(twinId);

            await foreach (var incomingRelationship in incomingRelationships)
            {
                await client.DeleteRelationshipAsync(incomingRelationship.SourceId, incomingRelationship.RelationshipId);
                logger.LogInformation($"Deleted incoming relationship {incomingRelationship.RelationshipId} from {twinId}");
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"*** Error {ex.Status}/{ex.ErrorCode} retrieving or deleting incoming relationships for {twinId} due to {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}