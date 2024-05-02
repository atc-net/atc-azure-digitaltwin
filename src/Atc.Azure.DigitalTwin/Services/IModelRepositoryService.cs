namespace Atc.Azure.DigitalTwin.Services;

public interface IModelRepositoryService
{
    // TODO: document
    void AddModel(
        Dtmi key,
        DTInterfaceInfo value);

    IEnumerable<string> GetModelsContent();

    IDictionary<Dtmi, DTInterfaceInfo> GetModels();

    void Clear();

    Task<bool> LoadModelContent(
        DirectoryInfo path);

    Task<bool> ValidateModels(
        DirectoryInfo path);
}