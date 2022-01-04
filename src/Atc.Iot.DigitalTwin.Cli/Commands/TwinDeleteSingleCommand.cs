namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class TwinDeleteSingleCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly ITwinService twinService;
    private readonly ILogger<TwinDeleteSingleCommand> logger;

    public TwinDeleteSingleCommand(ITwinService twinService, ILogger<TwinDeleteSingleCommand> logger)
    {
        this.twinService = twinService ?? throw new ArgumentNullException(nameof(twinService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override Task<int> ExecuteAsync(CommandContext context, TwinCommandSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(TwinCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var digitalTwinId = settings.TwinId;
        logger.LogInformation($"Deleting Twin with id '{digitalTwinId}'");

        return await twinService.DeleteTwinById(digitalTwinId)
            ? ConsoleExitStatusCodes.Success
            : ConsoleExitStatusCodes.Failure;
    }
}