namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinDeleteSingleCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinDeleteSingleCommand> logger;

    public TwinDeleteSingleCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinDeleteSingleCommand>();
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

        var twinService = TwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            settings.AdtInstanceUrl!);

        var digitalTwinId = settings.TwinId;
        logger.LogInformation($"Deleting Twin with id '{digitalTwinId}'");

        return await twinService.DeleteTwinById(digitalTwinId)
            ? ConsoleExitStatusCodes.Success
            : ConsoleExitStatusCodes.Failure;
    }
}