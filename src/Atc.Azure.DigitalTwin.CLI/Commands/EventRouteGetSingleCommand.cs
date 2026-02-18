namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class EventRouteGetSingleCommand : AsyncCommand<EventRouteCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<EventRouteGetSingleCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public EventRouteGetSingleCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<EventRouteGetSingleCommand>();
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
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

        logger.LogInformation($"Getting event route '{eventRouteId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var eventRoute = await digitalTwinService.GetEventRoute(eventRouteId, cancellationToken);
            if (eventRoute is null)
            {
                logger.LogError($"Failed to fetch event route '{eventRouteId}'");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation("Successfully fetched event route.");
            logger.LogInformation(JsonSerializer.Serialize(eventRoute, jsonSerializerOptions));

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