namespace Atc.Azure.DigitalTwin.Parsers;

/// <summary>
/// DigitalTwinParser LoggerMessages.
/// </summary>
[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "OK - By Design")]
public sealed partial class DigitalTwinParser
{
    private readonly ILogger<DigitalTwinParser> logger;

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinParser.ParseFailed,
        Level = LogLevel.Error,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to parse models: {errorMessage}")]
    private partial void LogParseFailed(
        string errorMessage,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinParser.ParseError,
        Level = LogLevel.Error,
        Message = "{callerMethodName}({callerLineNumber}) - Parse-Error: {parseError}")]
    private partial void LogParseError(
        string parseError,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);
}
