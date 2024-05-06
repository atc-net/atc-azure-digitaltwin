namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinCreateCommand : AsyncCommand<TwinCreateCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinCreateCommand> logger;

    public TwinCreateCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinCreateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        TwinCreateCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        TwinCreateCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var twinId = settings.TwinId;
        var modelId = settings.ModelId;
        var modelVersion = settings.ModelVersion;
        var jsonPayload = settings.JsonPayload;

        var inputData = JsonDocument.Parse(jsonPayload).RootElement.Clone();
        var twinData = MergeWithTwinModelBase(inputData, modelId, modelVersion);

        logger.LogInformation($"Creating twin with id '{twinId}' on model with id '{modelId}' on version '{modelVersion}' with payload '{jsonPayload}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                settings.AdtInstanceUrl!);

            var (succeeded, errorMessage) = await digitalTwinService.CreateOrReplaceDigitalTwin(
                twinId,
                twinData);

            if (!succeeded)
            {
                logger.LogError($"Failed to create twin: {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation("Successfully created twin.");
            return ConsoleExitStatusCodes.Success;
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
    }

    private static ExpandoObject MergeWithTwinModelBase(
        JsonElement inputData,
        string modelId,
        int modelVersion)
    {
        dynamic mergedObject = new ExpandoObject();
        var dict = (IDictionary<string, object>)mergedObject;

        // Directly create $metadata without unnecessary serialization
        var metadata = new ExpandoObject() as IDictionary<string, object>;
        metadata["$model"] = $"{modelId};{modelVersion}";
        dict["$metadata"] = metadata;

        // Load predefined base properties from TwinModelBase, skipping metadata since we've already handled it
        var baseProperties = new TwinModelBase(modelId, modelVersion);
        var baseJson = JsonSerializer.Serialize(baseProperties);
        var baseData = JsonDocument.Parse(baseJson).RootElement;

        // Skip $metadata from baseProperties if it's included there, add other properties to mergedObject
        foreach (var property in baseData.EnumerateObject())
        {
            if (property.Name == "$metadata")
            {
                continue;
            }

            var jsonElementToObject = JsonElementToObject(property.Value);
            if (jsonElementToObject is not null)
            {
                dict[property.Name] = jsonElementToObject;
            }
        }

        foreach (var property in inputData.EnumerateObject())
        {
            var jsonElementToObject = JsonElementToObject(property.Value);
            if (jsonElementToObject is not null)
            {
                dict[property.Name] = jsonElementToObject;
            }
        }

        return mergedObject;
    }

    private static object? JsonElementToObject(
        JsonElement element)
        => element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True or JsonValueKind.False => element.GetBoolean(),
            JsonValueKind.Object or JsonValueKind.Array => element.GetRawText(),
            _ => null
        };
}