namespace Application.Interfaces;

public interface IJsonDataImporter
{
    public Task<int> ImportAllFromFolderAsync(string folderPath);
}