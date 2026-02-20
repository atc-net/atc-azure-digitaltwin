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
        Message = "Request failed {Status}/{ErrorCode}")]
    private partial void LogRequestFailed(
        Exception exception,
        int status,
        string? errorCode);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Failure,
        Level = LogLevel.Error,
        Message = "An unexpected error occurred")]
    private partial void LogFailure(Exception exception);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Retrieving,
        Level = LogLevel.Trace,
        Message = "Retrieving {Type} with identifier '{Identifier}'")]
    private partial void LogRetrieving(
        string type,
        string identifier);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.NotFound,
        Level = LogLevel.Warning,
        Message = "Could not retrieve {Type} with identifier '{Identifier}'")]
    private partial void LogNotFound(
        string type,
        string identifier);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Retrieved,
        Level = LogLevel.Trace,
        Message = "Retrieved {Type} with identifier '{Identifier}'")]
    private partial void LogRetrieved(
        string type,
        string identifier);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Querying,
        Level = LogLevel.Trace,
        Message = "Querying for '{Query}'")]
    private partial void LogQuerying(string query);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.QueryingFailed,
        Level = LogLevel.Warning,
        Message = "Querying failed for '{Query}'")]
    private partial void LogQueryingFailed(string query);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.Queried,
        Level = LogLevel.Trace,
        Message = "Queried for '{Query}'")]
    private partial void LogQueried(string query);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingModels,
        Level = LogLevel.Trace,
        Message = "Retrieving models")]
    private partial void LogRetrievingModels();

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedModels,
        Level = LogLevel.Trace,
        Message = "Retrieved models")]
    private partial void LogRetrievedModels();

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingTwinIds,
        Level = LogLevel.Trace,
        Message = "Retrieving twin IDs for query '{Query}'")]
    private partial void LogRetrievingTwinIds(string query);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedTwinIds,
        Level = LogLevel.Trace,
        Message = "Retrieved twin IDs for query '{Query}'")]
    private partial void LogRetrievedTwinIds(string query);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingTwins,
        Level = LogLevel.Trace,
        Message = "Retrieving twins for query '{Query}'")]
    private partial void LogRetrievingTwins(string query);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedTwins,
        Level = LogLevel.Trace,
        Message = "Retrieved twins for query '{Query}'")]
    private partial void LogRetrievedTwins(string query);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingRelationship,
        Level = LogLevel.Trace,
        Message = "Retrieving relationship with name '{RelationshipName}' for twin '{TwinId}'")]
    private partial void LogRetrievingRelationship(
        string twinId,
        string relationshipName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RelationshipNotFound,
        Level = LogLevel.Warning,
        Message = "Could not retrieve relationship with name '{RelationshipName}' for twin '{TwinId}'")]
    private partial void LogRelationshipNotFound(
        string twinId,
        string relationshipName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedRelationship,
        Level = LogLevel.Trace,
        Message = "Retrieved relationship with name '{RelationshipName}' for twin '{TwinId}'")]
    private partial void LogRetrievedRelationship(
        string twinId,
        string relationshipName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievingRelationships,
        Level = LogLevel.Trace,
        Message = "Retrieving relationships for twin '{TwinId}'")]
    private partial void LogRetrievingRelationships(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RelationshipsNotFound,
        Level = LogLevel.Warning,
        Message = "Could not retrieve relationships for twin '{TwinId}'")]
    private partial void LogRelationshipsNotFound(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.RetrievedRelationships,
        Level = LogLevel.Trace,
        Message = "Retrieved relationships for twin '{TwinId}'")]
    private partial void LogRetrievedRelationships(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatingOrReplacingTwin,
        Level = LogLevel.Trace,
        Message = "Creating twin '{TwinId}' with payload: {JsonPayload}")]
    private partial void LogCreatingOrReplacingTwin(
        string twinId,
        [StringSyntax(StringSyntaxAttribute.Json)]
        string jsonPayload);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreateOrReplaceTwinFailed,
        Level = LogLevel.Warning,
        Message = "Failed to create/replace twin '{TwinId}' with payload: {JsonPayload}")]
    private partial void LogCreateOrReplaceTwinFailed(
        string twinId,
        [StringSyntax(StringSyntaxAttribute.Json)]
        string jsonPayload);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatedOrReplacedTwin,
        Level = LogLevel.Trace,
        Message = "Created/Replaced twin '{TwinId}' with payload: {JsonPayload}")]
    private partial void LogCreatedOrReplacedTwin(
        string twinId,
        [StringSyntax(StringSyntaxAttribute.Json)]
        string jsonPayload);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatingModels,
        Level = LogLevel.Trace,
        Message = "Creating models")]
    private partial void LogCreatingModels();

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreateModelsFailed,
        Level = LogLevel.Warning,
        Message = "Failed to create models")]
    private partial void LogCreateModelsFailed();

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatedModels,
        Level = LogLevel.Trace,
        Message = "Created models")]
    private partial void LogCreatedModels();

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatingOrUpdatingRelationship,
        Level = LogLevel.Trace,
        Message = "Creating/updating relationship with name '{RelationshipName}' between twins '{SourceTwinId}' and '{TargetTwinId}'")]
    private partial void LogCreatingOrUpdatingRelationship(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreateOrUpdatingRelationshipFailed,
        Level = LogLevel.Warning,
        Message = "Failed to create/update relationship with name '{RelationshipName}' between twins '{SourceTwinId}' and '{TargetTwinId}'")]
    private partial void LogCreateOrUpdateRelationshipFailed(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.CreatedOrUpdatedRelationship,
        Level = LogLevel.Trace,
        Message = "Created/updated relationship with name '{RelationshipName}' between twins '{SourceTwinId}' and '{TargetTwinId}'")]
    private partial void LogCreatedOrUpdatedRelationship(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DecommissioningModel,
        Level = LogLevel.Trace,
        Message = "Decommissioning model '{ModelId}'")]
    private partial void LogDecommissioningModel(string modelId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DecommissionModelFailed,
        Level = LogLevel.Warning,
        Message = "Failed to decommission model '{ModelId}'")]
    private partial void LogDecommissionModelFailed(string modelId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DecommissionedModel,
        Level = LogLevel.Trace,
        Message = "Decommissioned model '{ModelId}'")]
    private partial void LogDecommissionedModel(string modelId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletingModel,
        Level = LogLevel.Trace,
        Message = "Deleting model '{ModelId}'")]
    private partial void LogDeletingModel(string modelId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeleteModelFailed,
        Level = LogLevel.Warning,
        Message = "Failed to delete model '{ModelId}'")]
    private partial void LogDeleteModelFailed(string modelId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletedModel,
        Level = LogLevel.Trace,
        Message = "Deleted model '{ModelId}'")]
    private partial void LogDeletedModel(string modelId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletingRelationship,
        Level = LogLevel.Trace,
        Message = "Deleting relationship '{RelationshipName}' on twin '{TwinId}'")]
    private partial void LogDeletingRelationship(
        string twinId,
        string relationshipName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeleteRelationshipFailed,
        Level = LogLevel.Warning,
        Message = "Failed to delete relationship '{RelationshipName}' on twin '{TwinId}'")]
    private partial void LogDeleteRelationshipFailed(
        string twinId,
        string relationshipName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletedRelationship,
        Level = LogLevel.Trace,
        Message = "Deleted relationship '{RelationshipName}' on twin '{TwinId}'")]
    private partial void LogDeletedRelationship(
        string twinId,
        string relationshipName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletingTwin,
        Level = LogLevel.Trace,
        Message = "Deleting twin '{TwinId}'")]
    private partial void LogDeletingTwin(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeleteTwinFailed,
        Level = LogLevel.Warning,
        Message = "Failed to delete twin '{TwinId}'")]
    private partial void LogDeleteTwinFailed(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.DeletedTwin,
        Level = LogLevel.Trace,
        Message = "Deleted twin '{TwinId}'")]
    private partial void LogDeletedTwin(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdatingRelationship,
        Level = LogLevel.Trace,
        Message = "Updating relationship '{RelationshipId}' on twin '{TwinId}'")]
    private partial void LogUpdatingRelationship(
        string twinId,
        string relationshipId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdateRelationshipFailed,
        Level = LogLevel.Warning,
        Message = "Failed to update relationship '{RelationshipId}' on twin '{TwinId}'")]
    private partial void LogUpdateRelationshipFailed(
        string twinId,
        string relationshipId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdatedRelationship,
        Level = LogLevel.Trace,
        Message = "Updated relationship '{RelationshipId}' on twin '{TwinId}'")]
    private partial void LogUpdatedRelationship(
        string twinId,
        string relationshipId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdatingTwin,
        Level = LogLevel.Trace,
        Message = "Updating twin '{TwinId}'")]
    private partial void LogUpdatingTwin(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdateTwinFailed,
        Level = LogLevel.Warning,
        Message = "Failed to update twin '{TwinId}'")]
    private partial void LogUpdateTwinFailed(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.DigitalTwinService.UpdatedTwin,
        Level = LogLevel.Trace,
        Message = "Updated twin '{TwinId}'")]
    private partial void LogUpdatedTwin(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.ImportingGraph,
        Level = LogLevel.Trace,
        Message = "Importing graph for job '{JobId}'")]
    private partial void LogImportingGraph(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.ImportedGraph,
        Level = LogLevel.Trace,
        Message = "Imported graph for job '{JobId}'")]
    private partial void LogImportedGraph(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.ImportGraphFailed,
        Level = LogLevel.Warning,
        Message = "Failed to import graph for job '{JobId}'")]
    private partial void LogImportGraphFailed(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.RetrievingImportJob,
        Level = LogLevel.Trace,
        Message = "Retrieving import job '{JobId}'")]
    private partial void LogRetrievingImportJob(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.RetrievedImportJob,
        Level = LogLevel.Trace,
        Message = "Retrieved import job '{JobId}'")]
    private partial void LogRetrievedImportJob(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.RetrievingImportJobs,
        Level = LogLevel.Trace,
        Message = "Retrieving import jobs")]
    private partial void LogRetrievingImportJobs();

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.RetrievedImportJobs,
        Level = LogLevel.Trace,
        Message = "Retrieved import jobs")]
    private partial void LogRetrievedImportJobs();

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.DeletingImportJob,
        Level = LogLevel.Trace,
        Message = "Deleting import job '{JobId}'")]
    private partial void LogDeletingImportJob(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.DeletedImportJob,
        Level = LogLevel.Trace,
        Message = "Deleted import job '{JobId}'")]
    private partial void LogDeletedImportJob(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.DeleteImportJobFailed,
        Level = LogLevel.Warning,
        Message = "Failed to delete import job '{JobId}'")]
    private partial void LogDeleteImportJobFailed(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.CancellingImportJob,
        Level = LogLevel.Trace,
        Message = "Cancelling import job '{JobId}'")]
    private partial void LogCancellingImportJob(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.CancelledImportJob,
        Level = LogLevel.Trace,
        Message = "Cancelled import job '{JobId}'")]
    private partial void LogCancelledImportJob(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.ImportJobs.CancelImportJobFailed,
        Level = LogLevel.Warning,
        Message = "Failed to cancel import job '{JobId}'")]
    private partial void LogCancelImportJobFailed(string jobId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Component.RetrievingComponent,
        Level = LogLevel.Trace,
        Message = "Retrieving component '{ComponentName}' for twin '{TwinId}'")]
    private partial void LogRetrievingComponent(
        string twinId,
        string componentName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Component.RetrievedComponent,
        Level = LogLevel.Trace,
        Message = "Retrieved component '{ComponentName}' for twin '{TwinId}'")]
    private partial void LogRetrievedComponent(
        string twinId,
        string componentName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Component.ComponentNotFound,
        Level = LogLevel.Warning,
        Message = "Could not retrieve component '{ComponentName}' for twin '{TwinId}'")]
    private partial void LogComponentNotFound(
        string twinId,
        string componentName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Component.UpdatingComponent,
        Level = LogLevel.Trace,
        Message = "Updating component '{ComponentName}' for twin '{TwinId}'")]
    private partial void LogUpdatingComponent(
        string twinId,
        string componentName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Component.UpdatedComponent,
        Level = LogLevel.Trace,
        Message = "Updated component '{ComponentName}' for twin '{TwinId}'")]
    private partial void LogUpdatedComponent(
        string twinId,
        string componentName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Component.UpdateComponentFailed,
        Level = LogLevel.Warning,
        Message = "Failed to update component '{ComponentName}' for twin '{TwinId}'")]
    private partial void LogUpdateComponentFailed(
        string twinId,
        string componentName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Telemetry.PublishingTelemetry,
        Level = LogLevel.Trace,
        Message = "Publishing telemetry for twin '{TwinId}'")]
    private partial void LogPublishingTelemetry(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Telemetry.PublishedTelemetry,
        Level = LogLevel.Trace,
        Message = "Published telemetry for twin '{TwinId}'")]
    private partial void LogPublishedTelemetry(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Telemetry.PublishTelemetryFailed,
        Level = LogLevel.Warning,
        Message = "Failed to publish telemetry for twin '{TwinId}'")]
    private partial void LogPublishTelemetryFailed(string twinId);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Telemetry.PublishingComponentTelemetry,
        Level = LogLevel.Trace,
        Message = "Publishing component telemetry for twin '{TwinId}' component '{ComponentName}'")]
    private partial void LogPublishingComponentTelemetry(
        string twinId,
        string componentName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Telemetry.PublishedComponentTelemetry,
        Level = LogLevel.Trace,
        Message = "Published component telemetry for twin '{TwinId}' component '{ComponentName}'")]
    private partial void LogPublishedComponentTelemetry(
        string twinId,
        string componentName);

    [LoggerMessage(
        EventId = LoggingEventIdConstants.Telemetry.PublishComponentTelemetryFailed,
        Level = LogLevel.Warning,
        Message = "Failed to publish component telemetry for twin '{TwinId}' component '{ComponentName}'")]
    private partial void LogPublishComponentTelemetryFailed(
        string twinId,
        string componentName);
}