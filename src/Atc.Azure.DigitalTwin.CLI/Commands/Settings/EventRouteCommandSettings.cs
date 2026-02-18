namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public class EventRouteCommandSettings : ConnectionBaseCommandSettings
{
    [CommandOption("-e|--eventRouteId <EVENTROUTEID>")]
    [Description("The id of the event route")]
    public string EventRouteId { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(EventRouteId)
            ? ValidationResult.Error($"{nameof(EventRouteId)} is missing.")
            : ValidationResult.Success();
    }
}