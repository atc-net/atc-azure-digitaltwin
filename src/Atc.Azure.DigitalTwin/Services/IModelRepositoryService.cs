namespace Atc.Azure.DigitalTwin.Services;

/// <summary>
/// Defines a service for managing digital twin models locally.
/// </summary>
public interface IModelRepositoryService
{
    /// <summary>
    /// Adds a digital twin model to the repository.
    /// </summary>
    /// <param name="key">The DTMI (Digital Twin Model Identifier) as the key for the model.</param>
    /// <param name="value">The digital twin interface information to store.</param>
    void AddModel(
        Dtmi key,
        DTInterfaceInfo value);

    /// <summary>
    /// Retrieves the content of all models as a collection of strings.
    /// </summary>
    /// <returns>A list of strings representing the content of each model.</returns>
    IEnumerable<string> GetModelsContent();

    /// <summary>
    /// Retrieves all models.
    /// </summary>
    /// <returns>A dictionary where each key is a DTMI and the value is the associated digital twin interface information.</returns>
    IDictionary<Dtmi, DTInterfaceInfo> GetModels();

    /// <summary>
    /// Clears all models.
    /// </summary>
    void Clear();

    /// <summary>
    /// Loads model content from a specified directory into the repository.
    /// </summary>
    /// <param name="path">The directory containing model files to load.</param>
    /// <returns><see langword="true" /> if the load is successful; otherwise, <see langword="false" />.</returns>
    Task<bool> LoadModelContent(
        DirectoryInfo path);

    /// <summary>
    /// Validates the models in a specified directory.
    /// </summary>
    /// <param name="path">The directory containing model files to validate.</param>
    /// <returns><see langword="true" /> if the load is successful; otherwise, <see langword="false" />.</returns>
    Task<bool> ValidateModels(
        DirectoryInfo path);
}