namespace Atc.Azure.DigitalTwin.Tests.Services;

public sealed class ModelRepositoryServiceTests
{
    private const string ValidDtdlModel = """
        {
            "@id": "dtmi:com:example:Thermostat;1",
            "@type": "Interface",
            "displayName": "Thermostat",
            "@context": "dtmi:dtdl:context;2"
        }
        """;

    private readonly ModelRepositoryService sut;

    public ModelRepositoryServiceTests()
    {
        var mockParser = Substitute.For<IDigitalTwinParser>();
        sut = new ModelRepositoryService(NullLoggerFactory.Instance, mockParser);
    }

    [Fact]
    public async Task AddModel_SingleModel_CanBeRetrieved()
    {
        // Arrange
        var parser = new ModelParser();
        var result = await parser.ParseAsync(new[] { ValidDtdlModel });
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>().First();
        var dtmi = new Dtmi("dtmi:com:example:Thermostat;1");

        // Act
        sut.AddModel(dtmi, interfaceInfo);

        // Assert
        sut.GetModels().Should().ContainKey(dtmi);
        sut.GetModels()[dtmi].Should().Be(interfaceInfo);
    }

    [Fact]
    public void GetModels_InitiallyEmpty()
    {
        // Act & Assert
        sut.GetModels().Should().BeEmpty();
    }

    [Fact]
    public void GetModelsContent_InitiallyEmpty()
    {
        // Act & Assert
        sut.GetModelsContent().Should().BeEmpty();
    }

    [Fact]
    public async Task Clear_RemovesBothModelsAndContent()
    {
        // Arrange
        var parser = new ModelParser();
        var result = await parser.ParseAsync(new[] { ValidDtdlModel });
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>().First();
        var dtmi = new Dtmi("dtmi:com:example:Thermostat;1");
        sut.AddModel(dtmi, interfaceInfo);

        // Act
        sut.Clear();

        // Assert
        sut.GetModels().Should().BeEmpty();
        sut.GetModelsContent().Should().BeEmpty();
    }

