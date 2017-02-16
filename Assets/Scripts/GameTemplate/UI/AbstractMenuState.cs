using System;
using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

public abstract class AbstractUIState : AbstractState {

    string _resourcePath;

    UIScreen _screen;

	public AbstractUIState(string UIScreenResourcePath)
    {
        _resourcePath = UIScreenResourcePath;
    }

    protected abstract void HandleUIButtonPressed(int i);

    public override IPromise OnEnter()
    {
        return ResourceExtensions.LoadAsync<GameObject>(
            _resourcePath,
            o => _screen = GameManager.canvas.InstantiateBehindLoadingScreen(o).GetComponent<UIScreen>()
            )
            .Then(GameManager.loadingScreen.AnimateOff)
            .Then(() => _screen.animator.AnimateOn())
            .ThenDo(() => _screen.eButtonPressed += HandleUIButtonPressed);
    }

    public override IPromise OnExit()
    {
        _screen.bCanFireEvents = false;
        _screen.eButtonPressed -= HandleUIButtonPressed;

        return _screen.animator.AnimateOff()
            .Then(() => nextState is IRequireLoadingScreen ? GameManager.loadingScreen.AnimateOn() : Promise.Resolved())
            .ThenDo(() => UnityEngine.Object.Destroy(_screen.gameObject));
    }
}
