namespace Atc.Azure.DigitalTwin.Console.Sample.Contracts;

public class TwinModelBase
{
    [JsonPropertyName(DigitalTwinsJsonPropertyNames.DigitalTwinMetadata)]
    public DigitalTwinMetadata Metadata { get; set; } = new ();

    [JsonIgnore]
    public ETag? ETag
        => ETagAsString is not null
            ? new ETag(ETagAsString)
            : null;

    [JsonPropertyName("$etag")]
    public string? ETagAsString { get; set; }

    public TwinModelBase(
        string modelTwinId,
        int version)
        => Metadata.ModelId = $"{modelTwinId};{version}";
}