using System;
using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

public class TitleScreenState : State
{
    UIScreen _titleScreen;

    public override IPromise OnEnter()
    {
        return ResourceExtensions.LoadAsync<GameObject>(
            "Test/UI/TitleScreen",
            o => _titleScreen = GameManager.canvas.InstantiateBehindLoadingScreen(o).GetComponent<UIScreen>()
            )
            .Then(GameManager.loadingScreen.AnimateOff)
            .Then(() => _titleScreen.animator.AnimateOn())
            .ThenDo(() => _titleScreen.eButtonPressed += HandleTitleScreenButtonPressed);
    }

    private void HandleTitleScreenButtonPressed(int i)
    {
        _titleScreen.canFireEvents = false;
        _titleScreen.eButtonPressed -= HandleTitleScreenButtonPressed;

        _titleScreen.animator.AnimateOff()
            .ThenDo(() => nextState = new AnimateOnOffState());
    }

    public override IPromise OnExit()
    {
        return _titleScreen.animator.AnimateOff()
            .Then(GameManager.loadingScreen.AnimateOn)
            .ThenDo(() => UnityEngine.Object.Destroy(_titleScreen.gameObject));
    }
}
