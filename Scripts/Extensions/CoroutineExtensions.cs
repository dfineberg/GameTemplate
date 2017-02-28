using System;
using System.Collections;
using Promises;
using UnityEngine;

public class CoroutineExtensions : MonoBehaviour
{
    private static CoroutineExtensions _instance;

    private static void Init()
    {
        var obj = new GameObject("CoroutineExtensions") {hideFlags = HideFlags.HideInHierarchy};
        DontDestroyOnLoad(obj);
        _instance = obj.AddComponent<CoroutineExtensions>();
    }

    public static IPromise WaitForCoroutine(IEnumerator coroutine)
    {
        if (!_instance)
            Init();

        var p = new Promise();
        _instance.StartCoroutine(_instance.WaitForCoroutine(coroutine, p));
        return p;
    }

    public static IPromise WaitForSeconds(float time)
    {
        return WaitForCoroutine(WaitForSecondsRoutine(time));
    }

    public static IPromise WaitUntil(Func<bool> evaluator)
    {
        return WaitForCoroutine(WaitUntilRoutine(evaluator));
    }

    public static IPromise WaitUntil(YieldInstruction yieldInstruction)
    {
        return WaitForCoroutine(WaitUntilRoutine(yieldInstruction));
    }

    public static IPromise WaitForEndOfFrame()
    {
        return WaitForCoroutine(WaitForEndOfFrameRoutine());
    }

    public static IPromise Tween(float time, Easing.Functions easing, Action<float> onUpdate)
    {
        return WaitForCoroutine(TweenRoutine(time, easing, onUpdate));
    }

    public static IPromise Tween(float time, Action<float> onUpdate)
    {
        return Tween(time, Easing.Functions.Linear, onUpdate);
    }

    public static IPromise Tween<T>(float time, Easing.Functions easing, T fromValue, T toValue,
        Action<T, T, float> onUpdate)
    {
        return Tween(
            time,
            easing,
            t => onUpdate(fromValue, toValue, t)
        );
    }

    public static IPromise Tween<T>(float time, T fromValue, T toValue, Action<T, T, float> onUpdate)
    {
        return Tween(
            time,
            Easing.Functions.Linear,
            fromValue,
            toValue,
            onUpdate
        );
    }

    private IEnumerator WaitForCoroutine(IEnumerator coroutine, Promise promise)
    {
        yield return StartCoroutine(coroutine);
        promise.Resolve();
    }

    private static IEnumerator WaitForSecondsRoutine(float time)
    {
        yield return new WaitForSeconds(time);
    }

    private static IEnumerator WaitUntilRoutine(Func<bool> evaluator)
    {
        yield return new WaitUntil(evaluator);
    }

    private static IEnumerator WaitUntilRoutine(YieldInstruction yieldInstruction)
    {
        yield return yieldInstruction;
    }

    private static IEnumerator WaitForEndOfFrameRoutine()
    {
        yield return new WaitForEndOfFrame();
    }

    private static IEnumerator TweenRoutine(float time, Easing.Functions easing, Action<float> onUpdate)
    {
        var f = 0f;

        while (f <= time)
        {
            onUpdate(Easing.Interpolate(f / time, easing));
            f += Time.deltaTime;
            yield return null;
        }

        onUpdate(1f);
    }
}