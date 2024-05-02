namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinDeleteAllByModelCommand : AsyncCommand<ModelCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinDeleteAllByModelCommand> logger;

    public TwinDeleteAllByModelCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinDeleteAllByModelCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ModelCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        ModelCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var twinService = DigitalTwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            settings.AdtInstanceUrl!);

        var modelId = settings.ModelId;

        logger.LogInformation($"Deleting all twins by modelId '{modelId}");

        var twinList = await twinService.GetTwinIds($"SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL('{modelId}')");
        if (twinList is null)
        {
            return ConsoleExitStatusCodes.Failure;
        }

        foreach (var twinId in twinList)
        {
            await twinService.DeleteTwinRelationships(twinId);
        }

        foreach (var twinId in twinList)
        {
            var (succeeded, errorMessage) = await twinService.DeleteTwin(twinId);
            if (!succeeded)
            {
                logger.LogError($"Failed to delete twin '{twinId}': {errorMessage}");
            }
        }

        return ConsoleExitStatusCodes.Success;
    }
}