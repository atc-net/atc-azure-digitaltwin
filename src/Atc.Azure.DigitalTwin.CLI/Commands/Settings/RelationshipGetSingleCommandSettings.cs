// ReSharper disable ConvertIfStatementToReturnStatement
namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public sealed class RelationshipGetSingleCommandSettings : ConnectionBaseCommandSettings
{
    [CommandOption("-t|--twinId <TWINID>")]
    [Description("The id of the digital twin")]
    public string TwinId { get; set; } = string.Empty;

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

        if (string.IsNullOrEmpty(TwinId))
        {
            return ValidationResult.Error($"{nameof(TwinId)} is missing.");
        }

        if (string.IsNullOrEmpty(RelationshipId))
        {
            return ValidationResult.Error($"{nameof(RelationshipId)} is missing.");
        }

        return ValidationResult.Success();
    }
}