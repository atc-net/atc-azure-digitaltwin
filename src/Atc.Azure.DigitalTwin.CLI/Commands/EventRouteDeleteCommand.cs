namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class EventRouteDeleteCommand : AsyncCommand<EventRouteCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<EventRouteDeleteCommand> logger;

    public EventRouteDeleteCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<EventRouteDeleteCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        EventRouteCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        EventRouteCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var eventRouteId = settings.EventRouteId;

        logger.LogInformation($"Deleting event route '{eventRouteId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var (succeeded, errorMessage) = await digitalTwinService.DeleteEventRouteAsync(
                eventRouteId,
                cancellationToken);

            if (!succeeded)
            {
                logger.LogError($"Failed to delete event route '{eventRouteId}': {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Successfully deleted event route '{eventRouteId}'");
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