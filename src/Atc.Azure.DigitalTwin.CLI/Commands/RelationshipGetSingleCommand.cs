namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipGetSingleCommand : AsyncCommand<RelationshipCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipGetSingleCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public RelationshipGetSingleCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipGetSingleCommand>();
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        RelationshipCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        RelationshipCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var twinId = settings.TwinId;
        var relationshipName = settings.RelationshipName;

        logger.LogInformation($"Getting relationship for twin '{twinId}' and relationship name '{relationshipName}'.");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var result = await digitalTwinService.GetRelationshipAsync(twinId, relationshipName, cancellationToken);
            if (result is null)
            {
                logger.LogError($"Failed to retrieve relationship for twin '{twinId}' and relationship name '{relationshipName}'");
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