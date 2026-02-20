namespace Atc.Azure.DigitalTwin.Tests.Comparisons;

public sealed class DigitalTwinRelationshipInfoComparerTests
{
    private const string DtdlModelWithRelationships = """
        [{
            "@id": "dtmi:com:example:Thermostat;1",
            "@type": "Interface",
            "displayName": "Thermostat",
            "@context": "dtmi:dtdl:context;2"
        },
        {
            "@id": "dtmi:com:example:Room;1",
            "@type": "Interface",
            "displayName": "Room",
            "contents": [
                {
                    "@type": "Relationship",
                    "name": "hasThermostat",
                    "target": "dtmi:com:example:Thermostat;1"
                },
                {
                    "@type": "Relationship",
                    "name": "hasSensor",
                    "target": "dtmi:com:example:Thermostat;1"
                }
            ],
            "@context": "dtmi:dtdl:context;2"
        }]
        """;

    private readonly DigitalTwinRelationshipInfoComparer sut = new();

    [Fact]
    public async Task Equals_SameReference_ReturnsTrue()
    {
        // Arrange
        var relationship = await GetFirstRelationship();

        // Act & Assert
        sut.Equals(relationship, relationship)
            .Should().BeTrue();
    }

    [Fact]
    public async Task Equals_SameNameAndTarget_ReturnsTrue()
    {
        // Arrange
        var relationship1 = await GetFirstRelationship();
        var relationship2 = await GetFirstRelationship();

        // Act & Assert
        sut.Equals(relationship1, relationship2)
            .Should().BeTrue();
    }

    [Fact]
    public async Task Equals_DifferentName_ReturnsFalse()
    {
        // Arrange
        var relationships = await GetRelationships();
        var first = relationships[0];
        var second = relationships[1];

        // Act & Assert
        sut.Equals(first, second)
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
        var relationship = await GetFirstRelationship();

        // Act & Assert
        sut.Equals(null, relationship)
            .Should().BeFalse();
    }

    [Fact]
    public async Task Equals_RightNull_ReturnsFalse()
    {
        // Arrange
        var relationship = await GetFirstRelationship();

        // Act & Assert
        sut.Equals(relationship, null)
            .Should().BeFalse();
    }

    [Fact]
    public async Task Equals_IsSymmetric()
    {
        // Arrange
        var relationship1 = await GetFirstRelationship();
        var relationship2 = await GetFirstRelationship();

        // Act & Assert
        sut.Equals(relationship1, relationship2)
            .Should().Be(sut.Equals(relationship2, relationship1));
    }

    [Fact]
    public void GetHashCode_NullObj_ReturnsZero()
    {
        // Act & Assert
        sut.GetHashCode(null)
            .Should().Be(0);
    }

    [Fact]
    public async Task GetHashCode_SameRelationship_ReturnsSameHashCode()
    {
        // Arrange
        var relationship1 = await GetFirstRelationship();
        var relationship2 = await GetFirstRelationship();

        // Act & Assert
        sut.GetHashCode(relationship1)
            .Should().Be(sut.GetHashCode(relationship2));
    }

    [Fact]
    public async Task GetHashCode_DifferentRelationship_ReturnsDifferentHashCode()
    {
        // Arrange
        var relationships = await GetRelationships();
        var first = relationships[0];
        var second = relationships[1];

        // Act & Assert
        sut.GetHashCode(first)
            .Should().NotBe(sut.GetHashCode(second));
    }

    [Fact]
    public async Task GetHashCode_CaseDifferingNames_ReturnsDifferentHashCode()
    {
        // Arrange - Create two relationships that differ only in name casing
        const string lowerCaseModel = """
            [{
                "@id": "dtmi:com:example:Target;1",
                "@type": "Interface",
                "displayName": "Target",
                "@context": "dtmi:dtdl:context;2"
            },
            {
                "@id": "dtmi:com:example:SourceA;1",
                "@type": "Interface",
                "displayName": "SourceA",
                "contents": [{ "@type": "Relationship", "name": "hasTarget", "target": "dtmi:com:example:Target;1" }],
                "@context": "dtmi:dtdl:context;2"
            }]
            """;

        const string upperCaseModel = """
            [{
                "@id": "dtmi:com:example:Target;1",
                "@type": "Interface",
                "displayName": "Target",
                "@context": "dtmi:dtdl:context;2"
            },
            {
                "@id": "dtmi:com:example:SourceB;1",
                "@type": "Interface",
                "displayName": "SourceB",
                "contents": [{ "@type": "Relationship", "name": "HasTarget", "target": "dtmi:com:example:Target;1" }],
                "@context": "dtmi:dtdl:context;2"
            }]
            """;

        var lowerCaseRel = await GetRelationshipFromModel(
            lowerCaseModel,
            "SourceA");

        var upperCaseRel = await GetRelationshipFromModel(
            upperCaseModel,
            "SourceB");

        // Act
        var areEqual = sut.Equals(lowerCaseRel, upperCaseRel);
        var hashCodeLower = sut.GetHashCode(lowerCaseRel);
        var hashCodeUpper = sut.GetHashCode(upperCaseRel);

        // Assert - Both Equals and GetHashCode use ordinal (case-sensitive) comparison
        areEqual.Should().BeFalse();
        hashCodeLower.Should().NotBe(hashCodeUpper);
    }

    private static async Task<DTRelationshipInfo> GetFirstRelationship()
    {
        var relationships = await GetRelationships();
        return relationships[0];
    }

    private static async Task<DTRelationshipInfo> GetRelationshipFromModel(
        string dtdlModel,
        string sourceInterfaceName)
    {
        var parser = new ModelParser();
        var result = await parser.ParseAsync([dtdlModel]);
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>()
            .First(i => i.Id.AbsoluteUri.Contains(sourceInterfaceName, StringComparison.Ordinal));
        return interfaceInfo.Contents.Values.OfType<DTRelationshipInfo>().First();
    }

    private static async Task<List<DTRelationshipInfo>> GetRelationships()
    {
        var parser = new ModelParser();
        var result = await parser.ParseAsync([DtdlModelWithRelationships]);
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>()
            .First(i => i.Id.AbsoluteUri.Contains("Room", StringComparison.Ordinal));
        return [.. interfaceInfo.Contents.Values.OfType<DTRelationshipInfo>()];
    }
}