using Promises;
using UnityEngine;

public class LoadSceneState : AbstractState, IRequireLoadingScreen
{
    public override IPromise OnEnter()
    {
        return SceneManagerExtensions.LoadSceneAsync("Test")
            .ThenDo(() => Object.FindObjectOfType<Test>().ETestComplete += () => NextState = new CubeState())
            .Then(GameManager.LoadingScreen.AnimateOff);
    }

    public override IPromise OnExit()
    {
        return GameManager.LoadingScreen.AnimateOn()
            .Then(() => SceneManagerExtensions.UnloadSceneAsync("Test"));
    }
}