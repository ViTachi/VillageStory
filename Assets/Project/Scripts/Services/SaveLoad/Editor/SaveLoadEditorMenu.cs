using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveLoadEditorMenu
{
    [MenuItem("VillageStory/Reset Save Data", false, 0)]
    private static void ResetSaveData()
    {
        if (!EditorUtility.DisplayDialog(
                "Reset Save Data",
                "Delete save file from persistent data?\nThis cannot be undone.",
                "Reset",
                "Cancel"))
            return;

        string path = Path.Combine(Application.persistentDataPath, SaveLoadService.SaveFilePath);

        if (!File.Exists(path))
        {
            Debug.Log("[VillageStory] Save file not found, nothing to delete.");
            return;
        }

        File.Delete(path);
        Debug.Log($"[VillageStory] Save deleted: {path}");
    }
}
