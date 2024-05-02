namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

// TODO: Verify base-class
public class ModelCommandSettings : ConnectionBaseCommandSettings
{
    [CommandOption("-m|--modelId <MODELID>")]
    [Description("The id of the Model")]
    public string ModelId { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(ModelId)
            ? ValidationResult.Error("ModelId is missing.")
            : ValidationResult.Success();
    }
}