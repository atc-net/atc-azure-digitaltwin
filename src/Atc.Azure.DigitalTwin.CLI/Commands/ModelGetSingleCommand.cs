namespace Atc.Azure.DigitalTwin.CLI.Commands;

public class ModelGetSingleCommand : AsyncCommand<ModelCommandSettings>
{
    private readonly DigitalTwinsClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private readonly ILogger<ModelGetSingleCommand> logger;

    public ModelGetSingleCommand(DigitalTwinsClient client, ILogger<ModelGetSingleCommand> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(CommandContext context, ModelCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(ModelCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var modelId = settings.ModelId;

        logger.LogInformation($"Getting Model with id '{modelId}'");

        try
        {
            var result = await client.GetModelAsync(modelId);
            if (result is null)
            {
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation(JsonSerializer.Serialize(result.Value, jsonSerializerOptions));

            logger.LogInformation("Successfully fetched model.");
            return ConsoleExitStatusCodes.Success;
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
    }
}