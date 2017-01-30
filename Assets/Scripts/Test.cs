using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour {

	public Transform tweenTransform;
	
	void Start()
	{
		CoroutineExtensions.Tween(
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
}
