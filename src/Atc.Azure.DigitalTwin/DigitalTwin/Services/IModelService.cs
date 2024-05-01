namespace Atc.Azure.DigitalTwin.DigitalTwin.Services;

public interface IModelService
{
    // TODO: Extend and document
    void AddModel(
        Dtmi key,
        DTInterfaceInfo value);

    IEnumerable<string> GetModelsContent();

    IDictionary<Dtmi, DTInterfaceInfo> GetModels();

    void Clear();

    Task<bool> LoadModelContentAsync(
        DirectoryInfo path);

    Task<bool> ValidateModels(
        DirectoryInfo path);
}