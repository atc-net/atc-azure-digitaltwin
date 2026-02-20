namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ImportJobGetAllCommand : AsyncCommand<ConnectionBaseCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ImportJobGetAllCommand> logger;

    public ImportJobGetAllCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ImportJobGetAllCommand>();
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

        logger.LogInformation("Getting all import jobs");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var result = await digitalTwinService.GetImportJobsAsync(cancellationToken);

            if (result is null)
            {
                logger.LogError("Failed to retrieve import jobs");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Found {result.Count} import job(s)");
            foreach (var job in result)
            {
                logger.LogInformation($"  Job '{job.Id}': Status={job.Status}, Created={job.CreatedDateTime}");
            }

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