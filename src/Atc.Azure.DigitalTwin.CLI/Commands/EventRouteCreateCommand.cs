namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class EventRouteCreateCommand : AsyncCommand<EventRouteCreateCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<EventRouteCreateCommand> logger;

    public EventRouteCreateCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<EventRouteCreateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        EventRouteCreateCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        EventRouteCreateCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var eventRouteId = settings.EventRouteId;
        var endpointName = settings.EndpointName;
        var filter = settings.Filter;

        logger.LogInformation($"Creating event route '{eventRouteId}' targeting endpoint '{endpointName}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var (succeeded, errorMessage) = await digitalTwinService.CreateOrReplaceEventRouteAsync(
                eventRouteId,
                endpointName,
                filter,
                cancellationToken);

            if (!succeeded)
            {
                logger.LogError($"Failed to create event route '{eventRouteId}': {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Successfully created event route '{eventRouteId}'");
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