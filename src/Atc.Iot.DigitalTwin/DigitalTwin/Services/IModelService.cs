namespace Atc.Iot.DigitalTwin.DigitalTwin.Services;

public interface IModelService
{
    void AddModel(Dtmi key, DTInterfaceInfo value);

    IEnumerable<string> GetModelsContent();

    IDictionary<Dtmi, DTInterfaceInfo> GetModels();

    void Clear();

    Task<bool> LoadModelContentAsync(DirectoryInfo path);

    Task<bool> ValidateModels(DirectoryInfo path);
}