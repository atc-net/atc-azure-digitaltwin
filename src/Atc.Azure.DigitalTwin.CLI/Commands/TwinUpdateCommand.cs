namespace Atc.Azure.DigitalTwin.CLI.Commands;

public class TwinUpdateCommand : AsyncCommand
{
    private readonly ILogger<TwinUpdateCommand> logger;

    public TwinUpdateCommand(ILogger<TwinUpdateCommand> logger)
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