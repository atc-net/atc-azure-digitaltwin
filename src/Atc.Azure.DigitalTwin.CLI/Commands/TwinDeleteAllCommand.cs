namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinDeleteAllCommand : AsyncCommand<ConnectionBaseCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinDeleteAllCommand> logger;

    public TwinDeleteAllCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinDeleteAllCommand>();
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

        var twinService = TwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            settings.AdtInstanceUrl!);

        logger.LogInformation("Deleting all twins.");
        logger.LogInformation("Step 1: Find all twins.");

        var twinList = await twinService.GetTwinIds("SELECT * FROM DIGITALTWINS");
        if (twinList is null)
        {
            return ConsoleExitStatusCodes.Failure;
        }

        logger.LogInformation("Step 2: Find and remove relationships for each twin.");
        foreach (var twinId in twinList)
        {
            await twinService.DeleteTwinRelationships(twinId);
        }

        logger.LogInformation("Step 3: Delete all twins");
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