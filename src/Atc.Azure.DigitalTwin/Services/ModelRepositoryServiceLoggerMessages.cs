namespace Atc.Azure.DigitalTwin.Services;

/// <summary>
/// ModelRepositoryService LoggerMessages.
/// </summary>
[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "OK - By Design")]
public sealed partial class ModelRepositoryService
{
    private readonly ILogger<ModelRepositoryService> logger;

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ModelRepositoryService.UnknownDirectoryPath,
        Level = LogLevel.Error,
        Message = "Directory '{Path}' does not exist")]
    private partial void LogUnknownDirectoryPath(string path);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ModelRepositoryService.ModelsLoaded,
        Level = LogLevel.Trace,
        Message = "Loaded models from directory '{Path}'")]
    private partial void LogModelsLoaded(string path);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ModelRepositoryService.ParseFailed,
        Level = LogLevel.Error,
        Message = "Failed to parse models: {ErrorMessage}")]
    private partial void LogParseFailed(string errorMessage);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ModelRepositoryService.ParseError,
        Level = LogLevel.Error,
        Message = "Parse error: {ParseError}")]
    private partial void LogParseError(string parseError);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ModelRepositoryService.DependencyOrderingFailed,
        Level = LogLevel.Warning,
        Message = "Failed to order models by dependency, falling back to original order")]
    private partial void LogDependencyOrderingFailed();
}