using Promises;
using UnityEngine;

public class AnimateOnOffState : AbstractState, IRequireLoadingScreen
{
    private AnimateOnOffGroup _animator;

    public override IPromise OnEnter()
    {
        var loadAndAnimate = ResourceExtensions.LoadAsync<GameObject>(
                "Test/AnimateOnOffGroup",
                o => _animator = Object.Instantiate(o).GetComponent<AnimateOnOffGroup>()
            )
            .Then(() => GameManager.LoadingScreen.AnimateOff())
            .Then(() => _animator.AnimateOn())
            .ThenDo(() =>
            {
                GameManager.SaveManager.SaveFile.TestInt++;
                GameManager.SaveManager.Save();
            });

        CoroutineExtensions.WaitForSeconds(5f)
            .ThenDo(() => NextState = new TitleScreenState());

        return loadAndAnimate;
    }

    public override IPromise OnExit()
    {
        return _animator.AnimateOff()
            .Then(GameManager.LoadingScreen.AnimateOn)
            .ThenDo(() => Object.Destroy(_animator.gameObject));
    }
}