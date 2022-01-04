namespace Atc.Iot.DigitalTwin.Cli.Commands;

public class RelationshipDeleteCommand : AsyncCommand
{
    private readonly ILogger<RelationshipDeleteCommand> logger;

    public RelationshipDeleteCommand(ILogger<RelationshipDeleteCommand> logger)
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