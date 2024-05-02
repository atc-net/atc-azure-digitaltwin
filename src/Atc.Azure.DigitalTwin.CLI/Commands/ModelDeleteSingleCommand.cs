namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelDeleteSingleCommand : AsyncCommand<ModelCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelDeleteSingleCommand> logger;
    private readonly DigitalTwinsClient client; // TODO: XXX
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelDeleteSingleCommand(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelDeleteSingleCommand>();
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
    }
}