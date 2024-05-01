namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelGetAllCommand : AsyncCommand
{
    private readonly DigitalTwinsClient client;
    private readonly ILogger<ModelGetAllCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelGetAllCommand(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client)
    {
        logger = loggerFactory.CreateLogger<ModelGetAllCommand>();
        this.client = client;
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        try
        {
            var digitalTwinsModelDataResponse = client.GetModelsAsync(new GetModelsOptions { IncludeModelDefinition = true });

            var resultList = new List<DigitalTwinsModelData>();
            await foreach (var digitalTwinsModelData in digitalTwinsModelDataResponse)
            {
                logger.LogInformation($"ModelId: '{digitalTwinsModelData.Id}'");
                if (digitalTwinsModelData.DtdlModel != null)
                {
                    logger.LogInformation(JsonSerializer.Serialize(digitalTwinsModelData.DtdlModel, jsonSerializerOptions));
                }

                resultList.Add(digitalTwinsModelData);
            }

            logger.LogInformation($"Found {resultList.Count} model(s)");
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