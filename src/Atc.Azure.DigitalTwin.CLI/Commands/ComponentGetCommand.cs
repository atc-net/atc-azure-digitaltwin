namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ComponentGetCommand : AsyncCommand<ComponentCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ComponentGetCommand> logger;

    public ComponentGetCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ComponentGetCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ComponentCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ComponentCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var twinId = settings.TwinId;
        var componentName = settings.ComponentName;

        logger.LogInformation($"Getting component '{componentName}' for twin '{twinId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var result = await digitalTwinService.GetComponentAsync<JsonElement>(
                twinId,
                componentName,
                cancellationToken);

            if (result.ValueKind == JsonValueKind.Undefined)
            {
                logger.LogWarning($"Component '{componentName}' not found for twin '{twinId}'");
                return ConsoleExitStatusCodes.Failure;
            }

            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            logger.LogInformation(json);
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