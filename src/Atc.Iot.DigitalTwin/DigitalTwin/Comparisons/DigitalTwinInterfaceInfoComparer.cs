namespace Atc.Iot.DigitalTwin.DigitalTwin.Comparisons;

public sealed class DigitalTwinInterfaceInfoComparer : IEqualityComparer<DTInterfaceInfo>
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