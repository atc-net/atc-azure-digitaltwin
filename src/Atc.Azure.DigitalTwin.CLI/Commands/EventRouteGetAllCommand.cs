namespace Atc.Azure.DigitalTwin.CLI.Commands;

public class EventRouteGetAllCommand : AsyncCommand
{
    private readonly ILogger<EventRouteGetAllCommand> logger;

    public EventRouteGetAllCommand(ILogger<EventRouteGetAllCommand> logger)
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