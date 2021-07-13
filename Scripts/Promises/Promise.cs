using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameTemplate.Promises
{
    public enum EPromiseState
    {
        Pooled,
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

        IPromise ThenResolvePromise(Promise promise, object promisedObject = null);

        IPromise ThenDo(Action callback);

        IPromise ThenDo<T>(Action<T> callback);

        IPromise ThenAll(Func<IEnumerable<IPromise>> promises);

        IPromise ThenAll(params Func<IPromise>[] promises);

        IPromise ThenWaitForSeconds(float time, bool unscaled = false);

        IPromise ThenWaitUntil(Func<bool> evaluator);

        IPromise ThenWaitUntil(YieldInstruction yieldInstruction);

        IPromise ThenTween(float time, Action<float> onUpdate, Easing.Functions easing = Easing.Functions.Linear, bool unscaled = false);

        IPromise ThenTween<T>(float time, T from, T to, Action<T, T, float> onUpdate,
            Easing.Functions easing = Easing.Functions.Linear, bool unscaled = false);

        IPromise ThenLog(string message);

        IPromise Catch(Action<Exception> exceptionHandler);
    }

    public partial class Promise : IPromise, IDisposable
    {
        public EPromiseState CurrentState { get; private set; }

        public Exception RejectedException { get; private set; }

        public object PromisedObject { get; private set; }

        private readonly List<PromiseResolution> _resolutions = new List<PromiseResolution>();
        private readonly List<Action<Exception>> _rejectCallbacks = new List<Action<Exception>>();

        ~Promise()
        {
            Dispose();
            System.GC.ReRegisterForFinalize(this);
        }

        public static Promise Create()
        {
            var promise = ObjectPool.Pop<Promise>();
            promise.CurrentState = EPromiseState.Pending;
            return promise;
        }

        public void Dispose()
        {
            if (CurrentState == EPromiseState.Pooled) return;
            
            _resolutions.Clear();
            _rejectCallbacks.Clear();
            RejectedException = null;
            PromisedObject = null;

            CurrentState = EPromiseState.Pooled;
            ObjectPool.Push(this);
        }

        private T PromisedObjectAs<T>()
        {
            return PromisedObject is T ? (T) PromisedObject : default(T);
        }

        public IPromise Then(Func<IPromise> callback)
        {
            var p = Create();

            if (CurrentState == EPromiseState.Resolved)
                callback().ThenResolvePromise(p);
            else
                _resolutions.Add(FuncResolution.Create(callback, p));

            return p;
        }

        public IPromise Then<T>(Func<T, IPromise> callback)
        {
            var p = Create();

            if (CurrentState == EPromiseState.Resolved)
                callback(PromisedObjectAs<T>()).ThenResolvePromise(p);
            else
                _resolutions.Add(GenericFuncResolution<T>.Create(callback, p));

            return p;
        }

        public IPromise ThenResolvePromise(Promise promise, object promisedObject = null)
        {
            if (CurrentState == EPromiseState.Resolved)
                promise.Resolve(PromisedObject);
            else
                _resolutions.Add(ResolvePromiseResolution.Create(promise, promisedObject));

            return this;
        }

        public IPromise ThenDo(Action callback)
        {
            if (CurrentState == EPromiseState.Resolved)
                callback();
            else
                _resolutions.Add(ActionResolution.Create(callback));

            return this;
        }

        public IPromise ThenDo<T>(Action<T> callback)
        {
            if (CurrentState == EPromiseState.Resolved)
                callback(PromisedObjectAs<T>());
            else
                _resolutions.Add(GenericActionResolution<T>.Create(callback));

            return this;
        }

        public IPromise ThenAll(Func<IEnumerable<IPromise>> promises)
        {
            var p = Create();

            if (CurrentState == EPromiseState.Resolved)
                All(promises()).ThenResolvePromise(p);
            else
                _resolutions.Add(FuncToEnumerableResolution.Create(promises, p));

            return p;
        }

        public IPromise ThenAll(params Func<IPromise>[] promises)
        {
            return ThenAll(() => promises.SelectEach(p => p()));
        }

        public IPromise ThenWaitForSeconds(float time, bool unscaled = false)
        {
            var p = Create();

            if (CurrentState == EPromiseState.Resolved)
                CoroutineExtensions.WaitForSeconds(time, unscaled).ThenResolvePromise(p, PromisedObject);
            else
                _resolutions.Add(WaitForSecondsResolution.Create(time, unscaled, p));

            return p;
        }

        public IPromise ThenWaitUntil(Func<bool> evaluator)
        {            
            var p = Create();

            if (CurrentState == EPromiseState.Resolved)
                CoroutineExtensions.WaitUntil(evaluator).ThenResolvePromise(p, PromisedObject);
            else
                _resolutions.Add(WaitUntilResolution.Create(evaluator, p));

            return p;
        }

        public IPromise ThenWaitUntil(YieldInstruction yieldInstruction)
        {            
            var p = Create();

            if (CurrentState == EPromiseState.Resolved)
                CoroutineExtensions.WaitUntil(yieldInstruction).ThenResolvePromise(p, PromisedObject);
            else
                _resolutions.Add(WaitUntilResolution.Create(yieldInstruction, p));

            return p;
        }

        public IPromise ThenTween(float time, Action<float> onUpdate, Easing.Functions easing = Easing.Functions.Linear, bool unscaled = false)
        {            
            var p = Create();

            if (CurrentState == EPromiseState.Resolved)
                CoroutineExtensions.Tween(time, onUpdate, easing, unscaled).ThenResolvePromise(p, PromisedObject);
            else
                _resolutions.Add(TweenResolution.Create(time, onUpdate, easing, unscaled, p));

            return p;
        }

        public IPromise ThenTween<T>(float time, T from, T to, Action<T, T, float> onUpdate,
            Easing.Functions easing = Easing.Functions.Linear, bool unscaled = false)
        {
            var p = Create();

            if (CurrentState == EPromiseState.Resolved)
                CoroutineExtensions.Tween(time, from, to, onUpdate, easing, unscaled)
                    .ThenResolvePromise(p, PromisedObject);
            else
                _resolutions.Add(GenericTweenResolution<T>.Create(time, from, to, onUpdate, easing, unscaled, p));

            return p;
        }

        public IPromise ThenLog(string message)
        {
            if (CurrentState == EPromiseState.Resolved)
                Debug.Log(message);
            else
                _resolutions.Add(DebugLogResolution.Create(message));

            return this;
        }

        public IPromise Catch(Action<Exception> exceptionHandler)
        {
            _rejectCallbacks.Add(exceptionHandler);
            return this;
        }

        public static IPromise Sequence(params Func<IPromise>[] promiseFuncs)
        {
            var promisedObjects = new object[promiseFuncs.Length];

            var sequence = Resolved();
            int i = 0;
            foreach (var promiseFunc in promiseFuncs)
            {
                int index = i;
                sequence = sequence.Then(promiseFunc).ThenDo<object>(o => promisedObjects[index] = o);
                i++;
            }

            var p = Create();
            sequence.ThenDo(() => p.Resolve(promisedObjects));
            return p;
        }

        public static IPromise All(IEnumerable<IPromise> promises)
        {
            var returnPromise = Create();

            if (promises == null)
            {
                var e = new ArgumentNullException(nameof(promises), "Tried to pass a null argument to Promise.All");
                Debug.LogException(e);
                returnPromise.Reject(e);
                return returnPromise;
            }
            
            var promisesArray = promises as IPromise[] ?? promises.ToArray();

            if (promisesArray.Any(p => p == null))
            {
                var e = new ArgumentException("Tried to pass a null promise into Promise.All");
                Debug.LogException(e);
                returnPromise.Reject(e);
                return returnPromise;
            }
            
            var promisedObjects = new object[promisesArray.Length];

            if (promisesArray.Length == 0)
                return Resolved(promisedObjects);

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
            var p = Create();
            p.Resolve(promisedObject);
            return p;
        }

        public void Resolve(object promisedObject = null)
        {
            if (CurrentState != EPromiseState.Pending)
                return;

            PromisedObject = promisedObject;
            CurrentState = EPromiseState.Resolved;

            foreach (var resolution in _resolutions)
                resolution.Resolve(PromisedObject);
        }

        public void Reject(Exception ex)
        {
            if (CurrentState != EPromiseState.Pending)
                return;

            RejectedException = ex;
            CurrentState = EPromiseState.Rejected;

            foreach (var rejectCallback in _rejectCallbacks)
                rejectCallback(ex);
        }

        #region IEnumerator // Allows Promises to work as yield instructions in Coroutines

        object IEnumerator.Current => null;

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