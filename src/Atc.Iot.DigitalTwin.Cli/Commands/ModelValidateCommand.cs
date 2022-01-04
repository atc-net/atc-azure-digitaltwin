namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class ModelValidateCommand : AsyncCommand<ModelPathSettings>
{
    private const string ListFormat = "{0,-80}{1}";
    private readonly IModelService modelService;
    private readonly ILogger<ModelValidateCommand> logger;

    public ModelValidateCommand(IModelService modelService, ILogger<ModelValidateCommand> logger)
    {
        this.modelService = modelService ?? throw new ArgumentNullException(nameof(modelService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override Task<int> ExecuteAsync(CommandContext context, ModelPathSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(ModelPathSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var directoryPath = settings.DirectoryPath;
        var directoryInfo = new DirectoryInfo(directoryPath);
        if (!await modelService.ValidateModels(directoryInfo))
        {
            logger.LogError($"Could not validate models from the specified folder '{directoryPath}'");
            return ConsoleExitStatusCodes.Failure;
        }

        var models = modelService.GetModels();

        logger.LogInformation("Loaded the following models:");

        foreach (DTInterfaceInfo @interface in models.Values)
        {
            @interface.DisplayName.TryGetValue("en", out var displayName);
            logger.LogInformation(string.Format(GlobalizationConstants.EnglishCultureInfo, ListFormat, @interface.Id.AbsoluteUri, displayName ?? "<none>"));
        }

        return ConsoleExitStatusCodes.Success;
    }
}