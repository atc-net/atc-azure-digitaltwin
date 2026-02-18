namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public sealed class EventRouteCreateCommandSettings : EventRouteCommandSettings
{
    [CommandOption("--endpointName <ENDPOINTNAME>")]
    [Description("The name of the endpoint to route events to")]
    public string EndpointName { get; set; } = string.Empty;

    [CommandOption("--filter <FILTER>")]
    [Description("An optional filter expression (defaults to 'true' for all events)")]
    public string? Filter { get; set; }

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(EndpointName)
            ? ValidationResult.Error($"{nameof(EndpointName)} is missing.")
            : ValidationResult.Success();
    }
}