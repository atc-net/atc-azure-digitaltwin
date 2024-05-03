namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public class RelationshipCommandSettings : BaseCommandSettings
{
    [CommandOption("-r|--relationshipId <RELATIONSHIPID>")]
    [Description("The id of the relationship")]
    public string RelationshipId { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(RelationshipId)
            ? ValidationResult.Error($"{nameof(RelationshipId)} is missing.")
            : ValidationResult.Success();
    }
}