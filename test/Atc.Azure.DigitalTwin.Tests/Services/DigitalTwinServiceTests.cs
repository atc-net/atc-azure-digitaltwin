namespace Atc.Azure.DigitalTwin.Tests.Services;

public sealed class DigitalTwinServiceTests : IDisposable
{
    private readonly DigitalTwinsClient mockClient;
    private readonly DigitalTwinService sut;
    private readonly List<Response> responsesToDispose = [];

    public DigitalTwinServiceTests()
    {
        mockClient = Substitute.For<DigitalTwinsClient>();
        sut = new DigitalTwinService(NullLoggerFactory.Instance, mockClient);
    }

    public void Dispose()
    {
        foreach (var response in responsesToDispose)
        {
            response.Dispose();
        }
    }

    private static AsyncPageable<T> CreateAsyncPageable<T>(params T[] items)
        where T : notnull
    {
        var page = Page<T>.FromValues(
            items,
            continuationToken: null,
            Substitute.For<Response>());

        return AsyncPageable<T>.FromPages([page]);
    }

    private Response TrackResponse(Response response)
    {
        responsesToDispose.Add(response);
        return response;
    }

    private Response CreateErrorResponse()
    {
        var response = Substitute.For<Response>();
        response.IsError.Returns(true);
        return TrackResponse(response);
    }

    private Response CreateSuccessResponse()
    {
        var response = Substitute.For<Response>();
        response.IsError.Returns(false);
        return TrackResponse(response);
    }

