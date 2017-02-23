using Promises;
using UnityEngine;

public abstract class AbstractMenuState : AbstractState
{
    private readonly string _resourcePath;

    protected MenuScreen Screen;

    protected AbstractMenuState(string uiScreenResourcePath)
    {
        _resourcePath = uiScreenResourcePath;
    }

    public override IPromise OnEnter()
    {
        return ResourceExtensions.LoadAsync<GameObject>(
                _resourcePath,
                HandleResourceLoaded
            )
            .Then(GameManager.LoadingScreen.AnimateOff)
            .Then(() => Screen.Animator.AnimateOn())
            .ThenDo(() => Screen.ButtonPressedEvent += HandleUiButtonPressed);
    }

    public override IPromise OnExit()
    {
        Screen.CanFireEvents = false;
        Screen.ButtonPressedEvent -= HandleUiButtonPressed;

        return Screen.Animator.AnimateOff()
            .Then(() => NextState is IRequireLoadingScreen ? GameManager.LoadingScreen.AnimateOn() : Promise.Resolved())
            .ThenDo(() => Object.Destroy(Screen.gameObject));
    }

    protected virtual void HandleResourceLoaded(GameObject loadedObject)
    {
        Screen = GameManager.Canvas.InstantiateBehindLoadingScreen(loadedObject).GetComponent<MenuScreen>();
    }

    protected abstract void HandleUiButtonPressed(int i);
}