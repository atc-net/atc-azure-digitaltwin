namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ImportJobCancelCommand : AsyncCommand<ImportJobCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ImportJobCancelCommand> logger;

    public ImportJobCancelCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ImportJobCancelCommand>();
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

        logger.LogInformation($"Cancelling import job '{jobId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var result = await digitalTwinService.CancelImportJobAsync(
                jobId,
                cancellationToken);

            if (result is null)
            {
                logger.LogError($"Failed to cancel import job '{jobId}'");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Successfully cancelled import job '{jobId}' with status '{result.Status}'");
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