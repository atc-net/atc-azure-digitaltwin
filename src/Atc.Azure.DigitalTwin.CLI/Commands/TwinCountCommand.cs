namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinCountCommand : AsyncCommand
{
    private readonly ITwinService twinService;
    private readonly ILogger<TwinCountCommand> logger;

    public TwinCountCommand(ITwinService twinService, ILogger<TwinCountCommand> logger)
    {
        this.twinService = twinService ?? throw new ArgumentNullException(nameof(twinService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogInformation("Finding all twins");

        var twinList = await twinService.GetTwinsFromQuery("SELECT * FROM DIGITALTWINS");
        if (twinList is null)
        {
            logger.LogError("No Twins found");
            return ConsoleExitStatusCodes.Failure;
        }

        var groupedList = twinList.GroupBy(x => x.Metadata.ModelId, StringComparer.Ordinal).ToList();
        if (groupedList.Count > 0)
        {
            logger.LogInformation("Found:");

            foreach (var group in groupedList)
            {
                logger.LogInformation($"     {group.ToList().Count} twin(s) based on model '{group.Key}'.");
            }
        }
        else
        {
            logger.LogError("No Twins found");
        }

        return ConsoleExitStatusCodes.Success;
    }
}