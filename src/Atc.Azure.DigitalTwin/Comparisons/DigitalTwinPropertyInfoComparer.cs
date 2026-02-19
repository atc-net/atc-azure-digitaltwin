namespace Atc.Azure.DigitalTwin.Comparisons;

public sealed class DigitalTwinPropertyInfoComparer : IEqualityComparer<DTPropertyInfo?>
{
    // Products are equal if their names and product numbers are equal.
    public bool Equals(
        DTPropertyInfo? x,
        DTPropertyInfo? y)
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

        // Check whether the products' properties are equal.
        return x.Name == y.Name && x.Schema == y.Schema;
    }

    // If Equals() returns true for a pair of objects
    // then GetHashCode() must return the same value for these objects.
    public int GetHashCode(DTPropertyInfo? obj)
    {
        if (obj is null)
        {
            return 0;
        }

        // Get hash code for the Name field if it is not null.
        var hashPIName = obj.Name is null
            ? 0
            : obj.Name.GetHashCode(StringComparison.Ordinal);

        // Get hash code for the Code field.
        var hashPISchema = obj.Schema.GetHashCode();

        // Calculate the hash code for the product.
        return hashPIName ^ hashPISchema;
    }
}