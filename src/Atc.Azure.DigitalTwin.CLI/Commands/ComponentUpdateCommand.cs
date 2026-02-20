namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class ComponentUpdateCommand : AsyncCommand<ComponentUpdateCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<ComponentUpdateCommand> logger;

    public ComponentUpdateCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ComponentUpdateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        ComponentUpdateCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        ComponentUpdateCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var twinId = settings.TwinId;
        var componentName = settings.ComponentName;

        logger.LogInformation($"Updating component '{componentName}' for twin '{twinId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var (patchDocument, buildError) = BuildPatchDocument(settings.JsonPatch);
            if (patchDocument is null)
            {
                logger.LogError(buildError);
                return ConsoleExitStatusCodes.Failure;
            }

            var (succeeded, errorMessage) = await digitalTwinService.UpdateComponentAsync(
                twinId,
                componentName,
                patchDocument,
                cancellationToken: cancellationToken);

            if (!succeeded)
            {
                logger.LogError($"Failed to update component '{componentName}' for twin '{twinId}': {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Successfully updated component '{componentName}' for twin '{twinId}'");
            return ConsoleExitStatusCodes.Success;
        }
        catch (System.Text.Json.JsonException ex)
        {
            logger.LogError($"Invalid JSON patch document: {ex.Message}");
            return ConsoleExitStatusCodes.Failure;
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

    private static (JsonPatchDocument? Document, string? ErrorMessage) BuildPatchDocument(
        string jsonPatch)
    {
        var patchDocument = new JsonPatchDocument();
        using var patchDoc = JsonDocument.Parse(jsonPatch);
        var patchArray = patchDoc.RootElement;

        foreach (var operation in patchArray.EnumerateArray())
        {
            var op = operation.GetProperty("op").GetString();
            var path = operation.GetProperty("path").GetString()!;

            switch (op)
            {
                case "replace":
                    patchDocument.AppendReplace(path, operation.GetProperty("value"));
                    break;
                case "add":
                    patchDocument.AppendAdd(path, operation.GetProperty("value"));
                    break;
                case "remove":
                    patchDocument.AppendRemove(path);
                    break;
                default:
                    return (null, $"Unsupported patch operation '{op}'");
            }
        }

        return (patchDocument, null);
    }
}