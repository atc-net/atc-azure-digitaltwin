namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class ModelGetAllCommand : AsyncCommand
{
    private readonly DigitalTwinsClient client;
    private readonly ILogger<ModelGetAllCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelGetAllCommand(DigitalTwinsClient client, ILogger<ModelGetAllCommand> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        try
        {
            var results = client.GetModelsAsync(new GetModelsOptions { IncludeModelDefinition = true });

            var resultList = new List<DigitalTwinsModelData>();
            await foreach (DigitalTwinsModelData md in results)
            {
                logger.LogInformation($"ModelId: '{md.Id}'");
                if (md.DtdlModel != null)
                {
                    logger.LogInformation(JsonSerializer.Serialize(md.DtdlModel, jsonSerializerOptions));
                }

                resultList.Add(md);
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