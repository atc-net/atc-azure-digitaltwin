using Atc.Iot.DigitalTwin.DigitalTwin.Parsers;

namespace Atc.Iot.DigitalTwin.DigitalTwin.Services;

public class ModelService : IModelService
{
    private readonly IDigitalTwinParser dtdlParser;
    private readonly ILogger<ModelService> logger;

    public ModelService(IDigitalTwinParser dtdlParser, ILogger<ModelService> logger)
    {
        this.dtdlParser = dtdlParser ?? throw new ArgumentNullException(nameof(dtdlParser));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private readonly List<string> modelsContent = new ();

    private IDictionary<Dtmi, DTInterfaceInfo> Models { get; set; } = new Dictionary<Dtmi, DTInterfaceInfo>();

    public void AddModel(Dtmi key, DTInterfaceInfo value) => Models.Add(key, value);

    public IEnumerable<string> GetModelsContent() => modelsContent;

    public IDictionary<Dtmi, DTInterfaceInfo> GetModels() => Models;

    public void Clear() => Models.Clear();

    public Task<bool> LoadModelContentAsync(DirectoryInfo path)
    {
        ArgumentNullException.ThrowIfNull(path);
        return LoadModelContentInternalAsync(path);
    }

    private async Task<bool> LoadModelContentInternalAsync(DirectoryInfo path)
    {
        if (!path.Exists)
        {
            logger.LogError("*** DirectoryPath does not exist.");
            return false;
        }

        logger.LogInformation($"Reading files from {path.FullName}");
        logger.LogInformation(string.Empty);

        var jsonFiles = Directory
            .GetFiles(path.FullName, "*.json", SearchOption.AllDirectories)
            .ToArray();

        foreach (var fileName in jsonFiles)
        {
            modelsContent.Add(await File.ReadAllTextAsync(fileName));
            logger.LogInformation($"Loaded {fileName}");
        }

        logger.LogInformation(string.Empty);
        logger.LogInformation("Files loaded.");
        Console.WriteLine();

        return true;
    }

    public async Task<bool> ValidateModels(DirectoryInfo path)
    {
        if (!await LoadModelContentAsync(path))
        {
            return false;
        }

        try
        {
            logger.LogInformation("Parsing Models.");
            logger.LogInformation(string.Empty);
            await ParseAndStoreModels(modelsContent);
        }
        catch (ParsingException pe)
        {
            logger.LogError("*** Error parsing models");
            var errorCount = 1;
            foreach (var err in pe.Errors)
            {
                logger.LogError($"Error {errorCount}:");
                logger.LogError($"{err.Message}");
                logger.LogError($"Primary ID: {err.PrimaryID}");
                logger.LogError($"Secondary ID: {err.SecondaryID}");
                logger.LogError($"Property: {err.Property}\n");
                errorCount++;
            }

            return false;
        }

        return true;
    }

    /// <summary>
    /// Parses and stores the models.
    /// </summary>
    /// <remarks>
    /// Store only the newly loaded interfaces.
    /// Because the entities returned from ParseAsync contains
    /// more than just interfaces and also any entities loaded by the resolver:
    ///  - Filter to just interfaces
    ///  - Exclude interfaces that were loaded by the resolver.
    /// </remarks>
    /// <param name="modelTexts">The model texts.</param>
    private async Task ParseAndStoreModels(IEnumerable<string> modelTexts)
    {
        var (succeeded, interfaceEntities) = await dtdlParser.ParseAsync(modelTexts);
        if (!succeeded)
        {
            return;
        }

        var interfaces = from entity in interfaceEntities!.Values
                         where entity.EntityKind == DTEntityKind.Interface
                         select entity as DTInterfaceInfo;

        foreach (var @interface in interfaces)
        {
            AddModel(@interface.Id, @interface);
            logger.LogInformation($"Successfully parsed Interface '{@interface.Id.AbsoluteUri}'");
        }

        Console.WriteLine();
    }
}