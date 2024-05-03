namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinCreateCommand : AsyncCommand
{
    private readonly ILogger<TwinCreateCommand> logger;

    public TwinCreateCommand(
        ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<TwinCreateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogError("NOT IMPLEMENTED YET!");

        return Task.FromResult(ConsoleExitStatusCodes.Success);
    }
}