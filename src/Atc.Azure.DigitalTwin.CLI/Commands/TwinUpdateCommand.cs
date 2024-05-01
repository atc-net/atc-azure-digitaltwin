namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class TwinUpdateCommand : AsyncCommand
{
    private readonly ILogger<TwinUpdateCommand> logger;

    public TwinUpdateCommand(
        ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<TwinUpdateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogError("NOT IMPLEMENTED YET!");

        return Task.FromResult(ConsoleExitStatusCodes.Success);
    }
}