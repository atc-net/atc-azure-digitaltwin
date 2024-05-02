namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelGetAllCommand : AsyncCommand
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelGetAllCommand> logger;
    private readonly DigitalTwinsClient client; // TODO: XXX
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelGetAllCommand(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelGetAllCommand>();
        this.client = client;
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
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