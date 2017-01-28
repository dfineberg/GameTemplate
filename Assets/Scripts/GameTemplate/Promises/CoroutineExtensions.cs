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

	public static IPromise<Object> WaitForSeconds(float time)
	{
		if(!_instance)
			Init();

		Promise<Object> p = new Promise<Object>();
		_instance.StartCoroutine(_instance.WaitForSecondsRoutine(time, p));
		return p;
	}

	public static IPromise<Object> WaitUntil(System.Func<bool> evaluator)
	{
		if(!_instance)
			Init();

		Promise<Object> p = new Promise<Object>();
		_instance.StartCoroutine(_instance.WaitUntilRoutine(evaluator, p));
		return p;
	}

	public static IPromise<Object> Tween(float time, Easing.Functions easing, System.Action<float> onUpdate)
	{
		if(!_instance)
			Init();

		Promise<Object> p = new Promise<Object>();
		_instance.StartCoroutine(_instance.TweenRoutine(time, easing, onUpdate, p));
		return p;
	}

	public static IPromise<Object> Tween(float time, System.Action<float> onUpdate)
	{
		return Tween(time, Easing.Functions.Linear, onUpdate);
	}

	IEnumerator WaitForSecondsRoutine(float time, Promise<Object> promise)
	{
		yield return new WaitForSeconds(time);
		promise.Resolve(null);
	}

	IEnumerator WaitUntilRoutine(System.Func<bool> evaluator, Promise<Object> promise)
	{
		yield return new WaitUntil(evaluator);
		promise.Resolve(null);
	}

	IEnumerator TweenRoutine(float time, Easing.Functions easing, System.Action<float> onUpdate, Promise<Object> promise)
	{
		float f = 0f;

		while(f <= time)
		{
			onUpdate(Easing.Interpolate(f / time, easing));
			f += Time.deltaTime;
			yield return null;
		}

		onUpdate(1f);
		promise.Resolve(null);
	}
}
