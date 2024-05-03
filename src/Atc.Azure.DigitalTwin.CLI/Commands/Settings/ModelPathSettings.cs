namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public class ModelPathSettings : BaseCommandSettings
{
    [CommandOption("-d|--directoryPath <DIRECTORYPATH>")]
    [Description("Path to files (directory)")]
    public string DirectoryPath { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(DirectoryPath)
            ? ValidationResult.Error($"{nameof(DirectoryPath)} is missing.")
            : ValidationResult.Success();
    }
}