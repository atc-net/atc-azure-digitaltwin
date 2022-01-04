namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class TwinDeleteAllByModelCommand : AsyncCommand<ModelCommandSettings>
{
    private readonly ITwinService twinService;
    private readonly ILogger<TwinDeleteAllByModelCommand> logger;

    public TwinDeleteAllByModelCommand(ITwinService twinService, ILogger<TwinDeleteAllByModelCommand> logger)
    {
        this.twinService = twinService ?? throw new ArgumentNullException(nameof(twinService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override Task<int> ExecuteAsync(CommandContext context, ModelCommandSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(ModelCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

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