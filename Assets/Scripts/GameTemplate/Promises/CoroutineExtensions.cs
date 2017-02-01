using System.Collections;
using Promises;
using UnityEngine;

public class CoroutineExtensions : MonoBehaviour {

	private static CoroutineExtensions _instance;

	static void Init()
	{
		GameObject obj = new GameObject("CoroutineExtensions");
		obj.hideFlags = HideFlags.HideInHierarchy;
		DontDestroyOnLoad(obj);
		_instance = obj.AddComponent<CoroutineExtensions>();
	}

	public static IPromise WaitForSeconds(float time)
	{
		if(!_instance)
			Init();

		Promise p = new Promise();
		_instance.StartCoroutine(_instance.WaitForSecondsRoutine(time, p));
		return p;
	}

	public static IPromise WaitUntil(System.Func<bool> evaluator)
	{
		if(!_instance)
			Init();

		Promise p = new Promise();
		_instance.StartCoroutine(_instance.WaitUntilRoutine(evaluator, p));
		return p;
	}

	public static IPromise Tween(float time, Easing.Functions easing, System.Action<float> onUpdate)
	{
		if(!_instance)
			Init();

		Promise p = new Promise();
		_instance.StartCoroutine(_instance.TweenRoutine(time, easing, onUpdate, p));
		return p;
	}

	public static IPromise Tween(float time, System.Action<float> onUpdate)
	{
		return Tween(time, Easing.Functions.Linear, onUpdate);
	}

	public static IPromise Tween<T>(float time, Easing.Functions easing, T fromValue, T toValue, System.Action<T,T,float> onUpdate)
	{
		return Tween(
			time,
			easing,
			t => onUpdate(fromValue, toValue, t)
		);
	}

	public static IPromise Tween<T>(float time, T fromValue, T toValue, System.Action<T,T,float> onUpdate)
	{
		return Tween(
			time,
			Easing.Functions.Linear,
			fromValue,
			toValue,
			onUpdate
		);
	}

	IEnumerator WaitForSecondsRoutine(float time, Promise promise)
	{
		yield return new WaitForSeconds(time);
		promise.Resolve();
	}

	IEnumerator WaitUntilRoutine(System.Func<bool> evaluator, Promise promise)
	{
		yield return new WaitUntil(evaluator);
		promise.Resolve();
	}

	IEnumerator TweenRoutine(float time, Easing.Functions easing, System.Action<float> onUpdate, Promise promise)
	{
		float f = 0f;

		while(f <= time)
		{
			onUpdate(Easing.Interpolate(f / time, easing));
			f += Time.deltaTime;
			yield return null;
		}

		onUpdate(1f);
		promise.Resolve();
	}
}
