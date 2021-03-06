namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class EventRouteCreateCommand : AsyncCommand
{
    private readonly ILogger<EventRouteCreateCommand> logger;

    public EventRouteCreateCommand(ILogger<EventRouteCreateCommand> logger)
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