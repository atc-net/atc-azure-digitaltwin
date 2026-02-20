namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public class ImportJobCommandSettings : ConnectionBaseCommandSettings
{
    [CommandOption("--jobId <JOBID>")]
    [Description("The id of the import job")]
    public string JobId { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(JobId)
            ? ValidationResult.Error($"{nameof(JobId)} is missing.")
            : ValidationResult.Success();
    }
}