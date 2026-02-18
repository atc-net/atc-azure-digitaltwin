namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public sealed class TwinCreateCommandSettings : TwinCommandSettings
{
    [CommandOption("-m|--modelId <MODELID>")]
    [Description("The id of the Model")]
    public string ModelId { get; set; } = string.Empty;

    [CommandOption("--modelVersion <MODELVERSION>")]
    [Description("The version of the Model")]
    public int ModelVersion { get; set; }

    [CommandOption("--jsonPayload <JSONPAYLOAD>")]
    [Description("The serialized type to create as twin")]
    public string JsonPayload { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        if (string.IsNullOrEmpty(ModelId))
        {
            return ValidationResult.Error($"{nameof(ModelId)} is missing.");
        }

        if (ModelVersion <= 0)
        {
            return ValidationResult.Error($"{nameof(ModelVersion)} must be positive number.");
        }

        if (string.IsNullOrEmpty(JsonPayload))
        {
            return ValidationResult.Error($"{nameof(JsonPayload)} is missing.");
        }

        return ValidationResult.Success();
    }
}