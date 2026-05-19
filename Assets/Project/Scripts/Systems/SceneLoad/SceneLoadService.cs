using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneLoadService
{
    bool IsLoaded(SceneNames scene);

    Scene GetScene(SceneNames scene);

    UniTask LoadSceneAsync(
        SceneNames scene,
        LoadSceneMode mode = LoadSceneMode.Additive,
        CancellationToken token = default);

    UniTask UnloadSceneAsync(SceneNames scene, CancellationToken token = default);

    void SetActiveScene(SceneNames scene);
}

public class SceneLoadService : ISceneLoadService
{
    public bool IsLoaded(SceneNames scene)
    {
        Scene unityScene = SceneManager.GetSceneByName(scene.ToSceneName());
        return unityScene.IsValid() && unityScene.isLoaded;
    }

    public Scene GetScene(SceneNames scene) => SceneManager.GetSceneByName(scene.ToSceneName());

    public async UniTask LoadSceneAsync(
        SceneNames scene,
        LoadSceneMode mode = LoadSceneMode.Additive,
        CancellationToken token = default)
    {
        if (IsLoaded(scene))
            return;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene.ToSceneName(), mode);

        while (!loadOperation.isDone)
            await UniTask.Yield(token);
    }

    public async UniTask UnloadSceneAsync(SceneNames scene, CancellationToken token = default)
    {
        if (!IsLoaded(scene))
            return;

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene.ToSceneName());

        while (!unloadOperation.isDone)
            await UniTask.Yield(token);
    }

    public void SetActiveScene(SceneNames scene)
    {
        Scene unityScene = GetScene(scene);

        if (!unityScene.IsValid())
        {
            Debug.LogError($"SceneLoadService: scene '{scene.ToSceneName()}' is not loaded.");
            return;
        }

        SceneManager.SetActiveScene(unityScene);
    }
}
