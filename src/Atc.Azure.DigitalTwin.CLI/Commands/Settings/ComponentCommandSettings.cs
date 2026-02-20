namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public class ComponentCommandSettings : TwinCommandSettings
{
    [CommandOption("-c|--componentName <COMPONENTNAME>")]
    [Description("The name of the component")]
    public string ComponentName { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(ComponentName)
            ? ValidationResult.Error($"{nameof(ComponentName)} is missing.")
            : ValidationResult.Success();
    }
}