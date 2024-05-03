namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class EventRouteDeleteCommand : AsyncCommand
{
    private readonly ILogger<EventRouteDeleteCommand> logger;

    public EventRouteDeleteCommand(
        ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<EventRouteDeleteCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogError("NOT IMPLEMENTED YET!");

        return Task.FromResult(ConsoleExitStatusCodes.Success);
    }
}