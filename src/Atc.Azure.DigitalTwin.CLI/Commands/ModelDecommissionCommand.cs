namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelDecommissionCommand : AsyncCommand<ModelCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelDecommissionCommand> logger;

    public ModelDecommissionCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelDecommissionCommand>();
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

        var modelId = settings.ModelId;
        logger.LogInformation($"Decommissioning Model with id '{modelId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                settings.AdtInstanceUrl!);

            var (succeeded, errorMessage) = await digitalTwinService.DecommissionModel(modelId);
            if (!succeeded)
            {
                logger.LogError($"Failed to decommission model '{modelId}': {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation("Successfully decommissioned model.");
            return ConsoleExitStatusCodes.Success;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error {ex.Status}: {ex.GetLastInnerMessage()}");
            return ConsoleExitStatusCodes.Failure;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.GetLastInnerMessage());
            return ConsoleExitStatusCodes.Failure;
        }
    }
}