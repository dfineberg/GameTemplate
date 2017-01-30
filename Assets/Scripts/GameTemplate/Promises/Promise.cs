using System;
using System.Collections.Generic;
using UnityEngine;

namespace Promises
{
	public enum ePromiseState
	{
		PENDING,
		RESOLVED,
		REJECTED
	}

	public interface IPromise<T>
	{
		ePromiseState currentState { get; }

		T resolvedObject { get; }

		Exception rejectedException { get; }

		IPromise<U> Then<U>(Func<T, IPromise<U>> callback);

		IPromise<U> Then<U>(Func<IPromise<U>> callback);

		IPromise<T> ThenDo(Action<T> callback);

		IPromise<T> ThenDo(Action callback);

		IPromise<T[]> ThenAll(params Func<IPromise<T>>[] promises);

		IPromise<T> ThenWaitForSeconds(float time);

		IPromise<T> ThenWaitUntil(Func<bool> evaluator);

		IPromise<T> ThenTween(float time, Easing.Functions easing, Action<float> onUpdate);

		IPromise<T> ThenTween(float time, Action<float> onUpdate);

		IPromise<T> ThenTween<U>(float time, Easing.Functions easing, U fromValue, U toValue, Action<U,U,float> onUpdate);

		IPromise<T> ThenTween<U>(float time, U fromValue, U toValue, Action<U,U,float> onUpdate);

		IPromise<T> ThenLog(string message);

		IPromise<T> Catch(Action<Exception> exceptionHandler);
	}

    public class Promise<T> : IPromise<T>
    {
        public ePromiseState currentState
        {
            get
            {
                return _currentState;
            }
        }

        public T resolvedObject
        {
            get
            {
                return _resolvedObject;
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

		private T _resolvedObject;

		private Exception _rejectedException;

        private List<Action<T>> _resolveCallbacks;
		private List<Action<Exception>> _rejectCallbacks;

        public Promise()
		{
			_currentState = ePromiseState.PENDING;
            _resolveCallbacks = new List<Action<T>>();
			_rejectCallbacks = new List<Action<Exception>>();
        }

        public IPromise<U> Then<U>(Func<T, IPromise<U>> callback)
        {
			Promise<U> p = new Promise<U>();

			Action<T> resolution = objT =>{
				callback(objT)
				.ThenDo(objU => p.Resolve(objU));
			};
			
			if(currentState == ePromiseState.RESOLVED)
				resolution(resolvedObject);
			else
				_resolveCallbacks.Add(resolution);

			return p;
        }

		public IPromise<U> Then<U>(Func<IPromise<U>> callback)
		{
			return Then(o => callback());
		}

        public IPromise<T> ThenDo(Action<T> callback)
        {
			if(currentState == ePromiseState.RESOLVED)
				callback(resolvedObject);
			else				
				_resolveCallbacks.Add(callback);

            return this;
        }

		public IPromise<T> ThenDo(Action callback)
		{
			return ThenDo(o => callback());
		}

        public IPromise<T[]> ThenAll(params Func<IPromise<T>>[] promises)
        {
			Promise<T[]> p = new Promise<T[]>();

			Action<T> resolution = objT =>{
				IPromise<T>[] invokedPromises = new IPromise<T>[promises.Length];

				for(int i = 0; i < promises.Length; i++)
					invokedPromises[i] = promises[i]();

				All(invokedPromises)
				.ThenDo(objTs => p.Resolve(objTs));
			};

			if(currentState == ePromiseState.RESOLVED)
				resolution(resolvedObject);
			else
				_resolveCallbacks.Add(resolution);

			return p;
        }

		public IPromise<T> ThenWaitForSeconds(float time)
		{
			Promise<T> p = new Promise<T>();

			Action<T> resolution = t =>{
				CoroutineExtensions.WaitForSeconds(time)
				.ThenDo(o => p.Resolve(t));
			};

			if(currentState == ePromiseState.RESOLVED)
				resolution(resolvedObject);
			else
				_resolveCallbacks.Add(resolution);
			
			return p;
		}

		public IPromise<T> ThenWaitUntil(Func<bool> evaluator)
		{
			Promise<T> p = new Promise<T>();

			Action<T> resolution = t => {
				CoroutineExtensions.WaitUntil(evaluator)
				.ThenDo(o => p.Resolve(t));
			};

			if(currentState == ePromiseState.RESOLVED)
				resolution(resolvedObject);
			else
				_resolveCallbacks.Add(resolution);

			return p;
		}

        public IPromise<T> ThenTween(float time, Easing.Functions easing, Action<float> onUpdate)
        {
			Promise<T> p = new Promise<T>();

            Action<T> resolution = t => {
				CoroutineExtensions.Tween(time, easing, onUpdate)
				.ThenDo(o => p.Resolve(t));
			};

			if(currentState == ePromiseState.RESOLVED)
				resolution(resolvedObject);
			else
				_resolveCallbacks.Add(resolution);

			return p;
        }

        public IPromise<T> ThenTween(float time, Action<float> onUpdate)
        {
            return ThenTween(time, Easing.Functions.Linear, onUpdate);
        }

        public IPromise<T> ThenTween<U>(float time, Easing.Functions easing, U fromValue, U toValue, Action<U, U, float> onUpdate)
        {
            Promise<T> p = new Promise<T>();

			Action<T> resolution = t =>{
				CoroutineExtensions.Tween(
					time,
					easing,
					fromValue,
					toValue,
					onUpdate
				)
				.ThenDo(o => p.Resolve(t));
			};

			if(currentState == ePromiseState.RESOLVED)
				resolution(resolvedObject);
			else
				_resolveCallbacks.Add(resolution);

			return p;
        }

		public IPromise<T> ThenTween<U>(float time, U fromValue, U toValue, Action<U,U,float> onUpdate)
		{
			return ThenTween(
				time,
				Easing.Functions.Linear,
				fromValue,
				toValue,
				onUpdate
			);
		}

		public IPromise<T> ThenLog(string message)
		{
			_resolveCallbacks.Add(o => Debug.Log(message));

			return this;
		}

        public IPromise<T> Catch(Action<Exception> exceptionHandler)
        {
            _rejectCallbacks.Add(exceptionHandler);

			return this;
        }

		public static IPromise<T[]> All(params IPromise<T>[] promises)
        {
			Promise<T[]> p = new Promise<T[]>();
			T[] returnObjects = new T[promises.Length];

			foreach(var promise in promises)
			{
				promise.ThenDo(o =>{
					returnObjects[Array.IndexOf(promises, promise)] = o;
					
					bool resolve = true;

					for(int i = 0; i < promises.Length; i++)
					{
						if(promises[i].currentState == ePromiseState.REJECTED){
							p.Reject(p.rejectedException);
							break;
						}

						if(promises[i].currentState == ePromiseState.PENDING)
							resolve = false;
					}

					if(resolve)
						p.Resolve(returnObjects);
				});
			}

			return p;
        }

		public static IPromise<T> Resolved(T obj)
		{
			Promise<T> p = new Promise<T>();
			p.Resolve(obj);
			return p;
		}

		public void Resolve(T obj)
		{
			if(currentState != ePromiseState.PENDING)
                return;

			_resolvedObject = obj;
            _currentState = ePromiseState.RESOLVED;

			foreach(var callback in _resolveCallbacks)
				callback(obj);

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
    } // Promise
}
