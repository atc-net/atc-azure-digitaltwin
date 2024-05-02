namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelGetSingleCommand : AsyncCommand<ModelCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelGetSingleCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelGetSingleCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelGetSingleCommand>();
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
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                settings.AdtInstanceUrl!);

            var model = await digitalTwinService.GetModel(modelId);
            if (model is null)
            {
                logger.LogError($"Failed to fetch model '{modelId}'");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation("Successfully fetched model.");
            logger.LogInformation(JsonSerializer.Serialize(model, jsonSerializerOptions));

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