namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelCreateAllCommand : AsyncCommand<ModelPathSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelCreateAllCommand> logger;
    private readonly IModelRepositoryService modelRepositoryService;
    private readonly DigitalTwinsClient client; // TODO: XXX
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelCreateAllCommand(
        ILoggerFactory loggerFactory,
        IModelRepositoryService modelRepositoryService,
        DigitalTwinsClient client)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelCreateAllCommand>();
        this.modelRepositoryService = modelRepositoryService;
        this.client = client;
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ModelPathSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        ModelPathSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var directoryPath = settings.DirectoryPath;
        var directoryInfo = new DirectoryInfo(directoryPath);

        if (!await modelRepositoryService.LoadModelContent(directoryInfo))
        {
            logger.LogError($"Could not load model from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        try
        {
            var result = await client.CreateModelsAsync(modelRepositoryService.GetModelsContent());
            logger.LogInformation("Models uploaded successfully!");

            foreach (var digitalTwinsModelData in result.Value)
            {
                logger.LogInformation(JsonSerializer.Serialize(digitalTwinsModelData.DtdlModel, jsonSerializerOptions));
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