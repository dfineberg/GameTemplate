using System;
using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

public class LoadSceneState : AbstractState, IRequireLoadingScreen
{
    public override IPromise OnEnter()
    {
        return SceneManagerExtensions.LoadSceneAsync("Test")
            .ThenDo(() => UnityEngine.Object.FindObjectOfType<Test>().eTestComplete += () => nextState = new CubeState())
            .Then(GameManager.loadingScreen.AnimateOff);
    }

    public override IPromise OnExit()
    {
        return GameManager.loadingScreen.AnimateOn()
            .Then(() => SceneManagerExtensions.UnloadSceneAsync("Test"));
    }
}
