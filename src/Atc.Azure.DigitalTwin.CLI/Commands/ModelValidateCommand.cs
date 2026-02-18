namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelValidateCommand : AsyncCommand<ModelPathSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelValidateCommand> logger;

    public ModelValidateCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelValidateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ModelPathSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ModelPathSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var directoryPath = settings.DirectoryPath;
        var directoryInfo = new DirectoryInfo(directoryPath);

        var modelRepositoryService = ModelRepositoryServiceFactory.Create(loggerFactory);

        if (!await modelRepositoryService.ValidateModels(directoryInfo, cancellationToken))
        {
            logger.LogError($"Could not validate models from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        var models = modelRepositoryService.GetModels();

        logger.LogInformation("Loaded the following models:");

        foreach (var interfaceInfo in models.Values)
        {
            interfaceInfo.DisplayName.TryGetValue("en", out var displayName);

            logger.LogInformation($"{interfaceInfo.Id.AbsoluteUri} - {displayName ?? "<none>"}");
        }

        return ConsoleExitStatusCodes.Success;
    }
}