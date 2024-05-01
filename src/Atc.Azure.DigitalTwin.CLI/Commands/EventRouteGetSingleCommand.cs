namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class EventRouteGetSingleCommand : AsyncCommand
{
    private readonly ILogger<EventRouteGetSingleCommand> logger;

    public EventRouteGetSingleCommand(ILogger<EventRouteGetSingleCommand> logger)
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