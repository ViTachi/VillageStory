using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class FileService : IFileService
{
    public void SaveTextToFile(string way, string save)
    {
        way = Path.Combine(Application.persistentDataPath, way);
        if (!Directory.Exists(Path.GetDirectoryName(way)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(way));
        }
        File.WriteAllText(way, save.ToString());
    }

    public string LoadTextFromFile(string way)
    {
        way = Path.Combine(Application.persistentDataPath, way);
        if (!Directory.Exists(Path.GetDirectoryName(way)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(way));
        }
        try
        {
            return File.ReadAllText(way);
        }
        catch (FileNotFoundException ex)
        {
            ex.ToString();
            return default;
        }
    }

    public void SaveDataToFile<T>(string way, T save)
    {
        way = Path.Combine(Application.persistentDataPath, way);
        if (!Directory.Exists(Path.GetDirectoryName(way)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(way));
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(way, FileMode.Create);
        bf.Serialize(fs, save);
        fs.Close();
    }

    public T LoadDataFromFile<T>(string way)
    {
        way = Path.Combine(Application.persistentDataPath, way);
        if (!Directory.Exists(Path.GetDirectoryName(way)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(way));
        }
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(way, FileMode.Open);
            T save = (T)bf.Deserialize(fs);
            fs.Close();
            return save;
        }
        catch (FileNotFoundException ex)
        {
            ex.ToString();
            return default;
        }
    }

    public void DeleteFile(string way)
    {
        way = Path.Combine(Application.persistentDataPath, way);
        if (!Directory.Exists(Path.GetDirectoryName(way)))
        {
            return;
        }
        FileInfo file = new FileInfo(way);
        file.Delete();
    }

    public void DeleteDirectory(string way)
    {
        way = Path.Combine(Application.persistentDataPath, way);
        if (!Directory.Exists(way)) return;

        DirectoryInfo di = new DirectoryInfo(way);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
        Directory.Delete(way);
    }
}
