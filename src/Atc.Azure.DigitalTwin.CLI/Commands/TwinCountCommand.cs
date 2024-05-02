namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinCountCommand : AsyncCommand<ConnectionBaseCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinCountCommand> logger;

    public TwinCountCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinCountCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ConnectionBaseCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        ConnectionBaseCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        logger.LogInformation("Finding all twins");

        var twinService = TwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            settings.AdtInstanceUrl!);

        var twinList = await twinService.GetTwins("SELECT * FROM DIGITALTWINS");
        if (twinList is null ||
            twinList.Count == 0)
        {
            logger.LogWarning("No Twins found");
            return ConsoleExitStatusCodes.Failure;
        }

        var groupedTwinList = twinList
            .GroupBy(x => x.Metadata.ModelId, StringComparer.Ordinal)
            .ToList();

        logger.LogInformation("Found:");

        foreach (var group in groupedTwinList)
        {
            logger.LogInformation($"     {group.ToList().Count} twin(s) based on model '{group.Key}'.");
        }

        return ConsoleExitStatusCodes.Success;
    }
}