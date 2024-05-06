namespace Atc.Azure.DigitalTwin.Services;

/// <summary>
/// DigitalTwinService LoggerMessages.
/// </summary>
[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "OK - By Design")]
public sealed partial class DigitalTwinService
{
    private readonly ILogger<DigitalTwinService> logger;

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RequestFailed,
        Level = LogLevel.Error,
        Message = "{callerMethodName}({callerLineNumber}) - Request Failed {status}/{errorCode}: {errorMessage}")]
    private partial void LogRequestFailed(
        int status,
        string? errorCode,
        string? errorMessage,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Failure,
        Level = LogLevel.Error,
        Message = "{callerMethodName}({callerLineNumber}) - An unexpected exception of type '{exceptionType}' occurred: {errorMessage}")]
    private partial void LogFailure(
        string exceptionType,
        string? errorMessage,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Retrieving,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieving {type} with identifier '{identifier}'")]
    private partial void LogRetrieving(
        string type,
        string identifier,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.NotFound,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Could not retrieve {type} with identifier '{identifier}'")]
    private partial void LogNotFound(
        string type,
        string identifier,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Retrieved,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieved {type} with identifier '{identifier}'")]
    private partial void LogRetrieved(
        string type,
        string identifier,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Querying,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Querying for '{query}'")]
    private partial void LogQuerying(
        string query,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.QueryingFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Querying failed for '{query}'")]
    private partial void LogQueryingFailed(
        string query,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Queried,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Queried for '{query}'")]
    private partial void LogQueried(
        string query,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingModels,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieving models")]
    private partial void LogRetrievingModels(
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.ModelsNotFound,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Could not retrieve models")]
    private partial void LogModelsNotFound(
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedModels,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieved models")]
    private partial void LogRetrievedModels(
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingTwinIds,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieving twinIds for query '{query}'")]
    private partial void LogRetrievingTwinIds(
        string query,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedTwinIds,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieved twinIds for query '{query}'")]
    private partial void LogRetrievedTwinIds(
        string query,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingTwins,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieving twinIds for query '{query}'")]
    private partial void LogRetrievingTwins(
        string query,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedTwins,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieved twinIds for query '{query}'")]
    private partial void LogRetrievedTwins(
        string query,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingRelationship,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieving relationship with name '{relationshipName}' for twin '{twinId}'")]
    private partial void LogRetrievingRelationship(
        string twinId,
        string relationshipName,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RelationshipNotFound,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Could not retrieve relationship with name '{relationshipName}' for twin '{twinId}'")]
    private partial void LogRelationshipNotFound(
        string twinId,
        string relationshipName,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedRelationship,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieved relationship with name '{relationshipName}' for twin '{twinId}'")]
    private partial void LogRetrievedRelationship(
        string twinId,
        string relationshipName,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingRelationships,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieving relationships for twin '{twinId}'")]
    private partial void LogRetrievingRelationships(
        string twinId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RelationshipsNotFound,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Could not retrieve relationships for twin '{twinId}'")]
    private partial void LogRelationshipsNotFound(
        string twinId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedRelationships,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Retrieved relationships for twin '{twinId}'")]
    private partial void LogRetrievedRelationships(
        string twinId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatingOrReplacingTwin,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Creating twin '{twinId}' with payload: {jsonPayload}")]
    private partial void LogCreatingOrReplacingTwin(
        string twinId,
        [StringSyntax(StringSyntaxAttribute.Json)]
        string jsonPayload,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreateOrReplaceTwinFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to create/replace twin '{twinId}' with payload: {jsonPayload}")]
    private partial void LogCreateOrReplaceTwinFailed(
        string twinId,
        [StringSyntax(StringSyntaxAttribute.Json)]
        string jsonPayload,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatedOrReplacedTwin,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Created/Replaced twin '{twinId}' with payload: {jsonPayload}")]
    private partial void LogCreatedOrReplacedTwin(
        string twinId,
        [StringSyntax(StringSyntaxAttribute.Json)]
        string jsonPayload,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatingModels,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Creating models")]
    private partial void LogCreatingModels(
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreateModelsFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to create models")]
    private partial void LogCreateModelsFailed(
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatedModels,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Created models")]
    private partial void LogCreatedModels(
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatingOrUpdatingRelationship,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Creating/updating relationship with name '{relationshipName}' between twins '{sourceTwinId}' and '{targetTwinId}'")]
    private partial void LogCreatingOrUpdatingRelationship(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreateOrUpdatingRelationshipFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to create/update relationship with name '{relationshipName}' between twins '{sourceTwinId}' and '{targetTwinId}'")]
    private partial void LogCreateOrUpdateRelationshipFailed(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatedOrUpdatedRelationship,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Created/updated relationship with name '{relationshipName}' between twins '{sourceTwinId}' and '{targetTwinId}'")]
    private partial void LogCreatedOrUpdatedRelationship(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DecommissioningModel,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Decommissioning model '{modelId}'")]
    private partial void LogDecommissioningModel(
        string modelId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DecommissionModelFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to decommission model '{modelId}'")]
    private partial void LogDecommissionModelFailed(
        string modelId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DecommissionedModel,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Decommissioned model '{modelId}'")]
    private partial void LogDecommissionedModel(
        string modelId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletingModel,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Deleting model '{modelId}'")]
    private partial void LogDeletingModel(
        string modelId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeleteModelFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to delete model '{modelId}'")]
    private partial void LogDeleteModelFailed(
        string modelId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletedModel,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Deleted model '{modelId}'")]
    private partial void LogDeletedModel(
        string modelId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletingRelationship,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Deleting relationship '{relationshipName}' on twin '{twinId}'")]
    private partial void LogDeletingRelationship(
        string twinId,
        string relationshipName,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeleteRelationshipFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to delete relationship '{relationshipName}' on twin '{twinId}'")]
    private partial void LogDeleteRelationshipFailed(
        string twinId,
        string relationshipName,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletedRelationship,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Deleted relationship '{relationshipName}' on twin '{twinId}'")]
    private partial void LogDeletedRelationship(
        string twinId,
        string relationshipName,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletingTwin,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Deleting twin '{twinId}'")]
    private partial void LogDeletingTwin(
        string twinId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeleteTwinFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to delete twin '{twinId}'")]
    private partial void LogDeleteTwinFailed(
        string twinId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletedTwin,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Deleted twin '{twinId}'")]
    private partial void LogDeletedTwin(
        string twinId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdatingRelationship,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Updating relationship '{relationshipId}' on twin '{twinId}'")]
    private partial void LogUpdatingRelationship(
        string twinId,
        string relationshipId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdateRelationshipFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to update relationship '{relationshipId}' on twin '{twinId}'")]
    private partial void LogUpdateRelationshipFailed(
        string twinId,
        string relationshipId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdatedRelationship,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Updated relationship '{relationshipId}' on twin '{twinId}'")]
    private partial void LogUpdatedRelationship(
        string twinId,
        string relationshipId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdatingTwin,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Updating twin '{twinId}'")]
    private partial void LogUpdatingTwin(
        string twinId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdateTwinFailed,
        Level = LogLevel.Warning,
        Message = "{callerMethodName}({callerLineNumber}) - Failed to update twin '{twinId}'")]
    private partial void LogUpdateTwinFailed(
        string twinId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdatedTwin,
        Level = LogLevel.Trace,
        Message = "{callerMethodName}({callerLineNumber}) - Updated twin '{twinId}'")]
    private partial void LogUpdatedTwin(
        string twinId,
        [CallerMemberName] string callerMethodName = "",
        [CallerLineNumber] int callerLineNumber = 0);
}