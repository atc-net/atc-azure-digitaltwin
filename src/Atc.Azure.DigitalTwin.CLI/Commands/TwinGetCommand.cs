namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinGetCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinGetCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public TwinGetCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinGetCommand>();
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        TwinCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        TwinCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var digitalTwinService = DigitalTwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            new Uri(settings.AdtInstanceUrl!));

        var twinId = settings.TwinId;
        logger.LogInformation($"Getting Twin with id '{twinId}'");

        var twin = await digitalTwinService.GetTwin<BasicDigitalTwin>(twinId!, cancellationToken);
        if (twin is null)
        {
            logger.LogError($"Twin '{twinId}' not found");
            return ConsoleExitStatusCodes.Failure;
        }

        logger.LogInformation(JsonSerializer.Serialize(twin, jsonSerializerOptions));

        return ConsoleExitStatusCodes.Success;
    }
}