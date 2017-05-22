using Promises;
using UnityEngine;

public abstract class AbstractMenuState : AbstractState
{
    protected readonly string ResourcePath;

    protected MenuScreen Screen;

    // give the AbstractMenuState a reference to a prefab in the resources folder
    protected AbstractMenuState(string uiScreenResourcePath)
    {
        ResourcePath = uiScreenResourcePath;
    }

    public override void OnEnter()
    {
        // disable the EventSystem until the screen has fully animated on to prevent the user from pressing buttons before the sequence has finished
        GameManager.EventSystem.enabled = false;
        EnterRoutine();
    }

    protected virtual IPromise EnterRoutine()
    {
        return ResourceExtensions.LoadAsync(ResourcePath) // load the UI screen prefab from the Resources folder
            .ThenDo<GameObject>(HandleResourceLoaded) // then instantiate the prefab with HandleResourceLoaded
            .Then(GameManager.LoadingScreen.AnimateOff) // then animate the loading screen off
            .Then(() => Screen.Animator.AnimateOn()) // then animate the UI screen on
            .ThenDo(() => // finally register callbacks for button presses on the UI screen
            {
                Screen.ButtonPressedEvent += HandleButtonPressed;
                Screen.BackButtonPressedEvent += HandleBackButtonPressed;
                GameManager.EventSystem.enabled = true;
            });
    }

    protected virtual void ExitRoutine(AbstractState nextState) // call this when you want to move to a new state
    {
        // unregisters callbacks and disables the EventSystem
        GameManager.EventSystem.enabled = false;
        Screen.CanFireEvents = false;
        Screen.ButtonPressedEvent -= HandleButtonPressed;
        Screen.BackButtonPressedEvent -= HandleBackButtonPressed;

        Screen.Animator.AnimateOff() // animate the current screen off
            .ThenDo(() => NextState = nextState); // then go to the next state
    }

    public override void OnExit()
    {
        Object.Destroy(Screen.gameObject);

        if (!(NextState is AbstractMenuState)) // in case the next state doesn't enable the EventSystem itself
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