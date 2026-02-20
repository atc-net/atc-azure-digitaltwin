namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ImportJobGetCommand : AsyncCommand<ImportJobCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ImportJobGetCommand> logger;

    public ImportJobGetCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ImportJobGetCommand>();
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

        logger.LogInformation($"Getting import job '{jobId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var result = await digitalTwinService.GetImportJobAsync(
                jobId,
                cancellationToken);

            if (result is null)
            {
                logger.LogWarning($"Import job '{jobId}' not found");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Import job '{jobId}': Status={result.Status}, Created={result.CreatedDateTime}");
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