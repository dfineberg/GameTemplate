using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour {

	public Transform tweenTransform;

	private GameObject _cube;
	private GameObject _capsule;
	
	void Start()
	{
		ResourceExtensions.LoadAllAsync<GameObject>(
			new [] { "Cube", "Capsule" },
			resources => {
				_cube = Instantiate(resources[0], new Vector3(-2f, 0f, 0f), Quaternion.identity);
				_capsule = Instantiate(resources[1], new Vector3(2f, 0f, 0f), Quaternion.identity);
				}
		)
		.ThenLog("load async complete")
		.ThenWaitForSeconds(3f)
		.ThenTween(
			2f,
			Easing.Functions.CubicEaseInOut,
			t => _cube.transform.LerpPosition(Vector3.zero, new Vector3(0f, 5f, 0f), t)
		)
		.ThenLog("cube animation complete");

		CoroutineExtensions.WaitForCoroutine(TestRoutine())
		.ThenLog("wait for coroutine complete")
		.ThenTween(
			2f,
			Easing.Functions.BackEaseOut,
			new Vector3(-5f, 0f, 0f),
			new Vector3(5f, 0f, 0f),
			tweenTransform.LerpPosition
			)
		.ThenLog("anim 1")
		.ThenWaitForSeconds(1f)
		.ThenTween(
			2f,
			Easing.Functions.CircularEaseInOut,
			1f,
			3f,
			tweenTransform.LerpScale
			)
		.ThenLog("anim 2")
		.ThenWaitForSeconds(1f)
		.ThenAll(
			() => CoroutineExtensions.Tween(
				1f,
				Easing.Functions.BounceEaseOut,
				3f,
				0.5f,
				tweenTransform.LerpScale
			),
			() => CoroutineExtensions.Tween(
				3f,
				Easing.Functions.ExponentialEaseOut,
				new Vector3(5f, 0f, 0f),
				new Vector3(-5f, 3f, 0f),
				tweenTransform.LerpPosition
			)
		)
		.ThenLog("anim 3");
	}

	IEnumerator TestRoutine()
	{
		yield return new WaitForSeconds(3f);
	}
}
