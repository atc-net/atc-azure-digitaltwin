namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinDeleteAllCommand : AsyncCommand
{
    private readonly ITwinService twinService;
    private readonly ILogger<TwinDeleteAllCommand> logger;

    public TwinDeleteAllCommand(
        ILoggerFactory loggerFactory,
        ITwinService twinService)
    {
        logger = loggerFactory.CreateLogger<TwinDeleteAllCommand>();
        this.twinService = twinService;
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context)
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