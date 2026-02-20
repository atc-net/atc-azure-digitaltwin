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
        var result = await parser.ParseAsync([ValidDtdlModel]);
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
        var result = await parser.ParseAsync([ValidDtdlModel]);
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
        var result1 = await parser.ParseAsync([ValidDtdlModel]);
        var result2 = await parser.ParseAsync([model2]);
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
        var result = await parser.ParseAsync([ValidDtdlModel]);
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

    [Fact]
    public void GetModelsContentInDependencyOrder_EmptyContent_ReturnsEmpty()
    {
        // Act
        var result = sut.GetModelsContentInDependencyOrder();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetModelsContentInDependencyOrder_SingleModel_ReturnsSameModel()
    {
        // Arrange
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");

        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "model.json"),
                ValidDtdlModel,
                TestContext.Current.CancellationToken);

            await sut.LoadModelContentAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Act
            var result = new List<string>(sut.GetModelsContentInDependencyOrder());

            // Assert
            result.Should().HaveCount(1);
            result[0].Should().Be(ValidDtdlModel);
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task GetModelsContentInDependencyOrder_IndependentModels_ReturnsAll()
    {
        // Arrange
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");

        const string modelA = """
            {
                "@id": "dtmi:com:example:ModelA;1",
                "@type": "Interface",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        const string modelB = """
            {
                "@id": "dtmi:com:example:ModelB;1",
                "@type": "Interface",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        try
        {
            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "modelA.json"),
                modelA,
                TestContext.Current.CancellationToken);

            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "modelB.json"),
                modelB,
                TestContext.Current.CancellationToken);

            await sut.LoadModelContentAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Act
            var result = new List<string>(sut.GetModelsContentInDependencyOrder());

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(modelA);
            result.Should().Contain(modelB);
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task GetModelsContentInDependencyOrder_DependentModels_ReturnsBaseFirst()
    {
        // Arrange
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");

        const string baseModel = """
            {
                "@id": "dtmi:com:example:Base;1",
                "@type": "Interface",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        const string derivedModel = """
            {
                "@id": "dtmi:com:example:Derived;1",
                "@type": "Interface",
                "extends": "dtmi:com:example:Base;1",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        try
        {
            // Write derived first to ensure ordering is not just file-order
            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "a_derived.json"),
                derivedModel,
                TestContext.Current.CancellationToken);

            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "b_base.json"),
                baseModel,
                TestContext.Current.CancellationToken);

            await sut.LoadModelContentAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Act
            var result = new List<string>(sut.GetModelsContentInDependencyOrder());

            // Assert
            result.Should().HaveCount(2);

            var baseIndex = result.FindIndex(m => m.Contains("dtmi:com:example:Base;1", StringComparison.Ordinal));
            var derivedIndex = result.FindIndex(m => m.Contains("dtmi:com:example:Derived;1", StringComparison.Ordinal));

            baseIndex.Should().BeLessThan(derivedIndex, "base model should appear before derived model");
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task GetModelsContentInDependencyOrder_ThreeLevelChain_ReturnsCorrectOrder()
    {
        // Arrange
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");

        const string equipment = """
            {
                "@id": "dtmi:com:example:Equipment;1", 
                "@type": "Interface", 
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        const string machinery = """
            {
                "@id": "dtmi:com:example:Machinery;1", 
                "@type": "Interface", 
                "extends": "dtmi:com:example:Equipment;1", 
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        const string pressMachine = """
            {
                "@id": "dtmi:com:example:PressMachine;1",
                "@type": "Interface",
                "extends": "dtmi:com:example:Machinery;1",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        try
        {
            // Write in reverse dependency order to ensure sorting is applied
            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "a_press.json"),
                pressMachine,
                TestContext.Current.CancellationToken);

            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "b_machinery.json"),
                machinery,
                TestContext.Current.CancellationToken);

            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "c_equipment.json"),
                equipment,
                TestContext.Current.CancellationToken);

            await sut.LoadModelContentAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Act
            var result = new List<string>(sut.GetModelsContentInDependencyOrder());

            // Assert
            result.Should().HaveCount(3);

            var equipmentIndex = result.FindIndex(m => m.Contains("dtmi:com:example:Equipment;1", StringComparison.Ordinal));
            var machineryIndex = result.FindIndex(m => m.Contains("dtmi:com:example:Machinery;1", StringComparison.Ordinal));
            var pressIndex = result.FindIndex(m => m.Contains("dtmi:com:example:PressMachine;1", StringComparison.Ordinal));

            equipmentIndex.Should().BeLessThan(machineryIndex, "Equipment should appear before Machinery");
            machineryIndex.Should().BeLessThan(pressIndex, "Machinery should appear before PressMachine");
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task GetModelsContentInDependencyOrder_ExtendsArray_HandlesCorrectly()
    {
        // Arrange
        var tempDir = Directory.CreateTempSubdirectory("dtdl-test-");

        const string modelA = """
            {
                "@id": "dtmi:com:example:A;1",
                "@type": "Interface",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        const string modelB = """
            {
                "@id": "dtmi:com:example:B;1",
                "@type": "Interface",
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        const string modelC = """
            {
                "@id": "dtmi:com:example:C;1",
                "@type": "Interface",
                "extends": ["dtmi:com:example:A;1", "dtmi:com:example:B;1"],
                "@context": "dtmi:dtdl:context;2"
            }
            """;

        try
        {
            // Write child first to ensure sorting is needed
            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "a_child.json"),
                modelC,
                TestContext.Current.CancellationToken);

            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "b_parentA.json"),
                modelA,
                TestContext.Current.CancellationToken);

            await File.WriteAllTextAsync(
                Path.Combine(tempDir.FullName, "c_parentB.json"),
                modelB,
                TestContext.Current.CancellationToken);

            await sut.LoadModelContentAsync(new DirectoryInfo(tempDir.FullName), TestContext.Current.CancellationToken);

            // Act
            var result = new List<string>(sut.GetModelsContentInDependencyOrder());

            // Assert
            result.Should().HaveCount(3);

            var indexA = result.FindIndex(m => m.Contains("dtmi:com:example:A;1", StringComparison.Ordinal));
            var indexB = result.FindIndex(m => m.Contains("dtmi:com:example:B;1", StringComparison.Ordinal));
            var indexC = result.FindIndex(m => m.Contains("dtmi:com:example:C;1", StringComparison.Ordinal));

            indexA.Should().BeLessThan(indexC, "parent A should appear before child C");
            indexB.Should().BeLessThan(indexC, "parent B should appear before child C");
        }
        finally
        {
            tempDir.Delete(recursive: true);
        }
    }
}