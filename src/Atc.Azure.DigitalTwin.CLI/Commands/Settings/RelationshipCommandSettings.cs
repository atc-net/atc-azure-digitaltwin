// ReSharper disable ConvertIfStatementToReturnStatement
namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public sealed class RelationshipCommandSettings : ConnectionBaseCommandSettings
{
    [CommandOption("-t|--twinId <TWINID>")]
    [Description("The id of the digital twin")]
    public string TwinId { get; set; } = string.Empty;

    [CommandOption("-r|--relationshipName <RELATIONSHIPNAME>")]
    [Description("The name of the relationship")]
    public string RelationshipName { get; set; } = string.Empty;

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

        if (string.IsNullOrEmpty(RelationshipName))
        {
            return ValidationResult.Error($"{nameof(RelationshipName)} is missing.");
        }

        return ValidationResult.Success();
    }
}