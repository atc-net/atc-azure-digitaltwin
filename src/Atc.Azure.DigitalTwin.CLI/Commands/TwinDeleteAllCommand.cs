namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinDeleteAllCommand : AsyncCommand<ConnectionBaseCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinDeleteAllCommand> logger;

    public TwinDeleteAllCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinDeleteAllCommand>();
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

        var digitalTwinService = DigitalTwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            new Uri(settings.AdtInstanceUrl!));

        logger.LogInformation("Deleting all twins.");
        logger.LogInformation("Step 1: Find all twins.");

        var twinList = await digitalTwinService.GetTwinIdsAsync("SELECT * FROM DIGITALTWINS", cancellationToken);
        if (twinList is null)
        {
            return ConsoleExitStatusCodes.Failure;
        }

        logger.LogInformation("Step 2: Find and remove relationships for each twin.");
        foreach (var twinId in twinList)
        {
            await digitalTwinService.DeleteRelationshipsAsync(twinId, cancellationToken);
        }

        logger.LogInformation("Step 3: Delete all twins");
        foreach (var twinId in twinList)
        {
            var (succeeded, errorMessage) = await digitalTwinService.DeleteTwinAsync(twinId, cancellationToken: cancellationToken);
            if (!succeeded)
            {
                logger.LogError($"Failed to delete twin '{twinId}': {errorMessage}");
            }
        }

        return ConsoleExitStatusCodes.Success;
    }
}