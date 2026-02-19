namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinDeleteAllByModelCommand : AsyncCommand<ModelCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinDeleteAllByModelCommand> logger;

    public TwinDeleteAllByModelCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinDeleteAllByModelCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ModelCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ModelCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var digitalTwinService = DigitalTwinServiceFactory.Create(
            loggerFactory,
            settings.TenantId!,
            new Uri(settings.AdtInstanceUrl!));

        var modelId = settings.ModelId;

        logger.LogInformation($"Deleting all twins by modelId '{modelId}");

        var twinList = await digitalTwinService.GetTwinIdsAsync($"SELECT * FROM DIGITALTWINS WHERE IS_OF_MODEL('{modelId}')", cancellationToken);
        if (twinList is null)
        {
            return ConsoleExitStatusCodes.Failure;
        }

        foreach (var twinId in twinList)
        {
            await digitalTwinService.DeleteRelationshipsAsync(twinId, cancellationToken);
        }

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