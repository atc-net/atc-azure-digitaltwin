namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipDeleteCommand : AsyncCommand
{
    private readonly ILogger<RelationshipDeleteCommand> logger;

    public RelationshipDeleteCommand(
        ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<RelationshipDeleteCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogError("NOT IMPLEMENTED YET!");

        return Task.FromResult(ConsoleExitStatusCodes.Success);
    }
}