using UnityEngine;

public interface IFileService
{
    void DeleteDirectory(string way);
    void DeleteFile(string way);
    T LoadDataFromFile<T>(string way);
    string LoadTextFromFile(string way);
    void SaveDataToFile<T>(string way, T save);
    void SaveTextToFile(string way, string save);
}
