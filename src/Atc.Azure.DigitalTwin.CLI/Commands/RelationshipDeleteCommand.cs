namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipDeleteCommand : AsyncCommand<RelationshipCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipDeleteCommand> logger;

    public RelationshipDeleteCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipDeleteCommand>();
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
        var relationshipId = settings.RelationshipId;

        logger.LogInformation($"Deleting relationship '{relationshipId}' from twin '{twinId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var (succeeded, errorMessage) = await digitalTwinService.DeleteRelationshipAsync(
                twinId,
                relationshipId,
                cancellationToken);

            if (!succeeded)
            {
                logger.LogError($"Failed to delete relationship '{relationshipId}' from twin '{twinId}': {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Deleted relationship '{relationshipId}' from twin '{twinId}'");
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