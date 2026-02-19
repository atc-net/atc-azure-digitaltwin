namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinUpdateCommand : AsyncCommand<TwinUpdateCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TwinUpdateCommand> logger;

    public TwinUpdateCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<TwinUpdateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        TwinUpdateCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        TwinUpdateCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var twinId = settings.TwinId;
        var jsonPatch = settings.JsonPatch;

        logger.LogInformation($"Updating twin with id '{twinId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var (patchDocument, buildError) = BuildPatchDocument(jsonPatch);
            if (patchDocument is null)
            {
                logger.LogError(buildError);
                return ConsoleExitStatusCodes.Failure;
            }

            var (succeeded, errorMessage) = await digitalTwinService.UpdateTwinAsync(
                twinId,
                patchDocument,
                cancellationToken: cancellationToken);

            if (!succeeded)
            {
                logger.LogError($"Failed to update twin '{twinId}': {errorMessage}");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Successfully updated twin '{twinId}'");
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
        var patchArray = JsonDocument.Parse(jsonPatch).RootElement;

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