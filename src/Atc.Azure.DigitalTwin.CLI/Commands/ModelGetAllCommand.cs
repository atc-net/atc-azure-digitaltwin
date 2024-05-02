namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelGetAllCommand : AsyncCommand<ConnectionBaseCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelGetAllCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelGetAllCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelGetAllCommand>();
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ConnectionBaseCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        ConnectionBaseCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                settings.AdtInstanceUrl!);

            var response = digitalTwinService.GetModels(new GetModelsOptions { IncludeModelDefinition = true });
            if (response is null)
            {
                logger.LogError("Failed to get models");
                return ConsoleExitStatusCodes.Failure;
            }

            var count = 0;
            await foreach (var digitalTwinsModelData in response)
            {
                logger.LogInformation($"ModelId: '{digitalTwinsModelData.Id}'");
                if (digitalTwinsModelData.DtdlModel != null)
                {
                    logger.LogInformation(JsonSerializer.Serialize(digitalTwinsModelData.DtdlModel, jsonSerializerOptions));
                }

                count++;
            }

            logger.LogInformation($"Found {count} model(s)");
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