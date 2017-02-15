using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

public class CubeState : State {

	private GameObject _cube;
	private GameObject _capsule;

	public override IPromise OnEnter()
	{
		var loadAndAnim = ResourceExtensions.LoadAllAsync<GameObject>(
			new [] { "Test/Cube", "Test/Capsule" },
			o => {
				_cube = Object.Instantiate(o[0], new Vector3(-2f, 0f, 0f), Quaternion.identity);
				_capsule = Object.Instantiate(o[1], new Vector3(2f, 0f, 0f), Quaternion.identity);

                _cube.transform.localScale = Vector3.zero;
                _capsule.transform.localScale = Vector3.zero;
			}
		)
        .Then(GameManager.loadingScreen.AnimateOff)
		.ThenTween(
			0.2f,
			Easing.Functions.BackEaseOut,
			f => {
				_cube.transform.LerpScale(0f, 1f, f);
				_capsule.transform.LerpScale(0f, 1f, f);
			}
		);

		CoroutineExtensions.WaitForSeconds(3f)
		.ThenDo(() => nextState = new AnimateOnOffState());

		return loadAndAnim;
	}

	public override IPromise OnExit()
	{
		return CoroutineExtensions.Tween(
			0.2f,
			Easing.Functions.BackEaseIn,
			f => {
				_cube.transform.LerpScale(1f, 0f, f);
				_capsule.transform.LerpScale(1f, 0f, f);
			}
		)
        .Then(GameManager.loadingScreen.AnimateOn)
		.ThenDo(() =>{
			Object.Destroy(_cube);
			Object.Destroy(_capsule);
		});
	}
}
