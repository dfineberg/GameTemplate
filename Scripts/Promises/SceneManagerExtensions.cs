using UnityEngine.SceneManagement;

namespace GameTemplate.Promises
{
    public static class SceneManagerExtensions
    {
        public static IPromise LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            return CoroutineExtensions.WaitUntil(SceneManager.LoadSceneAsync(sceneName, mode))
                .Then(() => WaitUntilSceneIsLoaded(SceneManager.GetSceneByName(sceneName)));
        }

        public static IPromise LoadSceneAsync(int sceneBuildIndex, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            return CoroutineExtensions.WaitUntil(SceneManager.LoadSceneAsync(sceneBuildIndex, mode))
                .Then(() => WaitUntilSceneIsLoaded(SceneManager.GetSceneByBuildIndex(sceneBuildIndex)));
        }

        private static IPromise WaitUntilSceneIsLoaded(Scene scene)
        {
            return CoroutineExtensions.WaitUntil(() => scene.isLoaded)
                .Then(() => Promise.Resolved(scene));
        }

        public static IPromise LoadWithoutActivating(string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            var asyncOp = SceneManager.LoadSceneAsync(sceneName, mode);
            asyncOp.allowSceneActivation = false;

            return CoroutineExtensions.WaitUntil(() => asyncOp.progress >= 0.9f)
                .Then(() => Promise.Resolved(asyncOp));
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

        public static IPromise UnloadSceneAsync()
        {
            var loadedScene = SceneManager.GetActiveScene().name;
            return UnloadSceneAsync(loadedScene);
        }
    }
}