namespace Atc.Azure.DigitalTwin.CLI.Commands.Settings;

public class ConnectionBaseCommandSettings : BaseCommandSettings
{
    [CommandOption("--tenantId <TENANTID>")]
    [Description("Azure Tenant Id")]
    public string? TenantId { get; init; }

    [CommandOption("-a|--adtInstanceUrl <ADTINSTANCEURL>")]
    [Description("Azure Digital Twin Instance Url")]
    public string? AdtInstanceUrl { get; init; }

    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(TenantId))
        {
            return ValidationResult.Error($"{nameof(TenantId)} must be present.");
        }

        if (string.IsNullOrWhiteSpace(AdtInstanceUrl))
        {
            return ValidationResult.Error($"{nameof(AdtInstanceUrl)} must be present.");
        }

        return ValidationResult.Success();
    }
}