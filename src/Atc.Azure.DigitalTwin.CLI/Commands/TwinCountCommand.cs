namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinCountCommand : AsyncCommand<ConnectionBaseCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinCountCommand> logger;

    public TwinCountCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinCountCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ConnectionBaseCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ConnectionBaseCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        logger.LogInformation("Finding all twins");

        var digitalTwinService = DigitalTwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            new Uri(settings.AdtInstanceUrl!));

        var twinList = await digitalTwinService.GetTwins("SELECT * FROM DIGITALTWINS", cancellationToken);
        if (twinList is null ||
            twinList.Count == 0)
        {
            logger.LogWarning("No Twins found");
            return ConsoleExitStatusCodes.Failure;
        }

        var twinsByModel = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var twin in twinList)
        {
            var modelId = twin.Metadata.ModelId;
            twinsByModel.TryGetValue(modelId, out var count);
            twinsByModel[modelId] = count + 1;
        }

        logger.LogInformation("Found:");

        foreach (var (modelId, count) in twinsByModel)
        {
            logger.LogInformation($"     {count} twin(s) based on model '{modelId}'.");
        }

        return ConsoleExitStatusCodes.Success;
    }
}