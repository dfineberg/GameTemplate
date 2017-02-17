using Promises;
using UnityEngine;

public abstract class AbstractMenuState : AbstractState
{
    private readonly string _resourcePath;

    private MenuScreen _screen;

    protected AbstractMenuState(string uiScreenResourcePath)
    {
        _resourcePath = uiScreenResourcePath;
    }

    public override IPromise OnEnter()
    {
        return ResourceExtensions.LoadAsync<GameObject>(
                _resourcePath,
                o => _screen = GameManager.Canvas.InstantiateBehindLoadingScreen(o).GetComponent<MenuScreen>()
            )
            .Then(GameManager.LoadingScreen.AnimateOff)
            .Then(() => _screen.Animator.AnimateOn())
            .ThenDo(() => _screen.ButtonPressedEvent += HandleUiButtonPressed);
    }

    public override IPromise OnExit()
    {
        _screen.CanFireEvents = false;
        _screen.ButtonPressedEvent -= HandleUiButtonPressed;

        return _screen.Animator.AnimateOff()
            .Then(() => NextState is IRequireLoadingScreen ? GameManager.LoadingScreen.AnimateOn() : Promise.Resolved())
            .ThenDo(() => Object.Destroy(_screen.gameObject));
    }

    protected abstract void HandleUiButtonPressed(int i);
}