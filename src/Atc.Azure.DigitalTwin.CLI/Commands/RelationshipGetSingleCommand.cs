namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipGetSingleCommand : AsyncCommand<RelationshipGetSingleCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipGetSingleCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public RelationshipGetSingleCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipGetSingleCommand>();
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

        logger.LogInformation($"Getting relationship for twin '{twinId}' and relationship id '{relationshipId}'.");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                settings.AdtInstanceUrl!);

            var result = await digitalTwinService.GetRelationship(twinId, relationshipId);
            if (result is null)
            {
                logger.LogError($"Failed to retrieve relationship for twin '{twinId} and relationship id '{relationshipId}''");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation(JsonSerializer.Serialize(result, jsonSerializerOptions));
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