namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ImportJobDeleteCommand : AsyncCommand<ImportJobCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ImportJobDeleteCommand> logger;

    public ImportJobDeleteCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ImportJobDeleteCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ImportJobCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ImportJobCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var jobId = settings.JobId;

        logger.LogInformation($"Deleting import job '{jobId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var (succeeded, errorMessage) = await digitalTwinService.DeleteImportJobAsync(
                jobId,
                cancellationToken);

            if (!succeeded)
            {
                logger.LogError($"Failed to delete import job '{jobId}': {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Successfully deleted import job '{jobId}'");
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