namespace Atc.Iot.DigitalTwin.Cli.Domain.DigitalTwin.Comparisons;

public class DigitalTwinPropertyInfoComparer : IEqualityComparer<DTPropertyInfo?>
{
    // Products are equal if their names and product numbers are equal.
    public bool Equals(DTPropertyInfo? x, DTPropertyInfo? y)
    {
        // Check whether the compared objects reference the same data.
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        // Check whether any of the compared objects is null.
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
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
        // Check whether the object is null
        if (ReferenceEquals(obj, null))
        {
            return 0;
        }

        // Get hash code for the Name field if it is not null.
        var hashPIName = obj.Name == null ? 0 : obj.Name.GetHashCode(StringComparison.OrdinalIgnoreCase);

        // Get hash code for the Code field.
        var hashPISchema = obj.Schema.GetHashCode();

        // Calculate the hash code for the product.
        return hashPIName ^ hashPISchema;
    }
}