namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipGetIncomingCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipGetIncomingCommand> logger;
    private readonly DigitalTwinsClient client; // TODO: XXX
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public RelationshipGetIncomingCommand(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipGetIncomingCommand>();
        this.client = client;
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        TwinCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        TwinCommandSettings settings)
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
            logger.LogError(ex.GetLastInnerMessage());
            return ConsoleExitStatusCodes.Failure;
        }
    }
}