    [Fact]
    public async Task GetModelAsync_Success_ReturnsModelData()
    {
        // Arrange
        var modelData = DigitalTwinsModelFactory.DigitalTwinsModelData(
            id: "dtmi:test:Model;1");

        var mockResponse = TrackResponse(Substitute.For<Response>());
        mockClient
            .GetModelAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(modelData, mockResponse)));

        // Act
        var result = await sut.GetModelAsync("dtmi:test:Model;1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("dtmi:test:Model;1");
    }

    [Fact]
    public async Task GetModelAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetModelAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "NotFound", innerException: null));

        // Act
        var result = await sut.GetModelAsync("dtmi:test:Model;1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetModelAsync_GeneralException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetModelAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Something went wrong"));

        // Act
        var result = await sut.GetModelAsync("dtmi:test:Model;1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetModelsAsync_Success_ReturnsList()
    {
        // Arrange
        var modelData = DigitalTwinsModelFactory.DigitalTwinsModelData(
            id: "dtmi:test:Model;1");

        var pageable = CreateAsyncPageable(modelData);

        mockClient
            .GetModelsAsync(Arg.Any<GetModelsOptions?>(), Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetModelsAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetModelsAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetModelsAsync(Arg.Any<GetModelsOptions?>(), Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Internal error", "InternalError", innerException: null));

        // Act
        var result = await sut.GetModelsAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateModelsAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var modelDataArray = Array.Empty<DigitalTwinsModelData>();
        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockClient
            .CreateModelsAsync(Arg.Any<IEnumerable<string>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(modelDataArray, mockResponse)));

        // Act
        var (succeeded, errorMessage) = await sut.CreateModelsAsync(
            ["""{ "@id": "dtmi:test:Model;1" }"""],
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task CreateModelsAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .CreateModelsAsync(Arg.Any<IEnumerable<string>>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(409, "Model already exists", "Conflict", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.CreateModelsAsync(
            ["""{ "@id": "dtmi:test:Model;1" }"""],
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DecommissionModelAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();

        mockClient
            .DecommissionModelAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DecommissionModelAsync(
            "dtmi:test:Model;1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task DecommissionModelAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();

        mockClient
            .DecommissionModelAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DecommissionModelAsync(
            "dtmi:test:Model;1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DecommissionModelAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .DecommissionModelAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "NotFound", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.DecommissionModelAsync(
            "dtmi:test:Model;1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteModelAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();

        mockClient
            .DeleteModelAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteModelAsync(
            "dtmi:test:Model;1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task DeleteModelAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .DeleteModelAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "NotFound", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteModelAsync(
            "dtmi:test:Model;1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteModelAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();

        mockClient
            .DeleteModelAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteModelAsync(
            "dtmi:test:Model;1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTwinAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetDigitalTwinAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Twin not found", "NotFound", innerException: null));

        // Act
        var result = await sut.GetTwinAsync("twin-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTwinAsync_Success_ReturnsTwin()
    {
        // Arrange
        var twin = new BasicDigitalTwin
        {
            Id = "twin-1",
            Metadata = new DigitalTwinMetadata { ModelId = "dtmi:test:Model;1" },
        };

        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockClient
            .GetDigitalTwinAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(twin, mockResponse)));

        // Act
        var result = await sut.GetTwinAsync("twin-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("twin-1");
    }

    [Fact]
    public async Task GetTwinIdsAsync_Success_ReturnsTwinIds()
    {
        // Arrange
        var twin1 = new BasicDigitalTwin
        {
            Id = "twin-1",
            Metadata = new DigitalTwinMetadata { ModelId = "dtmi:test:Model;1" },
        };

        var twin2 = new BasicDigitalTwin
        {
            Id = "twin-2",
            Metadata = new DigitalTwinMetadata { ModelId = "dtmi:test:Model;1" },
        };

        var pageable = CreateAsyncPageable(twin1, twin2);

        mockClient
            .QueryAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetTwinIdsAsync("SELECT * FROM digitaltwins", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain("twin-1");
        result.Should().Contain("twin-2");
    }

    [Fact]
    public async Task GetTwinIdsAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .QueryAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Query failed", "InternalError", innerException: null));

        // Act
        var result = await sut.GetTwinIdsAsync("SELECT * FROM digitaltwins", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTwinsAsync_Success_ReturnsTwins()
    {
        // Arrange
        var twin1 = new BasicDigitalTwin
        {
            Id = "twin-1",
            Metadata = new DigitalTwinMetadata { ModelId = "dtmi:test:Model;1" },
        };

        var pageable = CreateAsyncPageable(twin1);

        mockClient
            .QueryAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetTwinsAsync("SELECT * FROM digitaltwins", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result![0].Id.Should().Be("twin-1");
    }

    [Fact]
    public async Task GetTwinsAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .QueryAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Query failed", "InternalError", innerException: null));

        // Act
        var result = await sut.GetTwinsAsync("SELECT * FROM digitaltwins", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateOrReplaceDigitalTwinAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var twin = new BasicDigitalTwin
        {
            Id = "twin-1",
            Metadata = new DigitalTwinMetadata { ModelId = "dtmi:test:Model;1" },
        };

        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockClient
            .CreateOrReplaceDigitalTwinAsync(
                Arg.Any<string>(),
                Arg.Any<BasicDigitalTwin>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(twin, mockResponse)));

        // Act
        var (succeeded, errorMessage) = await sut.CreateOrReplaceDigitalTwinAsync(
            "twin-1",
            twin,
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task CreateOrReplaceDigitalTwinAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        var twin = new BasicDigitalTwin
        {
            Id = "twin-1",
            Metadata = new DigitalTwinMetadata { ModelId = "dtmi:test:Model;1" },
        };

        mockClient
            .CreateOrReplaceDigitalTwinAsync(
                Arg.Any<string>(),
                Arg.Any<BasicDigitalTwin>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(409, "Twin already exists", "Conflict", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.CreateOrReplaceDigitalTwinAsync(
            "twin-1",
            twin,
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteTwinAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();

        mockClient
            .DeleteDigitalTwinAsync(Arg.Any<string>(), Arg.Any<ETag?>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteTwinAsync(
            "twin-1",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task DeleteTwinAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();

        mockClient
            .DeleteDigitalTwinAsync(Arg.Any<string>(), Arg.Any<ETag?>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteTwinAsync(
            "twin-1",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteTwinAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .DeleteDigitalTwinAsync(Arg.Any<string>(), Arg.Any<ETag?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Twin not found", "NotFound", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteTwinAsync(
            "twin-1",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateTwinAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();

        mockClient
            .UpdateDigitalTwinAsync(
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        var patch = new JsonPatchDocument();
        patch.AppendReplace("/property", "value");

        // Act
        var (succeeded, errorMessage) = await sut.UpdateTwinAsync(
            "twin-1",
            patch,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task UpdateTwinAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .UpdateDigitalTwinAsync(
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Twin not found", "NotFound", innerException: null));

        var patch = new JsonPatchDocument();
        patch.AppendReplace("/property", "value");

        // Act
        var (succeeded, errorMessage) = await sut.UpdateTwinAsync(
            "twin-1",
            patch,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateTwinAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();

        mockClient
            .UpdateDigitalTwinAsync(
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        var patch = new JsonPatchDocument();
        patch.AppendReplace("/property", "value");

        // Act
        var (succeeded, errorMessage) = await sut.UpdateTwinAsync(
            "twin-1",
            patch,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRelationshipsAsync_Success_ReturnsRelationships()
    {
        // Arrange
        var relationship = new BasicRelationship
        {
            Id = "rel-1",
            SourceId = "twin-1",
            TargetId = "twin-2",
            Name = "contains",
        };

        var pageable = CreateAsyncPageable(relationship);

        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetRelationshipsAsync("twin-1", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result![0].Id.Should().Be("rel-1");
    }

    [Fact]
    public async Task GetRelationshipsAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Error", "InternalError", innerException: null));

        // Act
        var result = await sut.GetRelationshipsAsync("twin-1", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetIncomingRelationshipsAsync_Success_ReturnsList()
    {
        // Arrange
        var incomingRelationship = DigitalTwinsModelFactory.IncomingRelationship(
            "rel-1",
            "source-twin",
            "contains",
            "/properties/isActive");
        var pageable = CreateAsyncPageable(incomingRelationship);

        mockClient
            .GetIncomingRelationshipsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetIncomingRelationshipsAsync("twin-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetIncomingRelationshipsAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetIncomingRelationshipsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Error", "InternalError", innerException: null));

        // Act
        var result = await sut.GetIncomingRelationshipsAsync("twin-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateRelationshipAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var relationship = new BasicRelationship
        {
            Id = "twin-1-contains->twin-2",
            SourceId = "twin-1",
            TargetId = "twin-2",
            Name = "contains",
        };

        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockClient
            .CreateOrReplaceRelationshipAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<BasicRelationship>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(relationship, mockResponse)));

        // Act
        var (succeeded, errorMessage) = await sut.CreateRelationshipAsync(
            "twin-1",
            "twin-2",
            "contains",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task CreateRelationshipAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .CreateOrReplaceRelationshipAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<BasicRelationship>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(409, "Conflict", "Conflict", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.CreateRelationshipAsync(
            "twin-1",
            "twin-2",
            "contains",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteRelationshipsAsync_WithEmptyRelationships_CompletesSuccessfully()
    {
        // Arrange
        var emptyRelationships = CreateAsyncPageable<BasicRelationship>();
        var emptyIncoming = CreateAsyncPageable<IncomingRelationship>();

        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Returns(emptyRelationships);

        mockClient
            .GetIncomingRelationshipsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(emptyIncoming);

        // Act
        await sut.DeleteRelationshipsAsync("twin-1", TestContext.Current.CancellationToken);

        // Assert
        await mockClient.DidNotReceive().DeleteRelationshipAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<ETag?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateRelationshipAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();

        mockClient
            .UpdateRelationshipAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        var patch = new JsonPatchDocument();
        patch.AppendReplace("/isActive", true);

        // Act
        var (succeeded, errorMessage) = await sut.UpdateRelationshipAsync(
            "twin-1",
            "rel-1",
            patch,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task UpdateRelationshipAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .UpdateRelationshipAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "NotFound", innerException: null));

        var patch = new JsonPatchDocument();
        patch.AppendReplace("/isActive", true);

        // Act
        var (succeeded, errorMessage) = await sut.UpdateRelationshipAsync(
            "twin-1",
            "rel-1",
            patch,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateRelationshipAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();

        mockClient
            .UpdateRelationshipAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        var patch = new JsonPatchDocument();
        patch.AppendReplace("/isActive", true);

        // Act
        var (succeeded, errorMessage) = await sut.UpdateRelationshipAsync(
            "twin-1",
            "rel-1",
            patch,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task QueryAsync_Success_ReturnsList()
    {
        // Arrange
        var twin = new BasicDigitalTwin
        {
            Id = "twin-1",
            Metadata = new DigitalTwinMetadata { ModelId = "dtmi:test:Model;1" },
        };

        var pageable = CreateAsyncPageable(twin);

        mockClient
            .QueryAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.QueryAsync<BasicDigitalTwin>(
            "SELECT * FROM digitaltwins",
            TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result![0].Id.Should().Be("twin-1");
    }

    [Fact]
    public async Task QueryAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .QueryAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Query failed", "InternalError", innerException: null));

        // Act
        var result = await sut.QueryAsync<BasicDigitalTwin>(
            "SELECT * FROM digitaltwins",
            TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task QueryAsync_Paged_Success_ReturnsPage()
    {
        // Arrange
        var twin = new BasicDigitalTwin
        {
            Id = "twin-1",
            Metadata = new DigitalTwinMetadata { ModelId = "dtmi:test:Model;1" },
        };

        var pageable = CreateAsyncPageable(twin);

        mockClient
            .QueryAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.QueryAsync<BasicDigitalTwin>(
            "SELECT * FROM digitaltwins",
            10,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Values.Should().HaveCount(1);
        result.Values[0].Id.Should().Be("twin-1");
    }

    [Fact]
    public async Task QueryAsync_Paged_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .QueryAsync<BasicDigitalTwin>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Query failed", "InternalError", innerException: null));

        // Act
        var result = await sut.QueryAsync<BasicDigitalTwin>(
            "SELECT * FROM digitaltwins",
            10,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateOrReplaceEventRouteAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();
        mockClient
            .CreateOrReplaceEventRouteAsync(
                Arg.Any<string>(),
                Arg.Any<DigitalTwinsEventRoute>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.CreateOrReplaceEventRouteAsync(
            "route-1",
            "endpoint-1",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task CreateOrReplaceEventRouteAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .CreateOrReplaceEventRouteAsync(
                Arg.Any<string>(),
                Arg.Any<DigitalTwinsEventRoute>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(400, "Bad request", "BadRequest", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.CreateOrReplaceEventRouteAsync(
            "route-1",
            "endpoint-1",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateOrReplaceEventRouteAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();
        mockClient
            .CreateOrReplaceEventRouteAsync(
                Arg.Any<string>(),
                Arg.Any<DigitalTwinsEventRoute>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        // Act
        var (succeeded, errorMessage) = await sut.CreateOrReplaceEventRouteAsync(
            "route-1",
            "endpoint-1",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteEventRouteAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();
        mockClient
            .DeleteEventRouteAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteEventRouteAsync(
            "route-1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task DeleteEventRouteAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .DeleteEventRouteAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "NotFound", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteEventRouteAsync(
            "route-1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteEventRouteAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();
        mockClient
            .DeleteEventRouteAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteEventRouteAsync(
            "route-1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetEventRouteAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetEventRouteAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "NotFound", innerException: null));

        // Act
        var result = await sut.GetEventRouteAsync("route-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetEventRouteAsync_Success_ReturnsEventRoute()
    {
        // Arrange
        var eventRoute = new DigitalTwinsEventRoute("endpoint-1", "true");
        var mockResponse = TrackResponse(Substitute.For<Response>());
        mockClient
            .GetEventRouteAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(eventRoute, mockResponse)));

        // Act
        var result = await sut.GetEventRouteAsync("route-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.EndpointName.Should().Be("endpoint-1");
    }

    [Fact]
    public async Task GetEventRoutesAsync_Success_ReturnsRoutes()
    {
        // Arrange
        var eventRoute = new DigitalTwinsEventRoute("endpoint-1", "true");
        var pageable = CreateAsyncPageable(eventRoute);
        mockClient
            .GetEventRoutesAsync(Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetEventRoutesAsync(TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result![0].EndpointName.Should().Be("endpoint-1");
    }

    [Fact]
    public async Task GetEventRoutesAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetEventRoutesAsync(Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Error", "InternalError", innerException: null));

        // Act
        var result = await sut.GetEventRoutesAsync(TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task PublishTelemetryAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();
        mockClient
            .PublishTelemetryAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<DateTimeOffset?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.PublishTelemetryAsync(
            "twin-1",
            """{ "temperature": 25 }""",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task PublishTelemetryAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .PublishTelemetryAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<DateTimeOffset?>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Twin not found", "NotFound", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.PublishTelemetryAsync(
            "twin-1",
            """{ "temperature": 25 }""",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task PublishTelemetryAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();
        mockClient
            .PublishTelemetryAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<DateTimeOffset?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        // Act
        var (succeeded, errorMessage) = await sut.PublishTelemetryAsync(
            "twin-1",
            """{ "temperature": 25 }""",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task PublishComponentTelemetryAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();
        mockClient
            .PublishComponentTelemetryAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<DateTimeOffset?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.PublishComponentTelemetryAsync(
            "twin-1",
            "thermostat",
            """{ "temperature": 25 }""",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task PublishComponentTelemetryAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .PublishComponentTelemetryAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<DateTimeOffset?>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Twin not found", "NotFound", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.PublishComponentTelemetryAsync(
            "twin-1",
            "thermostat",
            """{ "temperature": 25 }""",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task PublishComponentTelemetryAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();
        mockClient
            .PublishComponentTelemetryAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<DateTimeOffset?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        // Act
        var (succeeded, errorMessage) = await sut.PublishComponentTelemetryAsync(
            "twin-1",
            "thermostat",
            """{ "temperature": 25 }""",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRelationshipAsync_Success_ReturnsRelationship()
    {
        // Arrange
        var relationship = new BasicRelationship
        {
            Id = "rel-1",
            SourceId = "twin-1",
            TargetId = "twin-2",
            Name = "contains",
        };
        var pageable = CreateAsyncPageable(relationship);
        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetRelationshipAsync("twin-1", "contains", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("rel-1");
    }

    [Fact]
    public async Task GetRelationshipAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Error", "InternalError", innerException: null));

        // Act
        var result = await sut.GetRelationshipAsync("twin-1", "contains", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetRelationshipAsync_MultipleRelationshipsOfSameType_ReturnsFirst()
    {
        // Arrange
        var relationship1 = new BasicRelationship
        {
            Id = "rel-1",
            SourceId = "twin-1",
            TargetId = "twin-2",
            Name = "contains",
        };

        var relationship2 = new BasicRelationship
        {
            Id = "rel-2",
            SourceId = "twin-1",
            TargetId = "twin-3",
            Name = "contains",
        };

        var pageable = CreateAsyncPageable(relationship1, relationship2);

        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetRelationshipAsync("twin-1", "contains", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("rel-1");
    }

    [Fact]
    public async Task GetRelationshipAsync_NoMatchingRelationship_ReturnsNull()
    {
        // Arrange
        var pageable = CreateAsyncPageable<BasicRelationship>();

        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetRelationshipAsync("twin-1", "contains", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteRelationshipAsync_RelationshipNotFound_ReturnsFailed()
    {
        // Arrange
        var pageable = CreateAsyncPageable<BasicRelationship>();

        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var (succeeded, errorMessage) = await sut.DeleteRelationshipAsync(
            "twin-1",
            "contains",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteRelationshipAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var relationship = new BasicRelationship
        {
            Id = "rel-1",
            SourceId = "twin-1",
            TargetId = "twin-2",
            Name = "contains",
            ETag = new ETag("etag-1"),
        };

        var pageable = CreateAsyncPageable(relationship);

        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Returns(pageable);

        var successResponse = CreateSuccessResponse();
        mockClient
            .DeleteRelationshipAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteRelationshipAsync(
            "twin-1",
            "contains",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task DeleteRelationshipAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .GetRelationshipsAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string?>(),
                Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Error", "InternalError", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteRelationshipAsync(
            "twin-1",
            "contains",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateOrUpdateRelationshipAsync_NewRelationship_ReturnsSucceeded()
    {
        // Arrange
        mockClient
            .GetRelationshipAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "NotFound", innerException: null));

        var relationship = new BasicRelationship
        {
            Id = "twin-1-contains->twin-2",
            SourceId = "twin-1",
            TargetId = "twin-2",
            Name = "contains",
        };

        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockClient
            .CreateOrReplaceRelationshipAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<BasicRelationship>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(relationship, mockResponse)));

        // Act
        var (succeeded, errorMessage) = await sut.CreateOrUpdateRelationshipAsync(
            "twin-1",
            "twin-2",
            "contains",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task CreateOrUpdateRelationshipAsync_ExistingRelationship_UpdatesSuccessfully()
    {
        // Arrange - FindRelationshipByIdAsync returns an existing relationship
        var existingRelationship = new BasicRelationship
        {
            Id = "twin-1-contains->twin-2",
            SourceId = "twin-1",
            TargetId = "twin-2",
            Name = "contains",
            ETag = new ETag("etag-1"),
        };

        var mockResponse = TrackResponse(Substitute.For<Response>());
        mockClient
            .GetRelationshipAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(existingRelationship, mockResponse)));

        // UpdateRelationshipAsync returns success
        var successResponse = CreateSuccessResponse();
        mockClient
            .UpdateRelationshipAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.CreateOrUpdateRelationshipAsync(
            "twin-1",
            "twin-2",
            "contains",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task CreateOrUpdateRelationshipAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .GetRelationshipAsync<BasicRelationship>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(500, "Internal error", "InternalError", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.CreateOrUpdateRelationshipAsync(
            "twin-1",
            "twin-2",
            "contains",
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task ImportGraphAsync_Success_ReturnsImportJob()
    {
        // Arrange
        var inputUri = new Uri("https://storage.blob.core.windows.net/container/input.ndjson");
        var outputUri = new Uri("https://storage.blob.core.windows.net/container/output.ndjson");
        var importJob = DigitalTwinsModelFactory.ImportJob(
            "job-1",
            inputUri,
            outputUri);

        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockClient
            .ImportGraphAsync(
                Arg.Any<string>(),
                Arg.Any<ImportJob>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(importJob, mockResponse)));

        // Act
        var result = await sut.ImportGraphAsync(
            "job-1",
            inputUri,
            outputUri,
            TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.InputBlobUri.Should().Be(inputUri);
    }

    [Fact]
    public async Task ImportGraphAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .ImportGraphAsync(
                Arg.Any<string>(),
                Arg.Any<ImportJob>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(400, "Bad request", "ValidationFailed", innerException: null));

        // Act
        var result = await sut.ImportGraphAsync(
            "job-1",
            new Uri("https://storage.blob.core.windows.net/c/in.ndjson"),
            new Uri("https://storage.blob.core.windows.net/c/out.ndjson"),
            TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetImportJobAsync_Success_ReturnsImportJob()
    {
        // Arrange
        var importJob = DigitalTwinsModelFactory.ImportJob(
            "job-1",
            new Uri("https://storage.blob.core.windows.net/c/in.ndjson"),
            new Uri("https://storage.blob.core.windows.net/c/out.ndjson"));

        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockClient
            .GetImportJobAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(importJob, mockResponse)));

        // Act
        var result = await sut.GetImportJobAsync("job-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetImportJobAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetImportJobAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "ImportJobNotFound", innerException: null));

        // Act
        var result = await sut.GetImportJobAsync("job-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetImportJobsAsync_Success_ReturnsList()
    {
        // Arrange
        var importJob = DigitalTwinsModelFactory.ImportJob(
            "job-1",
            new Uri("https://storage.blob.core.windows.net/c/in.ndjson"),
            new Uri("https://storage.blob.core.windows.net/c/out.ndjson"));

        var pageable = CreateAsyncPageable(importJob);

        mockClient
            .GetImportJobsAsync(Arg.Any<CancellationToken>())
            .Returns(pageable);

        // Act
        var result = await sut.GetImportJobsAsync(TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetImportJobsAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .GetImportJobsAsync(Arg.Any<CancellationToken>())
            .Throws(new RequestFailedException(500, "Error", "InternalError", innerException: null));

        // Act
        var result = await sut.GetImportJobsAsync(TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteImportJobAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var successResponse = CreateSuccessResponse();

        mockClient
            .DeleteImportJobAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(successResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteImportJobAsync(
            "job-1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task DeleteImportJobAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();

        mockClient
            .DeleteImportJobAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteImportJobAsync(
            "job-1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteImportJobAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .DeleteImportJobAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(400, "Bad request", "ValidationFailed", innerException: null));

        // Act
        var (succeeded, errorMessage) = await sut.DeleteImportJobAsync(
            "job-1",
            TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task CancelImportJobAsync_Success_ReturnsImportJob()
    {
        // Arrange
        var importJob = DigitalTwinsModelFactory.ImportJob(
            "job-1",
            new Uri("https://storage.blob.core.windows.net/c/in.ndjson"),
            new Uri("https://storage.blob.core.windows.net/c/out.ndjson"),
            ImportJobStatus.Cancelled);

        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockClient
            .CancelImportJobAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(importJob, mockResponse)));

        // Act
        var result = await sut.CancelImportJobAsync("job-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CancelImportJobAsync_RequestFailedException_ReturnsNull()
    {
        // Arrange
        mockClient
            .CancelImportJobAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(400, "Bad request", "ValidationFailed", innerException: null));

        // Act
        var result = await sut.CancelImportJobAsync("job-1", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetComponentAsync_Success_ReturnsComponent()
    {
        // Arrange
        const string componentJson = """{ "temperature": 25.0 }""";
        var component = JsonDocument.Parse(componentJson).RootElement;
        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockClient
            .GetComponentAsync<JsonElement>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Response.FromValue(component, mockResponse)));

        // Act
        var result = await sut.GetComponentAsync<JsonElement>(
            "twin-1",
            "thermostat",
            TestContext.Current.CancellationToken);

        // Assert
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Fact]
    public async Task GetComponentAsync_RequestFailedException_ReturnsDefault()
    {
        // Arrange
        mockClient
            .GetComponentAsync<JsonElement>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "NotFound", innerException: null));

        // Act
        var result = await sut.GetComponentAsync<JsonElement>(
            "twin-1",
            "thermostat",
            TestContext.Current.CancellationToken);

        // Assert
        result.ValueKind.Should().Be(JsonValueKind.Undefined);
    }

    [Fact]
    public async Task UpdateComponentAsync_Success_ReturnsSucceeded()
    {
        // Arrange
        var mockResponse = TrackResponse(Substitute.For<Response>());

        mockResponse.IsError.Returns(false);

        mockClient
            .UpdateComponentAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(mockResponse));

        var patchDocument = new JsonPatchDocument();
        patchDocument.AppendReplace("/temperature", 30.0);

        // Act
        var (succeeded, errorMessage) = await sut.UpdateComponentAsync(
            "twin-1",
            "thermostat",
            patchDocument,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeTrue();
        errorMessage.Should().BeNull();
    }

    [Fact]
    public async Task UpdateComponentAsync_RequestFailedException_ReturnsFailed()
    {
        // Arrange
        mockClient
            .UpdateComponentAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new RequestFailedException(404, "Not found", "NotFound", innerException: null));

        var patchDocument = new JsonPatchDocument();
        patchDocument.AppendReplace("/temperature", 30.0);

        // Act
        var (succeeded, errorMessage) = await sut.UpdateComponentAsync(
            "twin-1",
            "thermostat",
            patchDocument,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateComponentAsync_ErrorResponse_ReturnsFailed()
    {
        // Arrange
        var errorResponse = CreateErrorResponse();

        mockClient
            .UpdateComponentAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<JsonPatchDocument>(),
                Arg.Any<ETag?>(),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(errorResponse));

        var patchDocument = new JsonPatchDocument();
        patchDocument.AppendReplace("/temperature", 30.0);

        // Act
        var (succeeded, errorMessage) = await sut.UpdateComponentAsync(
            "twin-1",
            "thermostat",
            patchDocument,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        succeeded.Should().BeFalse();
        errorMessage.Should().NotBeNull();
    }
}