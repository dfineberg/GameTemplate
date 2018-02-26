using System;
using System.Collections;
using UnityEngine;

namespace GameTemplate.Promises
{
    public class CoroutineExtensions : MonoBehaviour
    {
        private static CoroutineExtensions _instance;
        private static Coroutine _lastRoutineCache;
        public static readonly WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();

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

            var p = Promise.Create();
            _instance.StartCoroutine(_instance.WaitForCoroutine(coroutine, p));
            return p;
        }

        public static IPromise WaitForSeconds(float time, bool unscaled = false)
        {
            return WaitForCoroutine(unscaled ? WaitForSecondsRealtimeRoutine(time) : WaitForSecondsRoutine(time));
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

        public static IPromise Tween(float time, Easing.Functions easing, Action<float> onUpdate, bool unscaled = false)
        {
            return WaitForCoroutine(TweenRoutine(time, easing, onUpdate, unscaled));
        }

        public static IPromise Tween(float time, Action<float> onUpdate, bool unscaled = false)
        {
            return Tween(time, Easing.Functions.Linear, onUpdate, unscaled);
        }

        public static IPromise Tween<T>(float time, Easing.Functions easing, T fromValue, T toValue,
            Action<T, T, float> onUpdate, bool unscaled = false)
        {
            return Tween(
                time,
                easing,
                t => onUpdate(fromValue, toValue, t),
                unscaled
            );
        }

        public static IPromise Tween<T>(float time, T fromValue, T toValue, Action<T, T, float> onUpdate, bool unscaled = false)
        {
            return Tween(
                time,
                Easing.Functions.Linear,
                fromValue,
                toValue,
                onUpdate,
                unscaled
            );
        }

        public static void StopAll()
        {
            if (!_instance)
                return;
        
            _instance.StopAllCoroutines();
        }

        private IEnumerator WaitForCoroutine(IEnumerator coroutine, Promise promise)
        {
            _lastRoutineCache = StartCoroutine(coroutine);
            yield return _lastRoutineCache;
            promise.Resolve();
        }

        private static IEnumerator WaitForSecondsRoutine(float time)
        {
            var targetTime = Time.time + time;

            while (Time.time < targetTime)
                yield return null;
        }

        private static IEnumerator WaitForSecondsRealtimeRoutine(float time)
        {
            var targetTime = Time.realtimeSinceStartup + time;

            while (Time.realtimeSinceStartup < targetTime)
                yield return null;            
        }

        private static IEnumerator WaitUntilRoutine(Func<bool> evaluator)
        {
            while (!evaluator())
                yield return null;
        }

        private static IEnumerator WaitUntilRoutine(YieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;
        }

        private static IEnumerator WaitForEndOfFrameRoutine()
        {
            yield return EndOfFrame;
        }

        private static IEnumerator TweenRoutine(float time, Easing.Functions easing, Action<float> onUpdate, bool unscaled = false)
        {
            var f = 0f;

            while (f <= time)
            {
                onUpdate(Easing.Interpolate(f / time, easing));
                var deltaTime = unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                f += deltaTime;
                yield return null;
            }

            onUpdate(1f);
        }
    }
}