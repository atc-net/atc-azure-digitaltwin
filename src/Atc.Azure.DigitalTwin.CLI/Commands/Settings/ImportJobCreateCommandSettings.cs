namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public sealed class ImportJobCreateCommandSettings : ImportJobCommandSettings
{
    [CommandOption("--inputBlobUri <INPUTBLOBURI>")]
    [Description("The URI of the input blob containing NDJSON import data")]
    public string InputBlobUri { get; set; } = string.Empty;

    [CommandOption("--outputBlobUri <OUTPUTBLOBURI>")]
    [Description("The URI of the output blob for import job logs")]
    public string OutputBlobUri { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        if (string.IsNullOrEmpty(InputBlobUri))
        {
            return ValidationResult.Error($"{nameof(InputBlobUri)} is missing.");
        }

        return string.IsNullOrEmpty(OutputBlobUri)
            ? ValidationResult.Error($"{nameof(OutputBlobUri)} is missing.")
            : ValidationResult.Success();
    }
}