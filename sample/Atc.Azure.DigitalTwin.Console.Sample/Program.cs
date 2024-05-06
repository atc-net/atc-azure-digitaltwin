var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
    .Build();

var digitalTwinOptions = new DigitalTwinOptions();
configuration.GetRequiredSection("DigitalTwinOptions").Bind(digitalTwinOptions);

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

var logger = loggerFactory.CreateLogger("Program");
using var cts = new CancellationTokenSource();

var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
var targetDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\.."));
var modelsPath = new DirectoryInfo(Path.Combine(targetDirectory, "models"));

var modelRepositoryService = new ModelRepositoryService(
    loggerFactory,
    new DigitalTwinParser(loggerFactory));

var modelsValid = await modelRepositoryService.ValidateModels(modelsPath);
if (!modelsValid)
{
    logger.LogError("Failed to validate models");
    return -1;
}

var loadedModelContent = await modelRepositoryService.LoadModelContent(modelsPath);
if (!loadedModelContent)
{
    logger.LogError($"Failed to load models from {modelsPath.FullName}");
    return -1;
}

var digitalTwinService = new DigitalTwinService(
    loggerFactory,
    new DigitalTwinsClient(
        new Uri(digitalTwinOptions.InstanceUrl),
        digitalTwinOptions.GetTokenCredential()));

var twinsModelData = await digitalTwinService.GetModel(
    $"{Names.PressMachineModelId};{Names.PressMachineVersion}",
    cts.Token);

if (twinsModelData is null)
{
    var (succeeded, errorMessage) = await digitalTwinService.CreateModels(
        modelRepositoryService.GetModelsContent(),
        cts.Token);

    if (!succeeded)
    {
        logger.LogError($"Failed to upload models: {errorMessage}");
        return -1;
    }
}

var modelsResponse = digitalTwinService.GetModels(new GetModelsOptions { IncludeModelDefinition = true }, cts.Token);
if (modelsResponse is not null)
{
    await foreach (var digitalTwinsModelData in modelsResponse)
    {
        if (digitalTwinsModelData.DtdlModel is null)
        {
            continue;
        }

        logger.LogInformation(digitalTwinsModelData.Id);
        logger.LogInformation(digitalTwinsModelData.DtdlModel);
    }
}

const string twinId = "my-twin-id";

var pressMachine = new PressMachine
{
    Manufacturer = "Acme Corp",
    SerialNumber = "SN123456",
    OperationalStatus = "Running",
    MaintenanceSchedule = "Every second day",
};

var (createTwinSucceeded, createTwinErrorMessage) = await digitalTwinService.CreateOrReplaceDigitalTwin(
    twinId,
    pressMachine,
    cts.Token);

if (!createTwinSucceeded)
{
    logger.LogError($"Failed to create twin: {createTwinErrorMessage}");
    return -1;
}

var twinList = await digitalTwinService.GetTwins("SELECT * FROM DIGITALTWINS", cts.Token);

if (twinList is not null)
{
    var groupedTwinList = twinList
        .GroupBy(x => x.Metadata.ModelId, StringComparer.Ordinal)
        .ToList();

    foreach (var group in groupedTwinList)
    {
        logger.LogInformation($"{group.ToList().Count} twin(s) based on model '{group.Key}'.");
    }
}

var (deleteTwinSucceeded, deleteTwinErrorMessage) = await digitalTwinService.DeleteTwin(
    twinId,
    cancellationToken: cts.Token);

if (!deleteTwinSucceeded)
{
    logger.LogError($"Failed to delete twin: {deleteTwinErrorMessage}");
    return -1;
}

return 0;