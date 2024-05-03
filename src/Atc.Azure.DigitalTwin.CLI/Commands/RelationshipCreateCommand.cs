namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipCreateCommand : AsyncCommand<RelationshipCreateCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipCreateCommand> logger;

    public RelationshipCreateCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipCreateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        RelationshipCreateCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        RelationshipCreateCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var sourceTwinId = settings.SourceTwinId;
        var targetTwinId = settings.TargetTwinId;
        var relationshipName = settings.RelationshipName;

        logger.LogInformation($"Creating relationship '{relationshipName}' between source twin '{sourceTwinId}' and target twin '{targetTwinId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                settings.AdtInstanceUrl!);

            var (succeeded, errorMessage) = await digitalTwinService.CreateRelationship(
                sourceTwinId,
                targetTwinId,
                relationshipName);

            if (!succeeded)
            {
                logger.LogError($"Failed to create relationship '{relationshipName}' between source twin '{sourceTwinId}' and target twin '{targetTwinId}': {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Created relationship '{relationshipName}' between source twin '{sourceTwinId}' and target twin '{targetTwinId}'");
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