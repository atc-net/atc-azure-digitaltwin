namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public class QueryCommandSettings : ConnectionBaseCommandSettings
{
    [CommandOption("-q|--query <QUERY>")]
    [Description("The ADT query to execute")]
    public string Query { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        var validationResult = base.Validate();
        if (!validationResult.Successful)
        {
            return validationResult;
        }

        return string.IsNullOrEmpty(Query)
            ? ValidationResult.Error($"{nameof(Query)} is missing.")
            : ValidationResult.Success();
    }
}