    [Fact]
    public async Task AddModel_MultipleDifferentModels_AllRetrievable()
    {
        // Arrange
        const string model2 = """
            {
                "@id": "dtmi:com:example:Room;1",
                "@type": "Interface",
                "displayName": "Room",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        var parser = new ModelParser();
        var result1 = await parser.ParseAsync(new[] { ValidDtdlModel });
        var result2 = await parser.ParseAsync(new[] { model2 });
        var interface1 = result1.Values.OfType<DTInterfaceInfo>().First();
        var interface2 = result2.Values.OfType<DTInterfaceInfo>().First();

        var dtmi1 = new Dtmi("dtmi:com:example:Thermostat;1");
        var dtmi2 = new Dtmi("dtmi:com:example:Room;1");

        // Act
        sut.AddModel(dtmi1, interface1);
        sut.AddModel(dtmi2, interface2);

        // Assert
        sut.GetModels().Should().HaveCount(2);
        sut.GetModels().Should().ContainKey(dtmi1);
        sut.GetModels().Should().ContainKey(dtmi2);
    }

    [Fact]
    public async Task AddModel_DuplicateKey_ThrowsArgumentException()
    {
        // Arrange
        var parser = new ModelParser();
        var result = await parser.ParseAsync(new[] { ValidDtdlModel });
        var interfaceInfo = result.Values.OfType<DTInterfaceInfo>().First();
        var dtmi = new Dtmi("dtmi:com:example:Thermostat;1");

        sut.AddModel(dtmi, interfaceInfo);

        // Act
        var act = () => sut.AddModel(dtmi, interfaceInfo);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public Task LoadModelContent_NullPath_ThrowsArgumentNullException()
        => Assert.ThrowsAsync<ArgumentNullException>(() => sut.LoadModelContentAsync(null!, TestContext.Current.CancellationToken));

    [Fact]
    public async Task LoadModelContent_NonExistentDirectory_ReturnsFalse()
    {
        // Arrange
        var path = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

        // Act
        var result = await sut.LoadModelContentAsync(path, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task LoadModelContent_EmptyDirectory_ReturnsTrue()
    {
        // Arrange
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");

        try
        {
            // Act
            var result = await sut.LoadModelContentAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Assert
            result.Should().BeTrue();
            sut.GetModelsContent().Should().BeEmpty();
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task LoadModelContent_DirectoryWithJsonFiles_LoadsContent()
    {
        // Arrange
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");
        var filePath = Path.Combine(tempDir.FullName, "model.json");
        await File.WriteAllTextAsync(filePath, ValidDtdlModel, TestContext.Current.CancellationToken);

        try
        {
            // Act
            var result = await sut.LoadModelContentAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Assert
            result.Should().BeTrue();
            sut.GetModelsContent().Should().HaveCount(1);
            sut.GetModelsContent().First().Should().Be(ValidDtdlModel);
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task LoadModelContent_CalledMultipleTimes_AccumulatesContent()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        // BUG/Design issue: LoadModelContent does not clear previous content,
        // so calling it multiple times accumulates model content.
        var tempDir1 = Directory.CreateTempSubdirectory("dtdl-test1-");
        var tempDir2 = Directory.CreateTempSubdirectory("dtdl-test2-");

        const string model2 = """
            {
                "@id": "dtmi:com:example:Room;1",
                "@type": "Interface",
                "displayName": "Room",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        await File.WriteAllTextAsync(
            Path.Combine(tempDir1.FullName, "model1.json"),
            ValidDtdlModel,
            TestContext.Current.CancellationToken);
        await File.WriteAllTextAsync(
            Path.Combine(tempDir2.FullName, "model2.json"),
            model2,
            TestContext.Current.CancellationToken);

        try
        {
            // Act
            await sut.LoadModelContentAsync(new DirectoryInfo(tempDir1.FullName), cancellationToken);
            await sut.LoadModelContentAsync(new DirectoryInfo(tempDir2.FullName), cancellationToken);

            // Assert - Content accumulates across calls
            sut.GetModelsContent().Should().HaveCount(2);
        }
        finally
        {
            tempDir1.Delete(recursive: true);
            tempDir2.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task LoadModelContent_DirectoryWithNoJsonFiles_ReturnsTrueWithEmptyContent()
    {
        // Arrange - Directory with a non-JSON file
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");
        await File.WriteAllTextAsync(
            Path.Combine(tempDir.FullName, "readme.txt"),
            "not a json file",
            TestContext.Current.CancellationToken);

        try
        {
            // Act
            var result = await sut.LoadModelContentAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Assert
            result.Should().BeTrue();
            sut.GetModelsContent().Should().BeEmpty();
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task ValidateModels_WithValidModels_ModelsAvailableAfterValidation()
    {
        // Arrange
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");
        await File.WriteAllTextAsync(
            Path.Combine(tempDir.FullName, "model.json"),
            ValidDtdlModel,
            TestContext.Current.CancellationToken);

        // Set up with real parser to validate end-to-end
        var realParser = new DigitalTwinParser(NullLoggerFactory.Instance);
        var serviceWithRealParser = new ModelRepositoryService(NullLoggerFactory.Instance, realParser);

        try
        {
            // Act
            var result = await serviceWithRealParser.ValidateModelsAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Assert - Validation succeeds and models are available
            result.Should().BeTrue();
            serviceWithRealParser.GetModels().Should().NotBeEmpty();
            serviceWithRealParser.GetModelsContent().Should().NotBeEmpty();
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task ValidateModels_WithInvalidDtdl_ReturnsFalse()
    {
        // Arrange
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");
        await File.WriteAllTextAsync(
            Path.Combine(tempDir.FullName, "invalid.json"),
            """{ "invalid": "not a dtdl model" }""",
            TestContext.Current.CancellationToken);

        var realParser = new DigitalTwinParser(NullLoggerFactory.Instance);
        var serviceWithRealParser = new ModelRepositoryService(NullLoggerFactory.Instance, realParser);

        try
        {
            // Act
            var result = await serviceWithRealParser.ValidateModelsAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Assert - Invalid DTDL correctly fails validation
            result.Should().BeFalse();
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task ValidateModels_NonExistentDirectory_ReturnsFalse()
    {
        // Arrange
        var path = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

        // Act
        var result = await sut.ValidateModelsAsync(path, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }
}