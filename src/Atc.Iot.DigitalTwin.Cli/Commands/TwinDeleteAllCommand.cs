namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class TwinDeleteAllCommand : AsyncCommand
{
    private readonly ITwinService twinService;
    private readonly ILogger<TwinDeleteAllCommand> logger;

    public TwinDeleteAllCommand(ITwinService twinService, ILogger<TwinDeleteAllCommand> logger)
    {
        this.twinService = twinService ?? throw new ArgumentNullException(nameof(twinService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogInformation("Deleting all twins.");
        logger.LogInformation("Step 1: Find all twins.");

        var twinList = await twinService.GetTwinIdsFromQuery("SELECT * FROM DIGITALTWINS");
        if (twinList is null)
        {
            return ConsoleExitStatusCodes.Failure;
        }

        logger.LogInformation("Step 2: Find and remove relationships for each twin.");
        foreach (var twinId in twinList)
        {
            await twinService.DeleteTwinRelationshipsByTwinId(twinId);
        }

        logger.LogInformation("Step 3: Delete all twins");
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