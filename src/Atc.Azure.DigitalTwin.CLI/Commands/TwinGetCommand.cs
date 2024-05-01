namespace Atc.Azure.DigitalTwin.CLI.Commands;

public class TwinGetCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly ITwinService twinService;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private readonly ILogger<TwinGetCommand> logger;

    public TwinGetCommand(ITwinService twinService, ILogger<TwinGetCommand> logger)
    {
        this.twinService = twinService ?? throw new ArgumentNullException(nameof(twinService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(CommandContext context, TwinCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(TwinCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var digitalTwinId = settings.TwinId;
        logger.LogInformation($"Getting Twin with id '{digitalTwinId}'");

        var twin = await twinService.GetTwinById(digitalTwinId);
        if (twin is null)
        {
            return ConsoleExitStatusCodes.Failure;
        }

        logger.LogInformation(JsonSerializer.Serialize(twin, jsonSerializerOptions));

        return ConsoleExitStatusCodes.Success;
    }
}