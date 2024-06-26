namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public class TwinCommandSettings : ConnectionBaseCommandSettings
{
    [CommandOption("-t|--twinId <TWINID>")]
    [Description("The id of the digital twin")]
    public string TwinId { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(TwinId)
            ? ValidationResult.Error($"{nameof(TwinId)} is missing.")
            : ValidationResult.Success();
    }
}