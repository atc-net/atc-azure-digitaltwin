namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelDeleteAllCommand : AsyncCommand
{
    private readonly DigitalTwinsClient client;
    private readonly IDigitalTwinParser dtdlParser;
    private readonly ILogger<ModelDeleteAllCommand> logger;

    public ModelDeleteAllCommand(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client,
        IDigitalTwinParser dtdlParser)
    {
        logger = loggerFactory.CreateLogger<ModelDeleteAllCommand>();
        this.client = client;
        this.dtdlParser = dtdlParser;
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        try
        {
            var jsonModelTexts = await GetTwinModelsAsJsonAsync();

            var (succeeded, interfaceEntities) = await dtdlParser.ParseAsync(jsonModelTexts);
            if (!succeeded)
            {
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation("Models parsed successfully. Deleting models...");

            var interfacesToDelete = GetInterfacesToDelete(interfaceEntities);
            var pass = 1;

            while (interfacesToDelete.Count > 0)
            {
                logger.LogInformation($"Model deletion pass {pass++}");

                var toDelete = GetInterfacesWhichCanBeDeleted(interfacesToDelete);
                foreach (var del in toDelete)
                {
                    interfacesToDelete.Remove(del);

                    try
                    {
                        await client.DeleteModelAsync(del.Id.ToString());
                        logger.LogInformation($"Model {del.Id} deleted successfully");
                    }
                    catch (RequestFailedException e)
                    {
                        logger.LogError($"Error deleting model {e.Status}: {e.Message}");
                    }
                }
            }
        }
        catch (RequestFailedException e)
        {
            logger.LogError($"Error {e.Status}: {e.Message}");
            return ConsoleExitStatusCodes.Failure;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return ConsoleExitStatusCodes.Failure;
        }

        return ConsoleExitStatusCodes.Success;
    }

    private async Task<List<string>> GetTwinModelsAsJsonAsync()
    {
        var jsonModelTexts = new List<string>();
        var results = client.GetModelsAsync(new GetModelsOptions { IncludeModelDefinition = true });
        await foreach (var md in results)
        {
            if (md.DtdlModel == null)
            {
                continue;
            }

            logger.LogInformation(md.Id);
            jsonModelTexts.Add(md.DtdlModel);
        }

        logger.LogInformation($"Found {jsonModelTexts.Count} model(s)");
        return jsonModelTexts;
    }

    /// <summary>
    /// We can only delete models that are not in the inheritance chain of other models
    /// or used as components by other models. Therefore, we use the model parser to parse the DTDL
    /// and then find the "leaf" models, and delete these.
    /// We repeat this process until no models are left.
    /// </summary>
    private List<DTInterfaceInfo> GetInterfacesWhichCanBeDeleted(
        List<DTInterfaceInfo> interfacesToDelete)
    {
        var referencedInterfaces = GetInterfacesWithReferences(interfacesToDelete);

        var toDelete = new List<DTInterfaceInfo>();
        foreach (var @interface in interfacesToDelete)
        {
            if (referencedInterfaces.TryGetValue(@interface.Id, out _))
            {
                continue;
            }

            logger.LogInformation($"Can delete {@interface.Id}");
            toDelete.Add(@interface);
        }

        return toDelete;
    }

    private static Dictionary<Dtmi, DTInterfaceInfo> GetInterfacesWithReferences(
        List<DTInterfaceInfo> interfacesToDelete)
    {
        var referenced = new Dictionary<Dtmi, DTInterfaceInfo>();
        foreach (var @interface in interfacesToDelete)
        {
            foreach (var ext in @interface.Extends)
            {
                referenced.TryAdd(ext.Id, ext);
            }

            var components = from content
                             in @interface.Contents.Values
                             where content.EntityKind == DTEntityKind.Component
                             select content as DTComponentInfo;

            foreach (var componentSchema in components.Select(x => x.Schema))
            {
                referenced.TryAdd(componentSchema.Id, componentSchema);
            }
        }

        return referenced;
    }

    private static List<DTInterfaceInfo> GetInterfacesToDelete(
        IReadOnlyDictionary<Dtmi, DTEntityInfo>? interfaceEntities)
    {
        var interfaces = from entity in interfaceEntities!.Values
            where entity.EntityKind == DTEntityKind.Interface
            select entity as DTInterfaceInfo;

        return interfaces.ToList();
    }
}