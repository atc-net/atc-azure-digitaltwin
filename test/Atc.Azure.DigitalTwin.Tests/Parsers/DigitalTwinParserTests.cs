namespace Atc.Azure.DigitalTwin.Tests.Parsers;

public sealed class DigitalTwinParserTests
{
    private const string ValidDtdlModel = """
        {
            "@id": "dtmi:com:example:Thermostat;1",
            "@type": "Interface",
            "displayName": "Thermostat",
            "contents": [
                {
                    "@type": "Property",
                    "name": "temperature",
                    "schema": "double"
                }
            ],
            "@context": "dtmi:dtdl:context;2"
        }
        """;

    private readonly DigitalTwinParser sut;

    public DigitalTwinParserTests()
    {
        sut = new DigitalTwinParser(NullLoggerFactory.Instance);
    }

    [Fact]
    public async Task Parse_ValidModel_ReturnsSucceededTrue()
    {
        // Act
        var result = await sut.ParseAsync([ValidDtdlModel]);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Interfaces.Should().NotBeNull();
        result.Interfaces.Should().ContainKey(new Dtmi("dtmi:com:example:Thermostat;1"));
    }

    [Fact]
    public async Task Parse_ValidModel_ReturnsInterfaces()
    {
        // Act
        var (_, interfaces) = await sut.ParseAsync([ValidDtdlModel]);

        // Assert
        interfaces.Should().NotBeNullOrEmpty();
        interfaces!.Values.Should().Contain(e => e.EntityKind == DTEntityKind.Interface);
    }

    [Fact]
    public async Task Parse_InvalidModel_ReturnsSucceededFalse()
    {
        // Arrange
        const string invalidModel = """{ "invalid": "not a dtdl model" }""";

        // Act
        var (succeeded, interfaces) = await sut.ParseAsync([invalidModel]);

        // Assert
        succeeded.Should().BeFalse();
        interfaces.Should().BeNull();
    }

    [Fact]
    public async Task Parse_MalformedJson_ReturnsSucceededFalse()
    {
        // Arrange
        var malformedJson = "{ this is not json }";

        // Act
        var (succeeded, interfaces) = await sut.ParseAsync([malformedJson]);

        // Assert
        succeeded.Should().BeFalse();
        interfaces.Should().BeNull();
    }

    [Fact]
    public async Task Parse_EmptyCollection_ReturnsSucceededTrue()
    {
        // Act
        var (succeeded, interfaces) = await sut.ParseAsync([]);

        // Assert
        succeeded.Should().BeTrue();
        interfaces.Should().NotBeNull();
        interfaces.Should().BeEmpty();
    }

    [Fact]
    public async Task Parse_MultipleValidModels_ReturnsAllInterfaces()
    {
        // Arrange
        const string model1 = """
            {
                "@id": "dtmi:com:example:Thermostat;1",
                "@type": "Interface",
                "displayName": "Thermostat",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        const string model2 = """
            {
                "@id": "dtmi:com:example:Room;1",
                "@type": "Interface",
                "displayName": "Room",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        // Act
        var (succeeded, interfaces) = await sut.ParseAsync([model1, model2]);

        // Assert
        succeeded.Should().BeTrue();
        interfaces.Should().NotBeNull();
        interfaces!.Keys.Should().Contain(new Dtmi("dtmi:com:example:Thermostat;1"));
        interfaces.Keys.Should().Contain(new Dtmi("dtmi:com:example:Room;1"));
    }

    [Fact]
    public async Task Parse_DependentModels_BothOrdersSucceed()
    {
        // Arrange - Room extends Thermostat via relationship
        const string baseModel = """
            {
                "@id": "dtmi:com:example:Sensor;1",
                "@type": "Interface",
                "displayName": "Sensor",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        const string dependentModel = """
            {
                "@id": "dtmi:com:example:Room;1",
                "@type": "Interface",
                "displayName": "Room",
                "contents": [
                    {
                        "@type": "Relationship",
                        "name": "hasSensor",
                        "target": "dtmi:com:example:Sensor;1"
                    }
                ],
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        // Act - Parse with dependent model listed BEFORE its dependency
        // The underlying ModelParser handles resolution internally,
        // but there is no topological sorting in the upload pipeline.
        var (succeededReverse, interfacesReverse) = await sut.ParseAsync([dependentModel, baseModel]);
        var (succeededNormal, interfacesNormal) = await sut.ParseAsync([baseModel, dependentModel]);

        // Assert - Both orders parse successfully (parser resolves internally)
        succeededReverse.Should().BeTrue();
        interfacesReverse.Should().NotBeNull();
        succeededNormal.Should().BeTrue();
        interfacesNormal.Should().NotBeNull();
    }
}