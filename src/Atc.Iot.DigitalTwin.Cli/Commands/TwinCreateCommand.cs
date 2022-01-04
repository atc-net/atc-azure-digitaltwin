namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class TwinCreateCommand : AsyncCommand
{
    private readonly ILogger<TwinCreateCommand> logger;

    public TwinCreateCommand(ILogger<TwinCreateCommand> logger)
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