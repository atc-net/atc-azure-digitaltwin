namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public sealed class TwinUpdateCommandSettings : TwinCommandSettings
{
    [CommandOption("--jsonPatch <JSONPATCH>")]
    [Description("The JSON patch document to apply to the twin")]
    public string JsonPatch { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(JsonPatch)
            ? ValidationResult.Error($"{nameof(JsonPatch)} is missing.")
            : ValidationResult.Success();
    }
}