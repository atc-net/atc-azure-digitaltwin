namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipGetAllCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipGetAllCommand> logger;
    private readonly DigitalTwinsClient client; // TODO: XXX
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public RelationshipGetAllCommand(
        ILoggerFactory loggerFactory,
        DigitalTwinsClient client)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipGetAllCommand>();
        this.client = client;
        jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        TwinCommandSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(
        TwinCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var twinId = settings.TwinId;

        logger.LogInformation($"Getting relationships for twin with id '{twinId}'");

        try
        {
            var relationships = client.GetRelationshipsAsync<BasicRelationship>(twinId);
            await foreach (var relationship in relationships)
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