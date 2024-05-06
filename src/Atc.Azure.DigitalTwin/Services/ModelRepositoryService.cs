// ReSharper disable SuggestBaseTypeForParameter
namespace Atc.Azure.DigitalTwin.Services;

/// <summary>
/// Service for managing digital twin models locally.
/// </summary>
public sealed partial class ModelRepositoryService : IModelRepositoryService
{
    private readonly IDigitalTwinParser dtdlParser;

    public ModelRepositoryService(
        ILoggerFactory loggerFactory,
        IDigitalTwinParser dtdlParser)
    {
        logger = loggerFactory.CreateLogger<ModelRepositoryService>();
        this.dtdlParser = dtdlParser;
    }

    private readonly List<string> modelsContent = [];

    private Dictionary<Dtmi, DTInterfaceInfo> Models { get; set; } = [];

    public void AddModel(
        Dtmi key,
        DTInterfaceInfo value)
        => Models.Add(key, value);

    public IEnumerable<string> GetModelsContent()
        => modelsContent;

    public IDictionary<Dtmi, DTInterfaceInfo> GetModels()
        => Models;

    public void Clear()
    {
        Models.Clear();
        modelsContent.Clear();
    }

    public Task<bool> LoadModelContent(
        DirectoryInfo path)
    {
        ArgumentNullException.ThrowIfNull(path);
        return LoadModelContentInternal(path);
    }

    private async Task<bool> LoadModelContentInternal(
        DirectoryInfo path)
    {
        if (!path.Exists)
        {
            LogUnknownDirectoryPath(path.FullName);
            return false;
        }

        var jsonFiles = Directory
            .GetFiles(path.FullName, "*.json", SearchOption.AllDirectories)
            .ToArray();

        foreach (var fileName in jsonFiles)
        {
            modelsContent.Add(await File.ReadAllTextAsync(fileName));
        }

        LogModelsLoaded(path.FullName);
        return true;
    }

    public async Task<bool> ValidateModels(
        DirectoryInfo path)
    {
        if (!await LoadModelContent(path))
        {
            return false;
        }

        try
        {
            await ParseAndStoreModels(modelsContent);
            Clear();
        }
        catch (ParsingException pe)
        {
            LogParseFailed(pe.GetLastInnerMessage());
            foreach (var error in pe.Errors)
            {
                LogParseError($"Message: {error.Message}, PrimaryID: {error.PrimaryID}, SecondaryID: {error.SecondaryID}, Property: {error.Property}");
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
    private async Task ParseAndStoreModels(
        IEnumerable<string> modelTexts)
    {
        var (succeeded, interfaceEntities) = await dtdlParser.Parse(modelTexts);
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
        }
    }
}