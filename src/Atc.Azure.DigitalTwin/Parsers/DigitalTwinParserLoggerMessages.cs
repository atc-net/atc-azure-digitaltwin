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
        Message = "Failed to parse models")]
    private partial void LogParseFailed(Exception exception);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinParser.ParseError,
        Level = LogLevel.Error,
        Message = "Parse error: {ParseError}")]
    private partial void LogParseError(string parseError);
}