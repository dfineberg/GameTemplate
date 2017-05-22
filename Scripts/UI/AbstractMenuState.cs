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

    public override void OnEnter()
    {
        GameManager.EventSystem.enabled = false;
        EnterRoutine();
    }

    protected virtual IPromise EnterRoutine()
    {
        return ResourceExtensions.LoadAsync(_resourcePath)
            .ThenDo<GameObject>(HandleResourceLoaded)
            .Then(GameManager.LoadingScreen.AnimateOff)
            .Then(() => Screen.Animator.AnimateOn())
            .ThenDo(() =>
            {
                Screen.ButtonPressedEvent += HandleButtonPressed;
                Screen.BackButtonPressedEvent += HandleBackButtonPressed;
                GameManager.EventSystem.enabled = true;
            });
    }

    private void ExitRoutine(AbstractState nextState)
    {
        GameManager.EventSystem.enabled = false;
        Screen.CanFireEvents = false;
        Screen.ButtonPressedEvent -= HandleButtonPressed;
        Screen.BackButtonPressedEvent -= HandleBackButtonPressed;

        Screen.Animator.AnimateOff()
            .ThenDo(() => NextState = nextState);
    }

    public override void OnExit()
    {
        Object.Destroy(Screen.gameObject);

        if (!(NextState is AbstractMenuState))
            GameManager.EventSystem.enabled = true;
    }

    protected virtual void HandleResourceLoaded(GameObject loadedObject)
    {
        Screen = GameManager.Canvas.InstantiateBehindLoadingScreen(loadedObject).GetComponent<MenuScreen>();
    }

    protected virtual void HandleBackButtonPressed()
    {
    }

    protected abstract void HandleButtonPressed(int i);
}