namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class RelationshipGetAllCommand : AsyncCommand<TwinCommandSettings>
{
    private readonly DigitalTwinsClient client;
    private readonly JsonSerializerOptions jsonSerializerOptions;
    private readonly ILogger<RelationshipGetAllCommand> logger;

    public RelationshipGetAllCommand(DigitalTwinsClient client, ILogger<RelationshipGetAllCommand> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.jsonSerializerOptions = JsonSerializerOptionsFactory.Create();
    }

    public override Task<int> ExecuteAsync(CommandContext context, TwinCommandSettings settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        return ExecuteInternalAsync(settings);
    }

    private async Task<int> ExecuteInternalAsync(TwinCommandSettings settings)
    {
        ConsoleHelper.WriteHeader();

        var digitalTwinId = settings.TwinId;
        logger.LogInformation($"Getting relationships for Digital Twin with id '{digitalTwinId}'");

        try
        {
            var relationships = client.GetRelationshipsAsync<BasicRelationship>(digitalTwinId);
            await foreach (var relationship in relationships)
            {
                logger.LogInformation(JsonSerializer.Serialize(relationship, jsonSerializerOptions));
            }

            return ConsoleExitStatusCodes.Success;
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
    }
}