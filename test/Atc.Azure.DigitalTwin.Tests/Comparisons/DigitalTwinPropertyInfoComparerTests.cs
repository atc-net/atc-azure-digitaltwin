namespace Atc.Azure.DigitalTwin.Tests.Comparisons;

public sealed class DigitalTwinPropertyInfoComparerTests
{
    private const string DtdlModelWithProperties = """
        {
            "@id": "dtmi:com:example:Thermostat;1",
            "@type": "Interface",
            "displayName": "Thermostat",
            "contents": [
                {
                    "@type": "Property",
                    "name": "temperature",
                    "schema": "double"
                },
                {
                    "@type": "Property",
                    "name": "humidity",
                    "schema": "integer"
                }
            ],
            "@context": "dtmi:dtdl:context;2"
        }
        """;

    private readonly DigitalTwinPropertyInfoComparer sut = new();

    [Fact]
    public async Task Equals_SameReference_ReturnsTrue()
    {
        // Arrange
        var property = await GetFirstProperty();

        // Act & Assert
        sut.Equals(property, property)
            .Should().BeTrue();
    }

    [Fact]
    public async Task Equals_SameNameAndSchema_ReturnsTrue()
    {
        // Arrange
        var property1 = await GetFirstProperty();
        var property2 = await GetFirstProperty();

        // Act & Assert
        sut.Equals(property1, property2)
            .Should().BeTrue();
    }

    [Fact]
    public async Task Equals_DifferentNameOrSchema_ReturnsFalse()
    {
        // Arrange
        var properties = await GetProperties();
        var temperature = properties.First(p => p.Name == "temperature");
        var humidity = properties.First(p => p.Name == "humidity");

        // Act & Assert
        sut.Equals(temperature, humidity)
            .Should().BeFalse();
    }

    [Fact]
    public void Equals_BothNull_ReturnsTrue()
    {
        // Act & Assert
        sut.Equals(null, null)
            .Should().BeTrue();
    }

    [Fact]
    public async Task Equals_LeftNull_ReturnsFalse()
    {
        // Arrange
        var property = await GetFirstProperty();

        // Act & Assert
        sut.Equals(null, property)
            .Should().BeFalse();
    }

    [Fact]
    public async Task Equals_RightNull_ReturnsFalse()
    {
        // Arrange
        var property = await GetFirstProperty();

        // Act & Assert
        sut.Equals(property, null)
            .Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_NullObj_ReturnsZero()
    {
        // Act & Assert
        sut.GetHashCode(null)
            .Should().Be(0);
    }

    [Fact]
    public async Task GetHashCode_SameProperty_ReturnsSameHashCode()
    {
        // Arrange
        var property1 = await GetFirstProperty();
        var property2 = await GetFirstProperty();

        // Act & Assert
        sut.GetHashCode(property1)
            .Should().Be(sut.GetHashCode(property2));
    }

    [Fact]
    public async Task GetHashCode_DifferentProperty_ReturnsDifferentHashCode()
    {
        // Arrange
        var properties = await GetProperties();
        var temperature = properties.First(p => p.Name == "temperature");
        var humidity = properties.First(p => p.Name == "humidity");

        // Act & Assert
        sut.GetHashCode(temperature)
            .Should().NotBe(sut.GetHashCode(humidity));
    }

    [Fact]
    public async Task Equals_IsSymmetric()
    {
        // Arrange
        var property1 = await GetFirstProperty();
        var property2 = await GetFirstProperty();

        // Act & Assert
        sut.Equals(property1, property2)
            .Should().Be(sut.Equals(property2, property1));
    }

    [Fact]
    public async Task GetHashCode_CaseDifferingNames_ReturnsDifferentHashCode()
    {
        // Arrange - Create two properties that differ only in name casing
        var lowerCaseProperty = await GetPropertyFromModel("""
            {
                "@id": "dtmi:com:example:DeviceA;1",
                "@type": "Interface",
                "displayName": "DeviceA",
                "contents": [{ "@type": "Property", "name": "temperature", "schema": "double" }],
                "@context": "dtmi:dtdl:context;2"
            }
            """);

        var upperCaseProperty = await GetPropertyFromModel("""
            {
                "@id": "dtmi:com:example:DeviceB;1",
                "@type": "Interface",
                "displayName": "DeviceB",
                "contents": [{ "@type": "Property", "name": "Temperature", "schema": "double" }],
                "@context": "dtmi:dtdl:context;2"
            }
            """);

        // Act
        var areEqual = sut.Equals(lowerCaseProperty, upperCaseProperty);
        var hashCodeLower = sut.GetHashCode(lowerCaseProperty);
        var hashCodeUpper = sut.GetHashCode(upperCaseProperty);

        // Assert - Both Equals and GetHashCode use ordinal (case-sensitive) comparison
        areEqual.Should().BeFalse();
        hashCodeLower.Should().NotBe(hashCodeUpper);
    }

    private static async Task<DTPropertyInfo> GetFirstProperty()
    {
        var properties = await GetProperties();
        return properties[0];
    }

    private static async Task<DTPropertyInfo> GetPropertyFromModel(
        string dtdlModel)
    {
        var parser = new ModelParser();
        var result = await parser.ParseAsync([dtdlModel]);
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>().First();
        return interfaceInfo.Contents.Values.OfType<DTPropertyInfo>().First();
    }

    private static async Task<List<DTPropertyInfo>> GetProperties()
    {
        var parser = new ModelParser();
        var result = await parser.ParseAsync([DtdlModelWithProperties]);
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>().First();
        return [.. interfaceInfo.Contents.Values.OfType<DTPropertyInfo>()];
    }
}