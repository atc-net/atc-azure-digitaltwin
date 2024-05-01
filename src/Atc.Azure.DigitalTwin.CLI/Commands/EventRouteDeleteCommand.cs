namespace Atc.Azure.DigitalTwin.CLI.Commands;

public class EventRouteDeleteCommand : AsyncCommand
{
    private readonly ILogger<EventRouteDeleteCommand> logger;

    public EventRouteDeleteCommand(ILogger<EventRouteDeleteCommand> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override Task<int> ExecuteAsync(CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogError("NOT IMPLEMENTED YET!");

        return Task.FromResult(ConsoleExitStatusCodes.Success);
    }
}