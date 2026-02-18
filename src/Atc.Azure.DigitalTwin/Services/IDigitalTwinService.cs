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
    Task<DigitalTwinsModelData?> GetModel(
        string modelId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a pageable list of all models.
    /// </summary>
    /// <param name="options">The options to apply to the models' retrieval.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An async pageable of digital twins model data; otherwise, null.</returns>
    AsyncPageable<DigitalTwinsModelData>? GetModels(
        GetModelsOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the digital twin data for a specified twin ID.
    /// </summary>
    /// <param name="twinId">The ID of the twin to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The digital twin data if found; otherwise, null.</returns>
    Task<BasicDigitalTwin?> GetTwin(
        string twinId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the digital twin data for a specified twin ID as a specific type.
    /// </summary>
    /// <param name="twinId">The ID of the twin to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="T">The type to which the twin data will be cast.</typeparam>
    /// <returns>The digital twin data if found; otherwise, null.</returns>
    Task<T?> GetTwin<T>(
        string twinId,
        CancellationToken cancellationToken = default)
        where T : notnull;

    /// <summary>
    /// Retrieves a list of twin IDs based on a specified query.
    /// </summary>
    /// <param name="query">The query string to use for filtering twin IDs.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of twin IDs if found; otherwise, null.</returns>
    Task<List<string>?> GetTwinIds(
        string query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of digital twins based on a specified query.
    /// </summary>
    /// <param name="query">The query string to use for filtering digital twins.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of digital twins if found; otherwise, null.</returns>
    Task<List<BasicDigitalTwin>?> GetTwins(
        string query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a pageable list of incoming relationships for a specified digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin to inspect for relationships.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An async pageable of incoming relationships; otherwise, null.</returns>
    AsyncPageable<IncomingRelationship>? GetIncomingRelationships(
        string twinId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific relationship for a specified digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin.</param>
    /// <param name="relationshipName">The name of the relationship to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The relationship data if found; otherwise, null.</returns>
    Task<BasicRelationship?> GetRelationship(
        string twinId,
        string relationshipName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a pageable list of relationships for a specified digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin to inspect for relationships.</param>
    /// <param name="relationshipName">The name of the relationship to filter by; can be null to retrieve all relationships.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An async pageable of relationships; otherwise, null.</returns>
    AsyncPageable<BasicRelationship>? GetRelationships(
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
    Task<(bool Succeeded, string? ErrorMessage)> CreateOrReplaceDigitalTwin<T>(
        string twinId,
        T twin,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates digital twin models based on the provided DTDL (Digital Twins Definition Language) models.
    /// </summary>
    /// <param name="dtdlModels">The DTDL models as a collection of strings.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> CreateModels(
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
    Task<(bool Succeeded, string? ErrorMessage)> CreateRelationship(
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
    Task<(bool Succeeded, string? ErrorMessage)> CreateOrUpdateRelationship(
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
    Task<(bool Succeeded, string? ErrorMessage)> DecommissionModel(
        string modelId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a digital twin model by its ID.
    /// </summary>
    /// <param name="modelId">The ID of the model to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> DeleteModel(
        string modelId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a specific relationship for a digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin whose relationship is to be deleted.</param>
    /// <param name="relationshipName">The name of the relationship to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> DeleteRelationship(
        string twinId,
        string relationshipName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all relationships associated with a specific digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin whose relationships are to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteRelationships(
        string twinId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a digital twin.
    /// </summary>
    /// <param name="twinId">The ID of the twin to delete.</param>
    /// <param name="ifMatch">Optional ETag to use for optimistic concurrency control.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing a boolean indicating success and an error message if applicable.</returns>
    Task<(bool Succeeded, string? ErrorMessage)> DeleteTwin(
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
    Task<(bool Succeeded, string? ErrorMessage)> UpdateRelationship(
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
    Task<(bool Succeeded, string? ErrorMessage)> UpdateTwin(
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
    Task<(bool Succeeded, string? ErrorMessage)> CreateOrReplaceEventRoute(
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
    Task<(bool Succeeded, string? ErrorMessage)> DeleteEventRoute(
        string eventRouteId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific event route.
    /// </summary>
    /// <param name="eventRouteId">The ID of the event route to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The event route if found; otherwise, null.</returns>
    Task<DigitalTwinsEventRoute?> GetEventRoute(
        string eventRouteId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all event routes.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An async pageable of event routes; otherwise, null.</returns>
    AsyncPageable<DigitalTwinsEventRoute>? GetEventRoutes(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Queries digital twins using a specified query string and returns a pageable set of results of a specific type.
    /// </summary>
    /// <param name="query">The query string used to filter and retrieve digital twins.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <typeparam name="T">The type to which the results will be cast.</typeparam>
    /// <returns>An async pageable of digital twins of the specified type; otherwise, null if no results are found.</returns>
    AsyncPageable<T>? Query<T>(
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
    Task<Page<T>?> Query<T>(
        string query,
        int pageSize,
        string? continuationToken = null,
        CancellationToken cancellationToken = default)
        where T : notnull;
}