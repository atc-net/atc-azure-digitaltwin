namespace Atc.Iot.DigitalTwin.Cli.Commands.Settings;

public class ModelUploadSingleSettings : ModelCommandSettings
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
            ? ValidationResult.Error("DirectoryPath is missing.")
            : ValidationResult.Success();
    }
}