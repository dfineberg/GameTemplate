using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

public class AnimateOnOffState : State {

	AnimateOnOffGroup _animator;

	public override IPromise OnEnter()
	{
		var loadAndAnimate = ResourceExtensions.LoadAsync<GameObject>(
			"Test/AnimateOnOffGroup",
			o => _animator = Object.Instantiate(o).GetComponent<AnimateOnOffGroup>()
		)
		.Then(() => _animator.AnimateOn());

		CoroutineExtensions.WaitForSeconds(5f)
		.ThenDo(() => nextState = new CubeState());

		return loadAndAnimate;
	}

	public override IPromise OnExit()
	{
		return _animator.AnimateOff()
		.ThenDo(() => Object.Destroy(_animator.gameObject));
	}
}
