using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

public class AnimateOnOffState : AbstractState, IRequireLoadingScreen {

	AnimateOnOffGroup _animator;

	public override IPromise OnEnter()
	{
        var loadAndAnimate = ResourceExtensions.LoadAsync<GameObject>(
            "Test/AnimateOnOffGroup",
            o => _animator = Object.Instantiate(o).GetComponent<AnimateOnOffGroup>()
        )
        .Then(() => GameManager.loadingScreen.AnimateOff())
        .Then(() => _animator.AnimateOn())
        .ThenDo(() =>
        {
            GameManager.saveManager.saveFile.testInt++;
            GameManager.saveManager.Save();
        });

		CoroutineExtensions.WaitForSeconds(5f)
		.ThenDo(() => nextState = new TitleScreenState());

		return loadAndAnimate;
	}

	public override IPromise OnExit()
	{
		return _animator.AnimateOff()
            .Then(GameManager.loadingScreen.AnimateOn)
		.ThenDo(() => Object.Destroy(_animator.gameObject));
	}
}
