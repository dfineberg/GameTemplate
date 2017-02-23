using Promises;
using UnityEngine.SceneManagement;

public static class SceneManagerExtensions
{
    public static IPromise LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        var scene = default(Scene);

        return CoroutineExtensions.WaitUntil(SceneManager.LoadSceneAsync(sceneName, mode))
            .ThenDo(() => scene = SceneManager.GetSceneByName(sceneName))
            .ThenWaitUntil(() => scene.isLoaded);
    }

    public static IPromise LoadSceneAsync(int sceneBuildIndex, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        var scene = default(Scene);

        return CoroutineExtensions.WaitUntil(SceneManager.LoadSceneAsync(sceneBuildIndex, mode))
            .ThenDo(() => scene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex))
            .ThenWaitUntil(() => scene.isLoaded);
    }

    public static IPromise UnloadSceneAsync(int sceneBuildIndex)
    {
        return CoroutineExtensions.WaitUntil(SceneManager.UnloadSceneAsync(sceneBuildIndex));
    }

    public static IPromise UnloadSceneAsync(string sceneName)
    {
        return CoroutineExtensions.WaitUntil(SceneManager.UnloadSceneAsync(sceneName));
    }

    public static IPromise UnloadSceneAsync(Scene scene)
    {
        return CoroutineExtensions.WaitUntil(SceneManager.UnloadSceneAsync(scene));
    }
}