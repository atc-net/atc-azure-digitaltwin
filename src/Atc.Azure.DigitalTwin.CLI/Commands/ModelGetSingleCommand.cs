namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelGetSingleCommand : AsyncCommand<ModelCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelGetSingleCommand> logger;
    private readonly DigitalTwinsClient client;  // TODO: XXX
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelGetSingleCommand(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelGetSingleCommand>();
        this.client = client;
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ModelCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        ModelCommandSettings settings)
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
            logger.LogError(ex.GetLastInnerMessage());
            return ConsoleExitStatusCodes.Failure;
        }
    }
}