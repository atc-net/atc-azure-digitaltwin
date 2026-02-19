namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelGetAllCommand : AsyncCommand<ConnectionBaseCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelGetAllCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ModelGetAllCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelGetAllCommand>();
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ConnectionBaseCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ConnectionBaseCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var models = await digitalTwinService.GetModels(new GetModelsOptions { IncludeModelDefinition = true }, cancellationToken);
            if (models is null)
            {
                logger.LogError("Failed to get models");
                return ConsoleExitStatusCodes.Failure;
            }

            foreach (var digitalTwinsModelData in models)
            {
                logger.LogInformation($"ModelId: '{digitalTwinsModelData.Id}'");
                if (digitalTwinsModelData.DtdlModel != null)
                {
                    logger.LogInformation(JsonSerializer.Serialize(digitalTwinsModelData.DtdlModel, jsonSerializerOptions));
                }
            }

            logger.LogInformation($"Found {models.Count} model(s)");
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