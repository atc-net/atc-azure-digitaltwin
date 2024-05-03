namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class EventRouteCreateCommand : AsyncCommand
{
    private readonly ILogger<EventRouteCreateCommand> logger;

    public EventRouteCreateCommand(
        ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<EventRouteCreateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogError("NOT IMPLEMENTED YET!");

        return Task.FromResult(ConsoleExitStatusCodes.Success);
    }
}