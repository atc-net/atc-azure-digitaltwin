namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipGetSingleCommand : AsyncCommand<RelationshipGetSingleCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipGetSingleCommand> logger;
    private readonly DigitalTwinsClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public RelationshipGetSingleCommand(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipGetSingleCommand>();
        this.client = client;
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        RelationshipGetSingleCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        RelationshipGetSingleCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var twinId = settings.TwinId;
        var relationshipId = settings.RelationshipId;

        logger.LogInformation($"Getting Relationship for twin with id '{twinId}' and relationship id '{relationshipId}'.");

        try
        {
            var result = await client.GetRelationshipAsync<BasicRelationship>(twinId, relationshipId);
            if (result is not null)
            {
                logger.LogInformation(JsonSerializer.Serialize(result.Value, jsonSerializerOptions));
            }

            return ConsoleExitStatusCodes.Success;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return ConsoleExitStatusCodes.Failure;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex}");
            return ConsoleExitStatusCodes.Failure;
        }
    }
}