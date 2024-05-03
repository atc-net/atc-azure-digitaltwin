namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelCreateAllCommand : AsyncCommand<ModelPathSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelCreateAllCommand> logger;

    public ModelCreateAllCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelCreateAllCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ModelPathSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        ModelPathSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var directoryPath = settings.DirectoryPath;
        var directoryInfo = new DirectoryInfo(directoryPath);

        var modelRepositoryService = ModelRepositoryServiceFactory.Create(loggerFactory);

        if (!await modelRepositoryService.LoadModelContent(directoryInfo))
        {
            logger.LogError($"Could not load model from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                settings.AdtInstanceUrl!);

            var (succeeded, errorMessage) = await digitalTwinService.CreateModels(modelRepositoryService.GetModelsContent());
            if (!succeeded)
            {
                logger.LogError($"Failed to upload models: {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation("Successfully uploaded models");
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