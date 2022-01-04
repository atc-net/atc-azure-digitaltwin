namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class RelationshipGetIncomingCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly DigitalTwinsClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private readonly ILogger<RelationshipGetIncomingCommand> logger;

    public RelationshipGetIncomingCommand(DigitalTwinsClient client, ILogger<RelationshipGetIncomingCommand> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(CommandContext context, TwinCommandSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(TwinCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var digitalTwinId = settings.TwinId;
        logger.LogInformation($"Getting incoming relationships for Digital Twin with id '{digitalTwinId}'");

        try
        {
            var relationships = client.GetIncomingRelationshipsAsync(digitalTwinId);
            await foreach (var relationship in relationships)
            {
                logger.LogInformation($"Relationship: {relationship.RelationshipName} from {relationship.SourceId} | {relationship.RelationshipId}");
                logger.LogInformation(JsonSerializer.Serialize(relationship, jsonSerializerOptions));
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
            logger.LogError(ex.Message);
            return ConsoleExitStatusCodes.Failure;
        }
    }
}