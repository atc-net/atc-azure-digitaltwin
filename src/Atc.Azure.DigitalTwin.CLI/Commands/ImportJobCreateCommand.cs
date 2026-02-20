namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ImportJobCreateCommand : AsyncCommand<ImportJobCreateCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ImportJobCreateCommand> logger;

    public ImportJobCreateCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ImportJobCreateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ImportJobCreateCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ImportJobCreateCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var jobId = settings.JobId;

        logger.LogInformation($"Creating import job '{jobId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var result = await digitalTwinService.ImportGraphAsync(
                jobId,
                new Uri(settings.InputBlobUri),
                new Uri(settings.OutputBlobUri),
                cancellationToken);

            if (result is null)
            {
                logger.LogError($"Failed to create import job '{jobId}'");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Successfully created import job '{jobId}' with status '{result.Status}'");
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