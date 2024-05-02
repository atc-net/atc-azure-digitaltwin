namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipCreateCommand : AsyncCommand
{
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<RelationshipCreateCommand> logger;

    public RelationshipCreateCommand(
        ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<RelationshipCreateCommand>();
    }

    public override Task<int> ExecuteAsync(
        CommandContext context)
    {
        ConsoleHelper.WriteHeader();

        logger.LogError("NOT IMPLEMENTED YET!");

        return Task.FromResult(ConsoleExitStatusCodes.Success);
    }
}