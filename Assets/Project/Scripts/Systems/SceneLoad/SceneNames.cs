/// <summary>Scene names must match .unity asset file names.</summary>
public enum SceneNames
{
    Entry,
    MainMenu,
    Level,
}

public static class SceneNamesExtensions
{
    public static string ToSceneName(this SceneNames scene) => scene.ToString();
}
