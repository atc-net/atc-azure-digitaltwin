namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public sealed class RelationshipCreateCommandSettings : ConnectionBaseCommandSettings
{
    [CommandOption("--source-twinId <SOURCETWINID>")]
    [Description("The id of the source digital twin")]
    public string SourceTwinId { get; set; } = string.Empty;

    [CommandOption("--target-twinId <TARGETTWINID>")]
    [Description("The id of the target digital twin")]
    public string TargetTwinId { get; set; } = string.Empty;

    [CommandOption("--relationshipName <RELATIONSHIPNAME>")]
    [Description("The name of the relationship")]
    public string RelationshipName { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        if (string.IsNullOrEmpty(SourceTwinId))
        {
            return ValidationResult.Error($"{nameof(SourceTwinId)} is missing.");
        }

        if (string.IsNullOrEmpty(TargetTwinId))
        {
            return ValidationResult.Error($"{nameof(TargetTwinId)} is missing.");
        }

        if (string.IsNullOrEmpty(RelationshipName))
        {
            return ValidationResult.Error($"{nameof(RelationshipName)} is missing.");
        }

        return ValidationResult.Success();
    }
}