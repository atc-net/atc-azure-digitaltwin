namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelCreateSingleCommand : AsyncCommand<ModelUploadSingleSettings>
{
    private readonly IModelService modelService;
    private readonly DigitalTwinsClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private readonly ILogger<ModelCreateSingleCommand> logger;

    public ModelCreateSingleCommand(
        IModelService modelService,
        DigitalTwinsClient client,
        ILogger<ModelCreateSingleCommand> logger)
    {
        this.modelService = modelService ?? throw new ArgumentNullException(nameof(modelService));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(CommandContext context, ModelUploadSingleSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(ModelUploadSingleSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var directoryPath = settings.DirectoryPath;
        var directoryInfo = new DirectoryInfo(directoryPath);
        if (!await modelService.LoadModelContentAsync(directoryInfo))
        {
            logger.LogError($"Could not load model from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        var modelId = settings.ModelId;

        logger.LogInformation($"Uploading Model with id '{modelId}'");

        try
        {
            var modelsContent = modelService.GetModelsContent();

            var model = modelsContent!.SingleOrDefault(x => x.Contains($"\"@id\": \"{modelId}\"", StringComparison.Ordinal));
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
        catch (RequestFailedException e)
        {
            logger.LogError($"Error {e.Status}: {e.Message}");
            return ConsoleExitStatusCodes.Failure;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return ConsoleExitStatusCodes.Failure;
        }

        return ConsoleExitStatusCodes.Success;
    }
}