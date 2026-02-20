namespace Atc.Azure.DigitalTwin.Services;

/// <summary>
/// Defines a service interface for managing and interacting with digital twins and their models.
/// </summary>
public interface IDigitalTwinService
{
    /// <summary>
    /// Retrieves the model data for a specified digital twin model ID.
    /// </summary>
    /// <param name="modelId">The ID of the model to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The model data if found; otherwise, null.</returns>
    Task<DigitalTwinsModelData?> GetModelAsync(
        string modelId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all models.
    /// </summary>
    /// <param name="options">The options to apply to the models' retrieval.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of digital twins model data; otherwise, null on failure.</returns>
    Task<List<DigitalTwinsModelData>?> GetModelsAsync(
        GetModelsOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the digital twin data for a specified twin ID.
    /// </summary>
    /// <param name="twinId">The ID of the twin to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The digital twin data if found; otherwise, null.</returns>
    Task<BasicDigitalTwin?> GetTwinAsync(
        string twinId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the digital twin data for a specified twin ID as a specific type.
    /// </summary>
    /// <param name="twinId">The ID of the twin to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="T">The type to which the twin data will be cast.</typeparam>
    /// <returns>The digital twin data if found; otherwise, null.</returns>
    Task<T?> GetTwinAsync<T>(
        string twinId,
        CancellationToken cancellationToken = default)
        where T : notnull;

    /// <summary>
    /// Retrieves a list of twin IDs based on a specified query.
    /// </summary>
    /// <param name="query">The query string to use for filtering twin IDs.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of twin IDs if found; otherwise, null.</returns>
    Task<List<string>?> GetTwinIdsAsync(
        string query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of digital twins based on a specified query.
    /// </summary>
    /// <param name="query">The query string to use for filtering digital twins.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of digital twins if found; otherwise, null.</returns>
    Task<List<BasicDigitalTwin>?> GetTwinsAsync(
        string query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all incoming relationships for a specified digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin to inspect for relationships.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of incoming relationships; otherwise, null on failure.</returns>
    Task<List<IncomingRelationship>?> GetIncomingRelationshipsAsync(
        string twinId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific relationship for a specified digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin.</param>
    /// <param name="relationshipName">The name of the relationship to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The relationship data if found; otherwise, null.</returns>
    Task<BasicRelationship?> GetRelationshipAsync(
        string twinId,
        string relationshipName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all relationships for a specified digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin to inspect for relationships.</param>
    /// <param name="relationshipName">The name of the relationship to filter by; can be null to retrieve all relationships.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of relationships; otherwise, null on failure.</returns>
    Task<List<BasicRelationship>?> GetRelationshipsAsync(
        string twinId,
        string? relationshipName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates digital twin based on the instance of the provided type.
    /// </summary>
    /// <typeparam name="T">The type of the digital twin object to create or replace.</typeparam>
    /// <param name="twinId">The ID of the twin to create.</param>
    /// <param name="twin">The twin to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> CreateOrReplaceDigitalTwinAsync<T>(
        string twinId,
        T twin,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates digital twin models based on the provided DTDL (Digital Twins Definition Language) models.
    /// </summary>
    /// <param name="dtdlModels">The DTDL models as a collection of strings.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> CreateModelsAsync(
        IEnumerable<string> dtdlModels,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a relationship between two digital twins.
    /// </summary>
    /// <param name="sourceTwinId">The ID of the source twin.</param>
    /// <param name="targetTwinId">The ID of the target twin.</param>
    /// <param name="relationshipName">The name of the relationship to create.</param>
    /// <param name="isActive">Indicates whether the relationship is active.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> CreateRelationshipAsync(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName,
        bool isActive = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates or updates a relationship between two digital twins.
    /// </summary>
    /// <param name="sourceTwinId">The ID of the source twin.</param>
    /// <param name="targetTwinId">The ID of the target twin.</param>
    /// <param name="relationshipName">The name of the relationship to create or update.</param>
    /// <param name="isActive">Indicates whether the relationship is active.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> CreateOrUpdateRelationshipAsync(
        string sourceTwinId,
        string targetTwinId,
        string relationshipName,
        bool isActive = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Decommissions a digital twin model by its ID.
    /// </summary>
    /// <param name="modelId">The ID of the model to decommission.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> DecommissionModelAsync(
        string modelId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a digital twin model by its ID.
    /// </summary>
    /// <param name="modelId">The ID of the model to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> DeleteModelAsync(
        string modelId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a specific relationship for a digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin whose relationship is to be deleted.</param>
    /// <param name="relationshipName">The name of the relationship to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> DeleteRelationshipAsync(
        string twinId,
        string relationshipName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all relationships associated with a specific digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin whose relationships are to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteRelationshipsAsync(
        string twinId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin to delete.</param>
    /// <param name="ifMatch">Optional ETag to use for optimistic concurrency control.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> DeleteTwinAsync(
        string twinId,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a specific relationship for a digital twin using a JSON patch document.
    /// </summary>
    /// <param name="twinId">The ID of the twin whose relationship is to be updated.</param>
    /// <param name="relationshipId">The ID of the relationship to update.</param>
    /// <param name="jsonPatchDocument">The JSON patch document containing updates to the relationship.</param>
    /// <param name="ifMatch">Optional ETag to use for optimistic concurrency control.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> UpdateRelationshipAsync(
        string twinId,
        string relationshipId,
        JsonPatchDocument jsonPatchDocument,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a digital twin using a JSON patch document.
    /// </summary>
    /// <param name="twinId">The ID of the twin to update.</param>
    /// <param name="jsonPatchDocument">The JSON patch document containing updates to apply to the twin.</param>
    /// <param name="ifMatch">Optional ETag to use for optimistic concurrency control.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> UpdateTwinAsync(
        string twinId,
        JsonPatchDocument jsonPatchDocument,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates or replaces an event route.
    /// </summary>
    /// <param name="eventRouteId">The ID of the event route.</param>
    /// <param name="endpointName">The name of the endpoint to route events to.</param>
    /// <param name="filter">An optional filter expression. Defaults to "true" (all events).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> CreateOrReplaceEventRouteAsync(
        string eventRouteId,
        string endpointName,
        string? filter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an event route.
    /// </summary>
    /// <param name="eventRouteId">The ID of the event route to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> DeleteEventRouteAsync(
        string eventRouteId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific event route.
    /// </summary>
    /// <param name="eventRouteId">The ID of the event route to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The event route if found; otherwise, null.</returns>
    Task<DigitalTwinsEventRoute?> GetEventRouteAsync(
        string eventRouteId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all event routes.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of event routes; otherwise, null on failure.</returns>
    Task<List<DigitalTwinsEventRoute>?> GetEventRoutesAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Queries digital twins using a specified query string and returns all results of a specific type.
    /// </summary>
    /// <param name="query">The query string used to filter and retrieve digital twins.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="T">The type to which the results will be cast.</typeparam>
    /// <returns>A list of digital twins of the specified type; otherwise, null on failure.</returns>
    Task<List<T>?> QueryAsync<T>(
        string query,
        CancellationToken cancellationToken = default)
        where T : notnull;

    /// <summary>
    /// Queries digital twins using a specified query string, with pagination parameters, and returns a single page of results of a specific type.
    /// </summary>
    /// <param name="query">The query string used to filter and retrieve digital twins.</param>
    /// <param name="pageSize">The number of items to include in one page of results.</param>
    /// <param name="continuationToken">The continuation token to use for retrieving subsequent pages.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="T">The type to which the results will be cast.</typeparam>
    /// <returns>A page of digital twins of the specified type; otherwise, null if no results are found.</returns>
    Task<Page<T>?> QueryAsync<T>(
        string query,
        int pageSize,
        string? continuationToken = null,
        CancellationToken cancellationToken = default)
        where T : notnull;

    /// <summary>
    /// Submits a bulk import job to import models, twins, and relationships from a blob.
    /// </summary>
    /// <param name="jobId">A unique identifier for the import job.</param>
    /// <param name="inputBlobUri">The URI of the input blob containing NDJSON import data.</param>
    /// <param name="outputBlobUri">The URI of the output blob for import job logs.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created import job if successful; otherwise, null.</returns>
    Task<ImportJob?> ImportGraphAsync(
        string jobId,
        Uri inputBlobUri,
        Uri outputBlobUri,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an import job by its ID.
    /// </summary>
    /// <param name="jobId">The ID of the import job to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The import job if found; otherwise, null.</returns>
    Task<ImportJob?> GetImportJobAsync(
        string jobId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all import jobs.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of import jobs; otherwise, null on failure.</returns>
    Task<List<ImportJob>?> GetImportJobsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an import job by its ID.
    /// </summary>
    /// <param name="jobId">The ID of the import job to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> DeleteImportJobAsync(
        string jobId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a running import job.
    /// </summary>
    /// <param name="jobId">The ID of the import job to cancel.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The cancelled import job if successful; otherwise, null.</returns>
    Task<ImportJob?> CancelImportJobAsync(
        string jobId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific component of a digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin.</param>
    /// <param name="componentName">The name of the component to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="T">The type to which the component data will be cast.</typeparam>
    /// <returns>The component data if found; otherwise, null.</returns>
    Task<T?> GetComponentAsync<T>(
        string twinId,
        string componentName,
        CancellationToken cancellationToken = default)
        where T : notnull;

    /// <summary>
    /// Updates a specific component of a digital twin using a JSON patch document.
    /// </summary>
    /// <param name="twinId">The ID of the twin whose component is to be updated.</param>
    /// <param name="componentName">The name of the component to update.</param>
    /// <param name="jsonPatchDocument">The JSON patch document containing updates to the component.</param>
    /// <param name="ifMatch">Optional ETag to use for optimistic concurrency control.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> UpdateComponentAsync(
        string twinId,
        string componentName,
        JsonPatchDocument jsonPatchDocument,
        ETag? ifMatch = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes telemetry data for a digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin to publish telemetry for.</param>
    /// <param name="payload">The telemetry payload as a JSON string.</param>
    /// <param name="timestamp">Optional timestamp for the telemetry message.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> PublishTelemetryAsync(
        string twinId,
        string payload,
        DateTimeOffset? timestamp = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes telemetry data for a specific component of a digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin to publish telemetry for.</param>
    /// <param name="componentName">The name of the component.</param>
    /// <param name="payload">The telemetry payload as a JSON string.</param>
    /// <param name="timestamp">Optional timestamp for the telemetry message.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> PublishComponentTelemetryAsync(
        string twinId,
        string componentName,
        string payload,
        DateTimeOffset? timestamp = null,
        CancellationToken cancellationToken = default);
}