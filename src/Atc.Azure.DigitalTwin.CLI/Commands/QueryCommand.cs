namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class QueryCommand : AsyncCommand<QueryCommandSettings>
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<QueryCommand> logger;

    public QueryCommand(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<QueryCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context,
        QueryCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return ExecuteInternalAsync(settings, cancellationToken);
    }

    private async Task<int> ExecuteInternalAsync(
        QueryCommandSettings settings,
        CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader();

        logger.LogInformation($"Executing query: {settings.Query}");

        try
        {
            var digitalTwinService = DigitalTwinServiceFactory.Create(
                loggerFactory,
                settings.TenantId!,
                new Uri(settings.AdtInstanceUrl!));

            var results = await digitalTwinService.QueryAsync<BasicDigitalTwin>(settings.Query, cancellationToken);
            if (results is null)
            {
                logger.LogWarning("Query returned no results");
                return ConsoleExitStatusCodes.Failure;
            }

            logger.LogInformation($"Query returned {results.Count} result(s)");

            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            foreach (var twin in results)
            {
                var json = JsonSerializer.Serialize(twin, jsonOptions);
                logger.LogInformation(json);
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
}