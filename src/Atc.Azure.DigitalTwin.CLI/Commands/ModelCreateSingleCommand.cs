namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelCreateSingleCommand : AsyncCommand<ModelUploadSingleSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelCreateSingleCommand> logger;
    private readonly IModelRepositoryService modelRepositoryService;
    private readonly DigitalTwinsClient client; // TODO: XXX
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelCreateSingleCommand(
        ILoggerFactory loggerFactory,
        IModelRepositoryService modelRepositoryService,
        DigitalTwinsClient client)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelCreateSingleCommand>();
        this.modelRepositoryService = modelRepositoryService;
        this.client = client;
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ModelUploadSingleSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        ModelUploadSingleSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var directoryPath = settings.DirectoryPath;
        var directoryInfo = new DirectoryInfo(directoryPath);

        if (!await modelRepositoryService.LoadModelContent(directoryInfo))
        {
            logger.LogError($"Could not load model from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        var modelId = settings.ModelId;

        logger.LogInformation($"Uploading Model with id '{modelId}'");

        try
        {
            var modelsContent = modelRepositoryService.GetModelsContent();

            var model = modelsContent.SingleOrDefault(x => x.Contains($"\"@id\": \"{modelId}\"", StringComparison.Ordinal));
            if (model is null)
            {
                logger.LogError($"Could not find model with the id '{modelId}'");
                return ConsoleExitStatusCodes.Failure;
            }

            var models = new[] { model };

            var result = await client.CreateModelsAsync(models);
            logger.LogInformation("Model uploaded successfully!");

            foreach (DigitalTwinsModelData md in result.Value)
            {
                logger.LogInformation(JsonSerializer.Serialize(md.DtdlModel, jsonSerializerOptions));
            }
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