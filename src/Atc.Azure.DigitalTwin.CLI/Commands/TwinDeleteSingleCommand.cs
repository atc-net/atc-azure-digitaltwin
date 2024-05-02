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

        var twinService = DigitalTwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            settings.AdtInstanceUrl!);

        var twinId = settings.TwinId;
        logger.LogInformation($"Deleting twin with id '{twinId}'");

        var (succeeded, errorMessage) = await twinService.DeleteTwin(twinId);
        if (!succeeded)
        {
            logger.LogError($"Failed to delete twin '{twinId}': {errorMessage}");
            return ConsoleExitStatusCodes.Failure;
        }

        logger.LogInformation($"Successfully deleted twin with id '{twinId}'");
        return ConsoleExitStatusCodes.Success;
    }
}