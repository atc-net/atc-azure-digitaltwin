namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelCreateAllCommand : AsyncCommand<ModelUploadMultipleSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelCreateAllCommand> logger;

    public ModelCreateAllCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelCreateAllCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ModelUploadMultipleSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ModelUploadMultipleSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var directoryPath = settings.DirectoryPath;
        var directoryInfo = new DirectoryInfo(directoryPath);

        var modelRepositoryService = ModelRepositoryServiceFactory.Create(loggerFactory);

        if (!await modelRepositoryService.LoadModelContentAsync(directoryInfo, cancellationToken))
        {
            logger.LogError($"Could not load model from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var (succeeded, errorMessage) = await digitalTwinService.CreateModelsAsync(modelRepositoryService.GetModelsContent(), cancellationToken);
            if (!succeeded)
            {
                logger.LogError($"Failed to upload models: {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation("Successfully uploaded models");
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

        return ConsoleExitStatusCodes.Success;
    }
}