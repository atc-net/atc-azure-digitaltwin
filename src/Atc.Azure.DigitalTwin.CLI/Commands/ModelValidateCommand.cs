namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelValidateCommand : AsyncCommand<ModelPathSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelValidateCommand> logger;
    private readonly IModelRepositoryService modelRepositoryService;

    public ModelValidateCommand(
        ILoggerFactory loggerFactory,
        IModelRepositoryService modelRepositoryService)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelValidateCommand>();
        this.modelRepositoryService = modelRepositoryService;
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
        if (!await modelRepositoryService.ValidateModels(directoryInfo))
        {
            logger.LogError($"Could not validate models from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        var models = modelRepositoryService.GetModels();

        logger.LogInformation("Loaded the following models:");

        foreach (DTInterfaceInfo @interface in models.Values)
        {
            @interface.DisplayName.TryGetValue("en", out var displayName);

            //// TODO: Log
            //// Use the logger's built-in formatting capabilities, which avoid creating an intermediate string.
            ////logger.LogInformation("{0,-80}{1}", @interface.Id.AbsoluteUri, displayName ?? "<none>");
            //// Assuming @interface.Id.AbsoluteUri and displayName are safe to log
            ////logger.LogInformation(string.Format(GlobalizationConstants.EnglishCultureInfo, ListFormat, @interface.Id.AbsoluteUri, displayName ?? "<none>"));
        }

        return ConsoleExitStatusCodes.Success;
    }
}