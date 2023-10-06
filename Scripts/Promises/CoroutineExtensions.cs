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
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

        private static void Init()
        {
            var obj = new GameObject("CoroutineExtensions") {hideFlags = HideFlags.HideAndDontSave};
            _instance = obj.AddComponent<CoroutineExtensions>();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
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

        public static IPromise Tween(float time, Action<float> onUpdate, Easing.Functions easing = Easing.Functions.Linear, bool unscaled = false)
        {
            return WaitForCoroutine(TweenRoutine<float>(time, 0f, 0f, easing, ((from, to, t) => onUpdate(t)), unscaled));
        }
        
        public static IPromise Tween<T>(float time, T from, T to, Action<T,T,float> onUpdate, Easing.Functions easing = Easing.Functions.Linear, bool unscaled = false)
        {
            return WaitForCoroutine(TweenRoutine<T>(time, from, to, easing, onUpdate, unscaled));
        }

        public static void StopAll()
        {
            if (!_instance)
                return;
        
            _instance.StopAllCoroutines();
        }

        public static void StopRoutine(IEnumerator enumerator)
        {
            if (!_instance)
                return;

            _instance.StopCoroutine(enumerator);
        }

        private IEnumerator WaitForCoroutine(IEnumerator enumerator, Promise promise)
        {
            _lastRoutineCache = StartCoroutine(enumerator);
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

        private static IEnumerator TweenRoutine<T>(float time, T from, T to, Easing.Functions easing, Action<T,T,float> onUpdate, bool unscaled = false)
        {
            var f = 0f;

            while (f <= time)
            {
                try
                {
                    onUpdate(from, to, Easing.Interpolate(f / time, easing));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                var deltaTime = unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
                f += deltaTime;
                yield return null;
            }

            try
            {
                onUpdate(from, to, 1f);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}