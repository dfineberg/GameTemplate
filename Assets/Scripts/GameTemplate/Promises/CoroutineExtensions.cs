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

	public static IPromise WaitForCoroutine(IEnumerator coroutine)
	{
		if(!_instance)
			Init();

		Promise p = new Promise();
		_instance.StartCoroutine(_instance.WaitForCoroutine(coroutine, p));
		return p;
	}

	public static IPromise WaitForSeconds(float time)
	{
		return WaitForCoroutine(_instance.WaitForSecondsRoutine(time));
	}

	public static IPromise WaitUntil(System.Func<bool> evaluator)
	{
		return WaitForCoroutine(_instance.WaitUntilRoutine(evaluator));
	}

	public static IPromise Tween(float time, Easing.Functions easing, System.Action<float> onUpdate)
	{
		return WaitForCoroutine(_instance.TweenRoutine(time, easing, onUpdate));
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

	IEnumerator WaitForCoroutine(IEnumerator coroutine, Promise promise)
	{
		yield return StartCoroutine(coroutine);
		promise.Resolve();
	}

	IEnumerator WaitForSecondsRoutine(float time)
	{
		yield return new WaitForSeconds(time);
	}

	IEnumerator WaitUntilRoutine(System.Func<bool> evaluator)
	{
		yield return new WaitUntil(evaluator);
	}

	IEnumerator TweenRoutine(float time, Easing.Functions easing, System.Action<float> onUpdate)
	{
		float f = 0f;

		while(f <= time)
		{
			onUpdate(Easing.Interpolate(f / time, easing));
			f += Time.deltaTime;
			yield return null;
		}

		onUpdate(1f);
	}
}
