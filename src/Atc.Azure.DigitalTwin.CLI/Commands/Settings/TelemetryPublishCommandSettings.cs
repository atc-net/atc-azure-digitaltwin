namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public class TelemetryPublishCommandSettings : TwinCommandSettings
{
    [CommandOption("-p|--payload <PAYLOAD>")]
    [Description("The telemetry payload as a JSON string")]
    public string Payload { get; set; } = string.Empty;

    [CommandOption("-c|--componentName <COMPONENTNAME>")]
    [Description("Optional component name for component telemetry")]
    public string? ComponentName { get; init; }

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(Payload)
            ? ValidationResult.Error($"{nameof(Payload)} is missing.")
            : ValidationResult.Success();
    }
}