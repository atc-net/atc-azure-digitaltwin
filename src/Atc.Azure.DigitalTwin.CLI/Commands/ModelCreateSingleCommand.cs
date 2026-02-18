namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelCreateSingleCommand : AsyncCommand<ModelUploadSingleSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelCreateSingleCommand> logger;

    public ModelCreateSingleCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelCreateSingleCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ModelUploadSingleSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ModelUploadSingleSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var directoryPath = settings.DirectoryPath;
        var directoryInfo = new DirectoryInfo(directoryPath);

        var modelRepositoryService = ModelRepositoryServiceFactory.Create(loggerFactory);

        if (!await modelRepositoryService.LoadModelContent(directoryInfo, cancellationToken))
        {
            logger.LogError($"Could not load model from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        var modelId = settings.ModelId;

        logger.LogInformation($"Uploading Model with id '{modelId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var modelsContent = modelRepositoryService.GetModelsContent();

            var model = modelsContent.SingleOrDefault(x => x.Contains($"\"@id\": \"{modelId}\"", StringComparison.Ordinal));
            if (model is null)
            {
                logger.LogError($"Could not find model with the id '{modelId}'");
                return ConsoleExitStatusCodes.Failure;
            }

            var models = new[] { model };

            var (succeeded, errorMessage) = await digitalTwinService.CreateModels(models, cancellationToken);
            if (!succeeded)
            {
                logger.LogError($"Failed to upload model: {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation("Successfully uploaded model");
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