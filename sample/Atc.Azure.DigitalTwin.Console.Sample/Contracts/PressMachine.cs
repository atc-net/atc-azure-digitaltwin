namespace Atc.Azure.DigitalTwin.Console.Sample.Contracts;

public class PressMachine() : TwinModelBase(Names.PressMachineModelId, Names.PressMachineVersion)
{
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;

    [JsonPropertyName("serialNumber")]
    public string SerialNumber { get; set; } = string.Empty;

    [JsonPropertyName("operationalStatus")]
    public string OperationalStatus { get; set; } = string.Empty;

    [JsonPropertyName("maintenanceSchedule")]
    public string MaintenanceSchedule { get; set; } = string.Empty;
}