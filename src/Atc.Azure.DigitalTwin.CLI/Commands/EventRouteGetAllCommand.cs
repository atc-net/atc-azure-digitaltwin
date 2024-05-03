namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class EventRouteGetAllCommand : AsyncCommand
{
    private readonly ILogger<EventRouteGetAllCommand> logger;

    public EventRouteGetAllCommand(
        ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<EventRouteGetAllCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogError("NOT IMPLEMENTED YET!");

        return Task.FromResult(ConsoleExitStatusCodes.Success);
    }
}