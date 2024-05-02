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

        var twinService = TwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            settings.AdtInstanceUrl!);

        var modelId = settings.ModelId;

        logger.LogInformation($"Deleting all twins by modelId '{modelId}");
        logger.LogInformation("Step 1: Find all twins");

        var twinList = await twinService.GetTwinIdsFromQuery($"SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL('{modelId}')");
        if (twinList is null)
        {
            return ConsoleExitStatusCodes.Failure;
        }

        logger.LogInformation("Step 2: Find and remove relationships for each twin.");
        foreach (var twinId in twinList)
        {
            await twinService.DeleteTwinRelationshipsByTwinId(twinId);
        }

        logger.LogInformation("Step 3: Delete all twins.");
        foreach (var twinId in twinList)
        {
            if (!await twinService.DeleteTwinById(twinId))
            {
                return ConsoleExitStatusCodes.Failure;
            }
        }

        return ConsoleExitStatusCodes.Success;
    }
}