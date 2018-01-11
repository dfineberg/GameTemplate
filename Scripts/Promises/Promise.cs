using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameTemplate.Promises
{
    public enum EPromiseState
    {
        Pending,
        Resolved,
        Rejected
    }

    public interface IPromise : IEnumerator
    {
        EPromiseState CurrentState { get; }

        Exception RejectedException { get; }

        object PromisedObject { get; }

        IPromise Then(Func<IPromise> callback);

        IPromise Then<T>(Func<T, IPromise> callback);

        IPromise ThenDo(Action callback);

        IPromise ThenDo<T>(Action<T> callback);

        IPromise ThenAll(Func<IEnumerable<IPromise>> promises);

        IPromise ThenAll(params Func<IPromise>[] promises);

        IPromise ThenWaitForSeconds(float time, bool unscaled = false);

        IPromise ThenWaitUntil(Func<bool> evaluator);

        IPromise ThenWaitUntil(YieldInstruction yieldInstruction);

        IPromise ThenTween(float time, Easing.Functions easing, Action<float> onUpdate, bool unscaled = false);

        IPromise ThenTween(float time, Action<float> onUpdate, bool unscaled = false);

        IPromise ThenTween<TU>(float time, Easing.Functions easing, TU fromValue, TU toValue,
            Action<TU, TU, float> onUpdate, bool unscaled = false);

        IPromise ThenTween<TU>(float time, TU fromValue, TU toValue, Action<TU, TU, float> onUpdate, bool unscaled = false);

        IPromise ThenLog(string message);

        IPromise Catch(Action<Exception> exceptionHandler);
    }

    public class Promise : IPromise
    {
        public EPromiseState CurrentState { get; private set; }

        public Exception RejectedException { get; private set; }
        public object PromisedObject { get; private set; }

        private readonly List<Action> _resolveCallbacks;
        private readonly List<Action<Exception>> _rejectCallbacks;

        public Promise()
        {
            CurrentState = EPromiseState.Pending;
            _resolveCallbacks = new List<Action>();
            _rejectCallbacks = new List<Action<Exception>>();
        }

        public IPromise Then(Func<IPromise> callback)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                callback()
                    .ThenDo<object>(o => p.Resolve(o));
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise Then<T>(Func<T, IPromise> callback)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                callback(PromisedObject is T ? (T) PromisedObject : default(T))
                    .ThenDo<object>(o => p.Resolve(o));
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenDo(Action callback)
        {
            if (CurrentState == EPromiseState.Resolved)
                callback();
            else
                _resolveCallbacks.Add(callback);

            return this;
        }

        public IPromise ThenDo<T>(Action<T> callback)
        {
            if (CurrentState == EPromiseState.Resolved)
                callback(PromisedObject is T ? (T) PromisedObject : default(T));
            else
                _resolveCallbacks.Add(() => callback(PromisedObject is T ? (T) PromisedObject : default(T)));

            return this;
        }

        public IPromise ThenAll(Func<IEnumerable<IPromise>> promises)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                All(promises())
                    .ThenDo<object>(o => p.Resolve(o));
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenAll(params Func<IPromise>[] promises)
        {
            return ThenAll(() => promises.SelectEach(p => p()));
        }

        public IPromise ThenWaitForSeconds(float time, bool unscaled = false)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                CoroutineExtensions.WaitForSeconds(time, unscaled)
                    .ThenDo(() => p.Resolve(PromisedObject));
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }


        public IPromise ThenWaitUntil(Func<bool> evaluator)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                CoroutineExtensions.WaitUntil(evaluator)
                    .ThenDo(() => p.Resolve(PromisedObject));
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenWaitUntil(YieldInstruction yieldInstruction)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                CoroutineExtensions.WaitUntil(yieldInstruction)
                    .ThenDo(() => p.Resolve(PromisedObject));
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenTween(float time, Easing.Functions easing, Action<float> onUpdate, bool unscaled = false)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                CoroutineExtensions.Tween(time, easing, onUpdate, unscaled)
                    .ThenDo(() => p.Resolve(PromisedObject));
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenTween(float time, Action<float> onUpdate, bool unscaled = false)
        {
            return ThenTween(time, Easing.Functions.Linear, onUpdate, unscaled);
        }

        public IPromise ThenTween<TU>(float time, Easing.Functions easing, TU fromValue, TU toValue, Action<TU, TU, float> onUpdate, bool unscaled = false)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                CoroutineExtensions.Tween(
                        time,
                        easing,
                        fromValue,
                        toValue,
                        onUpdate,
                        unscaled
                    )
                    .ThenDo(() => p.Resolve(PromisedObject));
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenTween<TU>(float time, TU fromValue, TU toValue, Action<TU, TU, float> onUpdate, bool unscaled = false)
        {
            return ThenTween(
                time,
                Easing.Functions.Linear,
                fromValue,
                toValue,
                onUpdate,
                unscaled
            );
        }

        public IPromise ThenLog(string message)
        {
            if (CurrentState == EPromiseState.Resolved)
                Debug.Log(message);
            else
                _resolveCallbacks.Add(() => Debug.Log(message));

            return this;
        }

        public IPromise Catch(Action<Exception> exceptionHandler)
        {
            _rejectCallbacks.Add(exceptionHandler);

            return this;
        }

        public static IPromise All(IEnumerable<IPromise> promises)
        {
            var returnPromise = new Promise();

            var promisesArray = promises as IPromise[] ?? promises.ToArray();
            var promisedObjects = new object[promisesArray.Length];

            if (promisesArray.Length == 0)
                return Resolved();

            foreach (var promise in promisesArray)
            {
                var thisPromise = promise;

                thisPromise.ThenDo<object>(o =>
                {
                    promisedObjects[Array.IndexOf(promisesArray, thisPromise)] = o;

                    var rejected = promisesArray.FirstOrDefault(p => p.CurrentState == EPromiseState.Rejected);

                    if (rejected != null)
                        returnPromise.Reject(rejected.RejectedException);

                    if (promisesArray.All(p => p.CurrentState == EPromiseState.Resolved))
                        returnPromise.Resolve(promisedObjects);
                });
            }

            return returnPromise;
        }

        public static IPromise All(params IPromise[] promises)
        {
            return All(promises as IEnumerable<IPromise>);
        }

        public static IPromise Resolved(object promisedObject = null)
        {
            var p = new Promise();
            p.Resolve(promisedObject);
            return p;
        }

        public void Resolve(object promisedObject = null)
        {
            if (CurrentState != EPromiseState.Pending)
                return;

            PromisedObject = promisedObject;
            CurrentState = EPromiseState.Resolved;

            foreach (var callback in _resolveCallbacks)
                callback();

            _resolveCallbacks.Clear();
        }

        public void Reject(Exception ex)
        {
            if (CurrentState != EPromiseState.Pending)
                return;

            RejectedException = ex;
            CurrentState = EPromiseState.Rejected;

            foreach (var rejectCallback in _rejectCallbacks)
                rejectCallback(ex);

            _rejectCallbacks.Clear();
        }

        #region IEnumerator // Allows Promises to work as yield instructions in Coroutines

        object IEnumerator.Current
        {
            get { return null; }
        }

        bool IEnumerator.MoveNext()
        {
            return CurrentState != EPromiseState.Resolved;
        }

        void IEnumerator.Reset()
        {
        }

        #endregion
    } // Promise
}