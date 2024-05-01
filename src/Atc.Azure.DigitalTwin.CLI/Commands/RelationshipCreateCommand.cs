namespace Atc.Azure.DigitalTwin.CLI.Commands;

public sealed class RelationshipCreateCommand : AsyncCommand
{
    private readonly ILogger<RelationshipCreateCommand> logger;

    public RelationshipCreateCommand(ILogger<RelationshipCreateCommand> logger)
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