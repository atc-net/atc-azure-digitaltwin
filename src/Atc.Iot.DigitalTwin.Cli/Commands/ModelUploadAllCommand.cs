using Atc.Iot.DigitalTwin.DigitalTwin.Services;

namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class ModelUploadAllCommand : AsyncCommand<ModelPathSettings>
{
    private readonly IModelService modelService;
    private readonly DigitalTwinsClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private readonly ILogger<ModelUploadAllCommand> logger;

    public ModelUploadAllCommand(
        IModelService modelService,
        DigitalTwinsClient client,
        ILogger<ModelUploadAllCommand> logger)
    {
        this.modelService = modelService ?? throw new ArgumentNullException(nameof(modelService));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(CommandContext context, ModelPathSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(ModelPathSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var directoryPath = settings.DirectoryPath;
        var directoryInfo = new DirectoryInfo(directoryPath);
        if (!await modelService.LoadModelContentAsync(directoryInfo))
        {
            logger.LogError($"Could not load model from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        try
        {
            var result = await client.CreateModelsAsync(modelService.GetModelsContent());
            logger.LogInformation("Models uploaded successfully!");

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