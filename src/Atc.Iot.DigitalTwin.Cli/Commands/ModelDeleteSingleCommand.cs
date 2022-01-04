namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class ModelDeleteSingleCommand : AsyncCommand<ModelCommandSettings>
{
    private readonly DigitalTwinsClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private readonly ILogger<ModelDeleteSingleCommand> logger;

    public ModelDeleteSingleCommand(DigitalTwinsClient client, ILogger<ModelDeleteSingleCommand> logger)
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

        logger.LogInformation($"Deleting Model with id '{modelId}'");

        try
        {
            var result = await client.DeleteModelAsync(modelId);
            if (result is null)
            {
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation(JsonSerializer.Serialize(result, jsonSerializerOptions));

            logger.LogInformation("Successfully deleted model.");
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