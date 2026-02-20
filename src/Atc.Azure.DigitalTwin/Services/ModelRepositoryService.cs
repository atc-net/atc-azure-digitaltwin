// ReSharper disable SuggestBaseTypeForParameter
namespace Atc.Azure.DigitalTwin.Services;

/// <summary>
/// Service for managing digital twin models locally.
/// </summary>
/// <remarks>
/// This service is NOT thread-safe and should be registered as transient or scoped in dependency injection.
/// </remarks>
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

    public IEnumerable<string> GetModelsContentInDependencyOrder()
    {
        if (modelsContent.Count <= 1)
        {
            return modelsContent;
        }

        try
        {
            return TopologicalSortByExtends(modelsContent);
        }
        catch (InvalidOperationException)
        {
            LogDependencyOrderingFailed();
            return modelsContent;
        }
    }

    public IDictionary<Dtmi, DTInterfaceInfo> GetModels()
        => Models;

    public void Clear()
    {
        Models.Clear();
        modelsContent.Clear();
    }

    public Task<bool> LoadModelContentAsync(
        DirectoryInfo path,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(path);
        return LoadModelContentInternalAsync(path, cancellationToken);
    }

    private async Task<bool> LoadModelContentInternalAsync(
        DirectoryInfo path,
        CancellationToken cancellationToken)
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
            modelsContent.Add(await File.ReadAllTextAsync(fileName, cancellationToken));
        }

        LogModelsLoaded(path.FullName);
        return true;
    }

    public async Task<bool> ValidateModelsAsync(
        DirectoryInfo path,
        CancellationToken cancellationToken = default)
    {
        Clear();

        if (!await LoadModelContentAsync(path, cancellationToken))
        {
            return false;
        }

        try
        {
            var parseSucceeded = await ParseAndStoreModelsAsync(modelsContent);

            if (!parseSucceeded)
            {
                LogParseFailed("Model parsing failed");
                return false;
            }
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
    private async Task<bool> ParseAndStoreModelsAsync(
        IEnumerable<string> modelTexts)
    {
        var (succeeded, interfaceEntities) = await dtdlParser.ParseAsync(modelTexts);
        if (!succeeded)
        {
            return false;
        }

        var interfaces = from entity in interfaceEntities!.Values
                         where entity.EntityKind == DTEntityKind.Interface
                         select entity as DTInterfaceInfo;

        foreach (var @interface in interfaces)
        {
            AddModel(@interface.Id, @interface);
        }

        return true;
    }

    private static List<string> TopologicalSortByExtends(List<string> models)
    {
        var dependencies = new Dictionary<int, List<int>>();
        var inDegree = new int[models.Count];

        var (modelIndex, unparseable) = BuildModelIndex(models, dependencies);
        BuildDependencyEdges(models, modelIndex, unparseable, dependencies, inDegree);

        return ExecuteKahnSort(models, dependencies, inDegree, unparseable);
    }

    private static (Dictionary<string, int> ModelIndex, HashSet<int> Unparseable) BuildModelIndex(
        List<string> models,
        Dictionary<int, List<int>> dependencies)
    {
        var modelIndex = new Dictionary<string, int>(StringComparer.Ordinal);
        var unparseable = new HashSet<int>();

        for (var i = 0; i < models.Count; i++)
        {
            dependencies[i] = [];

            try
            {
                using var doc = JsonDocument.Parse(models[i]);
                var root = doc.RootElement;

                if (root.ValueKind != JsonValueKind.Object)
                {
                    unparseable.Add(i);
                    continue;
                }

                if (root.TryGetProperty("@id", out var idProp))
                {
                    var id = idProp.GetString();
                    if (id is not null)
                    {
                        modelIndex[id] = i;
                    }
                    else
                    {
                        unparseable.Add(i);
                    }
                }
                else
                {
                    unparseable.Add(i);
                }
            }
            catch (System.Text.Json.JsonException)
            {
                unparseable.Add(i);
            }
        }

        return (modelIndex, unparseable);
    }

    private static void BuildDependencyEdges(
        List<string> models,
        Dictionary<string, int> modelIndex,
        HashSet<int> unparseable,
        Dictionary<int, List<int>> dependencies,
        int[] inDegree)
    {
        for (var i = 0; i < models.Count; i++)
        {
            if (unparseable.Contains(i))
            {
                continue;
            }

            try
            {
                using var doc = JsonDocument.Parse(models[i]);
                if (!doc.RootElement.TryGetProperty("extends", out var extendsProp))
                {
                    continue;
                }

                var extendIds = ParseExtendsProperty(extendsProp);

                foreach (var extendId in extendIds)
                {
                    if (modelIndex.TryGetValue(extendId, out var depIdx))
                    {
                        dependencies[depIdx].Add(i);
                        inDegree[i]++;
                    }
                }
            }
            catch (System.Text.Json.JsonException)
            {
                // Defensive: cannot happen for strings that passed JSON parsing in BuildModelIndex
            }
        }
    }

    private static List<string> ParseExtendsProperty(JsonElement extendsProp)
    {
        var extendIds = new List<string>();

        if (extendsProp.ValueKind == JsonValueKind.String)
        {
            var val = extendsProp.GetString();
            if (val is not null)
            {
                extendIds.Add(val);
            }
        }
        else if (extendsProp.ValueKind == JsonValueKind.Array)
        {
            foreach (var element in extendsProp.EnumerateArray())
            {
                var val = element.GetString();
                if (val is not null)
                {
                    extendIds.Add(val);
                }
            }
        }

        return extendIds;
    }

    private static List<string> ExecuteKahnSort(
        List<string> models,
        Dictionary<int, List<int>> dependencies,
        int[] inDegree,
        HashSet<int> unparseable)
    {
        var queue = new Queue<int>();
        for (var i = 0; i < models.Count; i++)
        {
            if (!unparseable.Contains(i) && inDegree[i] == 0)
            {
                queue.Enqueue(i);
            }
        }

        var sorted = new List<string>();
        var visited = 0;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            sorted.Add(models[current]);
            visited++;

            foreach (var dependent in dependencies[current])
            {
                inDegree[dependent]--;
                if (inDegree[dependent] == 0)
                {
                    queue.Enqueue(dependent);
                }
            }
        }

        var parsedCount = models.Count - unparseable.Count;
        if (visited != parsedCount)
        {
            throw new InvalidOperationException("Circular dependency detected among DTDL models.");
        }

        foreach (var idx in unparseable.OrderBy(x => x))
        {
            sorted.Add(models[idx]);
        }

        return sorted;
    }
}