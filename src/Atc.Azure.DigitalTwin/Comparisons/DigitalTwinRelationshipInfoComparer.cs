namespace Atc.Azure.DigitalTwin.Comparisons;

public sealed class DigitalTwinRelationshipInfoComparer : IEqualityComparer<DTRelationshipInfo?>
{
    // Relationships are equal if their names and targets are equal.
    public bool Equals(
        DTRelationshipInfo? x,
        DTRelationshipInfo? y)
    {
        // Check whether the compared objects reference the same data.
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null ||
            y is null)
        {
            return false;
        }

        return x.Name == y.Name && x.Target == y.Target;
    }

    // If Equals() returns true for a pair of objects
    // then GetHashCode() must return the same value for these objects.
    public int GetHashCode(DTRelationshipInfo? obj)
    {
        if (obj is null)
        {
            return 0;
        }

        // Get hash code for the Name field if it is not null.
        var hashPIName = obj.Name is null
            ? 0
            : obj.Name.GetHashCode(StringComparison.Ordinal);

        if (obj.Target is null)
        {
            return hashPIName;
        }

        var hashPITarget = obj.Target.GetHashCode();

        return hashPIName ^ hashPITarget;
    }
}