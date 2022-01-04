namespace Atc.Iot.DigitalTwin.Cli.Domain.DigitalTwin.Comparisons;

public class DigitalTwinInterfaceInfoComparer : IEqualityComparer<DTInterfaceInfo>
{
    public bool Equals(DTInterfaceInfo? x, DTInterfaceInfo? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
        {
            return false;
        }

        return x.Id == y.Id;
    }

    public int GetHashCode(DTInterfaceInfo obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return obj.Id.GetHashCode();
    }
}