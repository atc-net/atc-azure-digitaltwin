namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class EventRouteGetAllCommand : AsyncCommand<ConnectionBaseCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<EventRouteGetAllCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public EventRouteGetAllCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<EventRouteGetAllCommand>();
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ConnectionBaseCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ConnectionBaseCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var routes = await digitalTwinService.GetEventRoutesAsync(cancellationToken);
            if (routes is null)
            {
                logger.LogError("Failed to get event routes");
                return ConsoleExitStatusCodes.Failure;
            }

            foreach (var eventRoute in routes)
            {
                logger.LogInformation($"EventRouteId: '{eventRoute.Id}'");
                logger.LogInformation(JsonSerializer.Serialize(eventRoute, jsonSerializerOptions));
            }

            logger.LogInformation($"Found {routes.Count} event route(s)");
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

        return ConsoleExitStatusCodes.Success;
    }
}