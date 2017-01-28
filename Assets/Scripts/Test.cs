using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour {

	public Transform tweenTransform;
	
	void Start()
	{
		CoroutineExtensions.Tween(
			2f,
			Easing.Functions.BackEaseOut,
			t => tweenTransform.LerpPosition(new Vector3(-5f, 0f, 0f), new Vector3(5f, 0f, 0f), t)
			)
		.ThenDo(o => Debug.Log("anim 1"))
		.ThenWaitForSeconds(1f)
		.ThenTween(
			2f,
			Easing.Functions.CircularEaseInOut,
			t => tweenTransform.LerpScale(1f, 3f, t)
			)
		.ThenDo(o => Debug.Log("anim 2"))
		.ThenWaitForSeconds(1f)
		.ThenAll(
			() => CoroutineExtensions.Tween(
				1f,
				Easing.Functions.BounceEaseOut,
				t => tweenTransform.LerpScale(3f, 0.5f, t)
			),
			() => CoroutineExtensions.Tween(
				3f,
				Easing.Functions.ExponentialEaseOut,
				t => tweenTransform.LerpPosition(new Vector3(5f, 0f, 0f), new Vector3(-5f, 3f, 0f), t)
			)
		)
		.ThenDo(o => Debug.Log("anim 3"));
	}
}
