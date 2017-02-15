using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Promises
{
	public enum ePromiseState
	{
		PENDING,
		RESOLVED,
		REJECTED
	}

	public interface IPromise : IEnumerator
	{
		ePromiseState currentState { get; }

		Exception rejectedException { get; }

		IPromise Then(Func<IPromise> callback);

		IPromise ThenDo(Action callback);

        IPromise ThenAll(Func<IEnumerable<IPromise>> promises);

        IPromise ThenAll(params Func<IPromise>[] promises);

		IPromise ThenWaitForSeconds(float time);

		IPromise ThenWaitUntil(Func<bool> evaluator);

		IPromise ThenTween(float time, Easing.Functions easing, Action<float> onUpdate);

		IPromise ThenTween(float time, Action<float> onUpdate);

		IPromise ThenTween<U>(float time, Easing.Functions easing, U fromValue, U toValue, Action<U,U,float> onUpdate);

		IPromise ThenTween<U>(float time, U fromValue, U toValue, Action<U,U,float> onUpdate);

		IPromise ThenLog(string message);

		IPromise Catch(Action<Exception> exceptionHandler);
	}

    public class Promise : IPromise
    {
        public ePromiseState currentState
        {
            get
            {
                return _currentState;
            }
        }

        public Exception rejectedException
        {
            get
            {
                return _rejectedException;
            }
        }


        private ePromiseState _currentState;

		private Exception _rejectedException;

        private List<Action> _resolveCallbacks;
		private List<Action<Exception>> _rejectCallbacks;

        public Promise()
		{
			_currentState = ePromiseState.PENDING;
            _resolveCallbacks = new List<Action>();
			_rejectCallbacks = new List<Action<Exception>>();
        }

        public IPromise Then(Func<IPromise> callback)
        {
			Promise p = new Promise();

			Action resolution = () =>{
				callback()
				.ThenDo(p.Resolve);
			};
			
			if(currentState == ePromiseState.RESOLVED)
				resolution();
			else
				_resolveCallbacks.Add(resolution);

			return p;
        }

        public IPromise ThenDo(Action callback)
        {
			if(currentState == ePromiseState.RESOLVED)
				callback();
			else				
				_resolveCallbacks.Add(callback);

            return this;
        }

        public IPromise ThenAll(Func<IEnumerable<IPromise>> promises)
        {
            Promise p = new Promise();

            Action resolution = () =>
            {
                All(promises())
                .ThenDo(p.Resolve);
            };

            if (currentState == ePromiseState.RESOLVED)
                resolution();
            else
                _resolveCallbacks.Add(resolution);

            return p;
        }

        public IPromise ThenAll(params Func<IPromise>[] promises)
        {
            return ThenAll(promises);
        }

        public IPromise ThenWaitForSeconds(float time)
		{
			Promise p = new Promise();

			Action resolution = () =>{
				CoroutineExtensions.WaitForSeconds(time)
				.ThenDo(p.Resolve);
			};

			if(currentState == ePromiseState.RESOLVED)
				resolution();
			else
				_resolveCallbacks.Add(resolution);
			
			return p;
		}

		public IPromise ThenWaitUntil(Func<bool> evaluator)
		{
			Promise p = new Promise();

			Action resolution = () => {
				CoroutineExtensions.WaitUntil(evaluator)
				.ThenDo(p.Resolve);
			};

			if(currentState == ePromiseState.RESOLVED)
				resolution();
			else
				_resolveCallbacks.Add(resolution);

			return p;
		}

        public IPromise ThenTween(float time, Easing.Functions easing, Action<float> onUpdate)
        {
			Promise p = new Promise();

            Action resolution = () => {
				CoroutineExtensions.Tween(time, easing, onUpdate)
				.ThenDo(p.Resolve);
			};

			if(currentState == ePromiseState.RESOLVED)
				resolution();
			else
				_resolveCallbacks.Add(resolution);

			return p;
        }

        public IPromise ThenTween(float time, Action<float> onUpdate)
        {
            return ThenTween(time, Easing.Functions.Linear, onUpdate);
        }

        public IPromise ThenTween<U>(float time, Easing.Functions easing, U fromValue, U toValue, Action<U, U, float> onUpdate)
        {
            Promise p = new Promise();

			Action resolution = () =>{
				CoroutineExtensions.Tween(
					time,
					easing,
					fromValue,
					toValue,
					onUpdate
				)
				.ThenDo(p.Resolve);
			};

			if(currentState == ePromiseState.RESOLVED)
				resolution();
			else
				_resolveCallbacks.Add(resolution);

			return p;
        }

		public IPromise ThenTween<U>(float time, U fromValue, U toValue, Action<U,U,float> onUpdate)
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
			Promise returnPromise = new Promise();

			foreach(var promise in promises)
			{
				promise.ThenDo(() =>{
					var rejected = promises.FirstOrDefault(p => p.currentState == ePromiseState.REJECTED);

					if(rejected != null)
						returnPromise.Reject(rejected.rejectedException);

					if(promises.All(p => p.currentState == ePromiseState.RESOLVED))
						returnPromise.Resolve();
				});
			}

			return returnPromise;
        }

		public static IPromise All(params IPromise[] promises)
		{
			return All(promises);
		}

		public static IPromise Resolved()
		{
			Promise p = new Promise();
			p.Resolve();
			return p;
		}

		public void Resolve()
		{
			if(currentState != ePromiseState.PENDING)
                return;

            _currentState = ePromiseState.RESOLVED;

			foreach(var callback in _resolveCallbacks)
				callback();

			_resolveCallbacks.Clear();

            return;
        }

		public void Reject(Exception ex)
		{
			if(currentState != ePromiseState.PENDING)
				return;

			_rejectedException = ex;
			_currentState = ePromiseState.REJECTED;

			foreach(var rejectCallback in _rejectCallbacks)
				rejectCallback(ex);

				_rejectCallbacks.Clear();
		}

		#region IEnumerator // Allows Promises to work as yield instructions in Coroutines
        object IEnumerator.Current { get { return null; } }

        bool IEnumerator.MoveNext()
        {
            return currentState != ePromiseState.RESOLVED;
        }

        void IEnumerator.Reset() {}
		#endregion
    } // Promise
}
