namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class RelationshipGetSingleCommand : AsyncCommand<RelationshipGetSingleCommandSettings>
{
    private readonly DigitalTwinsClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private readonly ILogger<RelationshipGetSingleCommand> logger;

    public RelationshipGetSingleCommand(DigitalTwinsClient client, ILogger<RelationshipGetSingleCommand> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(CommandContext context, RelationshipGetSingleCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(RelationshipGetSingleCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var digitalTwinId = settings.TwinId;
        var relationshipId = settings.RelationshipId;

        logger.LogInformation($"Getting Relationship for Digital Twin with id '{digitalTwinId}' and Relationship id '{relationshipId}'.");

        try
        {
            var result = await client.GetRelationshipAsync<BasicRelationship>(digitalTwinId, relationshipId);
            if (result is not null)
            {
                logger.LogInformation(JsonSerializer.Serialize(result.Value, jsonSerializerOptions));
            }

            return ConsoleExitStatusCodes.Success;
        }
        catch (RequestFailedException e)
        {
            logger.LogError($"Error {e.Status}: {e.Message}");
            return ConsoleExitStatusCodes.Failure;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex}");
            return ConsoleExitStatusCodes.Failure;
        }
    }
}