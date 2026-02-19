namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipGetAllCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipGetAllCommand> logger;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public RelationshipGetAllCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipGetAllCommand>();
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        TwinCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        TwinCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        var twinId = settings.TwinId;

        logger.LogInformation($"Getting relationships for twin with id '{twinId}'");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var relationships = await digitalTwinService.GetRelationships(twinId, cancellationToken: cancellationToken);
            if (relationships is null)
            {
                logger.LogError($"No relationships found for twin '{twinId}'");
                return ConsoleExitStatusCodes.Failure;
            }

            foreach (var relationship in relationships)
            {
                logger.LogInformation(JsonSerializer.Serialize(relationship, jsonSerializerOptions));
            }

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
}