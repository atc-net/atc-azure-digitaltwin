namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipGetIncomingCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipGetIncomingCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public RelationshipGetIncomingCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipGetIncomingCommand>();
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

        var twinId = settings.TwinId;
        logger.LogInformation($"Getting incoming relationships for twin with id '{twinId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                settings.AdtInstanceUrl!);

            var response = digitalTwinService.GetIncomingRelationships(twinId);
            if (response is null)
            {
                logger.LogError($"Failed to fetch incoming relationships for twin with id '{twinId}'");
                return ConsoleExitStatusCodes.Failure;
            }

            await foreach (var relationship in response)
            {
                logger.LogInformation($"Relationship: {relationship.RelationshipName} from {relationship.SourceId} | {relationship.RelationshipId}");
                logger.LogInformation(JsonSerializer.Serialize(relationship, jsonSerializerOptions));
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
            logger.LogError(ex.GetLastInnerMessage());
            return ConsoleExitStatusCodes.Failure;
        }
    }
}