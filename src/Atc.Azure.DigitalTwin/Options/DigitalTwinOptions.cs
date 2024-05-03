namespace Atc.Azure.DigitalTwin.Options;

public sealed class DigitalTwinOptions
{
    public string TenantId { get; set; } = string.Empty;

    public string InstanceUrl { get; set; } = string.Empty;

    public override string ToString()
        => $"{nameof(TenantId)}: {TenantId}, {nameof(InstanceUrl)}: {InstanceUrl}";
}