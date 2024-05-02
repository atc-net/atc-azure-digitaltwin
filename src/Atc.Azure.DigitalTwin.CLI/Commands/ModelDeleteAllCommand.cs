// ReSharper disable SuggestBaseTypeForParameter
namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ModelDeleteAllCommand : AsyncCommand<ConnectionBaseCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ModelDeleteAllCommand> logger;
    private readonly IDigitalTwinParser dtdlParser;

    public ModelDeleteAllCommand(
        ILoggerFactory loggerFactory,
        IDigitalTwinParser dtdlParser)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ModelDeleteAllCommand>();
        this.dtdlParser = dtdlParser;
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ConnectionBaseCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        ConnectionBaseCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                settings.AdtInstanceUrl!);

            var jsonModels = await GetTwinModelsAsJson(digitalTwinService);

            var (succeeded, interfaceEntities) = await dtdlParser.ParseAsync(jsonModels);
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
                        await digitalTwinService.DeleteModel(del.Id.ToString());
                        logger.LogInformation($"Successfully deleted model {del.Id}");
                    }
                    catch (RequestFailedException ex)
                    {
                        logger.LogError($"Error deleting model {ex.Status}: {ex.GetLastInnerMessage()}");
                    }
                }
            }
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

    private async Task<List<string>> GetTwinModelsAsJson(
        DigitalTwinService digitalTwinService)
    {
        var jsonModelTexts = new List<string>();

        var response = digitalTwinService.GetModels(new GetModelsOptions { IncludeModelDefinition = true });
        if (response is null)
        {
            return jsonModelTexts;
        }

        await foreach (var digitalTwinsModelData in response)
        {
            if (digitalTwinsModelData.DtdlModel is null)
            {
                continue;
            }

            logger.LogInformation(digitalTwinsModelData.Id);
            jsonModelTexts.Add(digitalTwinsModelData.DtdlModel);
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