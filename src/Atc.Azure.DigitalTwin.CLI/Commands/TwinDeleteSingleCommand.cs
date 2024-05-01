namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinDeleteSingleCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly ITwinService twinService;
    private readonly ILogger<TwinDeleteSingleCommand> logger;

    public TwinDeleteSingleCommand(
        ILoggerFactory loggerFactory,
        ITwinService twinService)
    {
        logger = loggerFactory.CreateLogger<TwinDeleteSingleCommand>();
        this.twinService = twinService;
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        TwinCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        TwinCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var digitalTwinId = settings.TwinId;
        logger.LogInformation($"Deleting Twin with id '{digitalTwinId}'");

        return await twinService.DeleteTwinById(digitalTwinId)
            ? ConsoleExitStatusCodes.Success
            : ConsoleExitStatusCodes.Failure;
    }
}