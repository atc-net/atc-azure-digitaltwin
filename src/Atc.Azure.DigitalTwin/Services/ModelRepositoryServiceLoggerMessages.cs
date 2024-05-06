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
        Message = "{callerMethodName}({callerLineNumber}) - Directory '{path}' does not exist")]
    private partial void LogUnknownDirectoryPath(
        string path,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ModelRepositoryService.ModelsLoaded,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Loaded models from directory '{path}'")]
    private partial void LogModelsLoaded(
        string path,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ModelRepositoryService.ParseFailed,
        Level = LogLevel.Error,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to parse models: {errorMessage}")]
    private partial void LogParseFailed(
        string errorMessage,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ModelRepositoryService.ParseError,
        Level = LogLevel.Error,
        Message = "{callerMethodName}({callerLineNumber}) - Parse-Error: {parseError}")]
    private partial void LogParseError(
        string parseError,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);
}
