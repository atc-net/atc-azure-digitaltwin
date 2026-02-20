namespace Atc.Azure.DigitalTwin.Tests.Comparisons;

public sealed class DigitalTwinInterfaceInfoComparerTests
{
    private const string DtdlModel1 = """
        {
            "@id": "dtmi:com:example:Thermostat;1",
            "@type": "Interface",
            "displayName": "Thermostat",
            "@context": "dtmi:dtdl:context;2"
        }
        """;

    private const string DtdlModel2 = """
        {
            "@id": "dtmi:com:example:Room;1",
            "@type": "Interface",
            "displayName": "Room",
            "@context": "dtmi:dtdl:context;2"
        }
        """;

    private readonly DigitalTwinInterfaceInfoComparer sut = new();

    [Fact]
    public async Task Equals_SameReference_ReturnsTrue()
    {
        // Arrange
        var parser = new ModelParser();
        var result = await parser.ParseAsync([DtdlModel1]);
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>().First();

        // Act & Assert
        sut.Equals(interfaceInfo, interfaceInfo)
            .Should().BeTrue();
    }

    [Fact]
    public async Task Equals_SameId_ReturnsTrue()
    {
        // Arrange
        var parser = new ModelParser();
        var result1 = await parser.ParseAsync([DtdlModel1]);
        var result2 = await parser.ParseAsync([DtdlModel1]);
        var interface1 = result1.Values.OfType<DTInterfaceInfo>().First();
        var interface2 = result2.Values.OfType<DTInterfaceInfo>().First();

        // Act & Assert
        sut.Equals(interface1, interface2)
            .Should().BeTrue();
    }

    [Fact]
    public async Task Equals_DifferentId_ReturnsFalse()
    {
        // Arrange
        var parser = new ModelParser();
        var result1 = await parser.ParseAsync([DtdlModel1]);
        var result2 = await parser.ParseAsync([DtdlModel2]);
        var interface1 = result1.Values.OfType<DTInterfaceInfo>().First();
        var interface2 = result2.Values.OfType<DTInterfaceInfo>().First();

        // Act & Assert
        sut.Equals(interface1, interface2)
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
        var parser = new ModelParser();
        var result = await parser.ParseAsync([DtdlModel1]);
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>().First();

        // Act & Assert
        sut.Equals(null, interfaceInfo)
            .Should().BeFalse();
    }

    [Fact]
    public async Task Equals_RightNull_ReturnsFalse()
    {
        // Arrange
        var parser = new ModelParser();
        var result = await parser.ParseAsync([DtdlModel1]);
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>().First();

        // Act & Assert
        sut.Equals(interfaceInfo, null)
            .Should().BeFalse();
    }

    [Fact]
    public async Task Equals_IsSymmetric()
    {
        // Arrange
        var parser = new ModelParser();
        var result1 = await parser.ParseAsync([DtdlModel1]);
        var result2 = await parser.ParseAsync([DtdlModel1]);
        var interface1 = result1.Values.OfType<DTInterfaceInfo>().First();
        var interface2 = result2.Values.OfType<DTInterfaceInfo>().First();

        // Act & Assert
        sut.Equals(interface1, interface2)
            .Should().Be(sut.Equals(interface2, interface1));
    }

    [Fact]
    public async Task GetHashCode_SameId_ReturnsSameHashCode()
    {
        // Arrange
        var parser = new ModelParser();
        var result1 = await parser.ParseAsync([DtdlModel1]);
        var result2 = await parser.ParseAsync([DtdlModel1]);
        var interface1 = result1.Values.OfType<DTInterfaceInfo>().First();
        var interface2 = result2.Values.OfType<DTInterfaceInfo>().First();

        // Act & Assert
        sut.GetHashCode(interface1)
            .Should().Be(sut.GetHashCode(interface2));
    }

    [Fact]
    public async Task GetHashCode_DifferentId_ReturnsDifferentHashCode()
    {
        // Arrange
        var parser = new ModelParser();
        var result1 = await parser.ParseAsync([DtdlModel1]);
        var result2 = await parser.ParseAsync([DtdlModel2]);
        var interface1 = result1.Values.OfType<DTInterfaceInfo>().First();
        var interface2 = result2.Values.OfType<DTInterfaceInfo>().First();

        // Act & Assert
        sut.GetHashCode(interface1)
            .Should().NotBe(sut.GetHashCode(interface2));
    }

    [Fact]
    public void GetHashCode_NullObj_ThrowsArgumentNullException()
    {
        // Act
        var act = () => sut.GetHashCode(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}