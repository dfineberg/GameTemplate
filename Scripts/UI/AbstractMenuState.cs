using GameTemplate.Promises;
using UnityEngine;

namespace GameTemplate
{
    public abstract class AbstractMenuState : AbstractState
    {
        protected readonly string AssetBundlePath;
        protected readonly string ScreenAssetName;
        protected MenuScreen Screen;
        protected GameManagerCore GameManager;

        // give the AbstractMenuState a reference to a prefab in the resources folder
        protected AbstractMenuState(string uiScreenAssetBundlePath, string screenAssetName)
        {
            AssetBundlePath = uiScreenAssetBundlePath;
            ScreenAssetName = screenAssetName;
        }

        public override void OnEnter()
        {
            // disable the EventSystem until the screen has fully animated on to prevent the user from pressing buttons before the sequence has finished
            GameManager = gameObject.GetComponent<GameManagerCore>();
            GameManagerCore.EventSystem.enabled = false;
            EnterRoutine();
        }

        protected virtual IPromise EnterRoutine()
        {
            return AssetBundleExtensions.LoadAsset(AssetBundlePath, ScreenAssetName) // load the UI screen prefab from the asset bundle
                .ThenDo<GameObject>(HandleResourceLoaded) // then instantiate the prefab with HandleResourceLoaded
                .Then(() => GameManagerCore.LoadingScreen.AnimateOff()) // then animate the loading screen off
                .Then(() => Screen.Animator.AnimateOn()) // then animate the UI screen on
                .ThenDo(() => // finally register callbacks for button presses on the UI screen
                {
                    Screen.ButtonPressedEvent += HandleButtonPressed;
                    Screen.BackButtonPressedEvent += HandleBackButtonPressed;
                    GameManagerCore.EventSystem.enabled = true;
                });
        }

        // call this when you want to move to a new state
        protected virtual IPromise ExitRoutine(AbstractState nextState, bool showLoadingScreen = false)
        {
            // unregisters callbacks and disables the EventSystem
            GameManagerCore.EventSystem.enabled = false;
            Screen.CanFireEvents = false;
            Screen.ButtonPressedEvent -= HandleButtonPressed;
            Screen.BackButtonPressedEvent -= HandleBackButtonPressed;

            return Screen.Animator.AnimateOff() // animate the current screen off
                .Then(() => showLoadingScreen ? GameManagerCore.LoadingScreen.AnimateOn() : Promise.Resolved())
                .ThenDo(() => NextState = nextState); // then go to the next state
        }

        public override void OnExit()
        {
            Object.Destroy(Screen.gameObject);

            if (!(NextState is AbstractMenuState)) // in case the next state doesn't enable the EventSystem itself
                GameManagerCore.EventSystem.enabled = true;
        }

        // override this if the screen prefab has scripts on it that need initialising
        protected virtual void HandleResourceLoaded(GameObject loadedObject)
        {
            Screen = GameManagerCore.Canvas.InstantiateBehindLoadingScreen(loadedObject).GetComponent<MenuScreen>();
        }

        protected virtual void HandleBackButtonPressed()
        {
        }

        protected abstract void HandleButtonPressed(int i);
    }
}