using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Promises
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

        IPromise Then(Func<IPromise> callback);

        IPromise ThenDo(Action callback);

        IPromise ThenAll(Func<IEnumerable<IPromise>> promises);

        IPromise ThenAll(params Func<IPromise>[] promises);

        IPromise ThenWaitForSeconds(float time);

        IPromise ThenWaitUntil(Func<bool> evaluator);

        IPromise ThenWaitUntil(YieldInstruction yieldInstruction);

        IPromise ThenTween(float time, Easing.Functions easing, Action<float> onUpdate);

        IPromise ThenTween(float time, Action<float> onUpdate);

        IPromise ThenTween<TU>(float time, Easing.Functions easing, TU fromValue, TU toValue,
            Action<TU, TU, float> onUpdate);

        IPromise ThenTween<TU>(float time, TU fromValue, TU toValue, Action<TU, TU, float> onUpdate);

        IPromise ThenLog(string message);

        IPromise Catch(Action<Exception> exceptionHandler);
    }

    public class Promise : IPromise
    {
        public EPromiseState CurrentState { get; private set; }

        public Exception RejectedException { get; private set; }


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
                    .ThenDo(p.Resolve);
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

        public IPromise ThenAll(Func<IEnumerable<IPromise>> promises)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                All(promises())
                    .ThenDo(p.Resolve);
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

        public IPromise ThenWaitForSeconds(float time)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                CoroutineExtensions.WaitForSeconds(time)
                    .ThenDo(p.Resolve);
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
                    .ThenDo(p.Resolve);
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
                    .ThenDo(p.Resolve);
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenTween(float time, Easing.Functions easing, Action<float> onUpdate)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                CoroutineExtensions.Tween(time, easing, onUpdate)
                    .ThenDo(p.Resolve);
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenTween(float time, Action<float> onUpdate)
        {
            return ThenTween(time, Easing.Functions.Linear, onUpdate);
        }

        public IPromise ThenTween<TU>(float time, Easing.Functions easing, TU fromValue, TU toValue,
            Action<TU, TU, float> onUpdate)
        {
            var p = new Promise();

            Action resolution = () =>
            {
                CoroutineExtensions.Tween(
                        time,
                        easing,
                        fromValue,
                        toValue,
                        onUpdate
                    )
                    .ThenDo(p.Resolve);
            };

            if (CurrentState == EPromiseState.Resolved)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenTween<TU>(float time, TU fromValue, TU toValue, Action<TU, TU, float> onUpdate)
        {
            return ThenTween(
                time,
                Easing.Functions.Linear,
                fromValue,
                toValue,
                onUpdate
            );
        }

        public IPromise ThenLog(string message)
        {
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

            foreach (var promise in promisesArray)
            {
                promise.ThenDo(() =>
                {
                    var rejected = promisesArray.FirstOrDefault(p => p.CurrentState == EPromiseState.Rejected);

                    if (rejected != null)
                        returnPromise.Reject(rejected.RejectedException);

                    if (promisesArray.All(p => p.CurrentState == EPromiseState.Resolved))
                        returnPromise.Resolve();
                });
            }

            return returnPromise;
        }

        public static IPromise All(params IPromise[] promises)
        {
            return All(promises as IEnumerable<IPromise>);
        }

        public static IPromise Resolved()
        {
            var p = new Promise();
            p.Resolve();
            return p;
        }

        public void Resolve()
        {
            if (CurrentState != EPromiseState.Pending)
                return;

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