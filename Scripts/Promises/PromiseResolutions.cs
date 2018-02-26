using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameTemplate.Promises
{
    public partial class Promise
    {
        private abstract class PromiseResolution : IDisposable
        {
            public abstract void Resolve(object o);
            public abstract void Dispose();

            ~PromiseResolution()
            {
                Dispose();
            }
        }

        private class ActionResolution : PromiseResolution
        {
            private Action _resolutionAction;
            
            public override void Resolve(object o)
            {
                _resolutionAction();
            }

            public override void Dispose()
            {
                _resolutionAction = null;
                ObjectPool.Push(this);
            }

            public static ActionResolution Create(Action action)
            {
                var r = ObjectPool.Pop<ActionResolution>();
                r._resolutionAction = action;
                return r;
            }
        }
        
        private class GenericActionResolution<T> : PromiseResolution
        {
            private Action<T> _resolutionAction;

            public override void Resolve(object o)
            {
                _resolutionAction(o is T ? (T) o : default(T));
            }

            public override void Dispose()
            {
                _resolutionAction = null;
                ObjectPool.Push(this);
            }

            public static GenericActionResolution<T> Create(Action<T> action)
            {
                var r = ObjectPool.Pop<GenericActionResolution<T>>();
                r._resolutionAction = action;
                return r;
            }
        }
        
        private class DebugLogResolution : PromiseResolution
        {
            private string _message;
            
            public override void Resolve(object o)
            {
                Debug.Log(_message);
            }

            public override void Dispose()
            {
                _message = null;
                ObjectPool.Push(this);
            }

            public static DebugLogResolution Create(string message)
            {
                var r = ObjectPool.Pop<DebugLogResolution>();
                r._message = message;
                return r;
            }
        }
        
        private class ResolvePromiseResolution : PromiseResolution
        {
            private Promise _promise;
            
            public override void Resolve(object o)
            {
                _promise.Resolve(o);
            }

            public override void Dispose()
            {
                _promise = null;
                ObjectPool.Push(this);
            }

            public static ResolvePromiseResolution Create(Promise promise)
            {
                var r = ObjectPool.Pop<ResolvePromiseResolution>();
                r._promise = promise;
                return r;
            }
        }
        
        private class FuncResolution : PromiseResolution
        {
            private Func<IPromise> _func;
            private Promise _promise;
            
            public override void Resolve(object o)
            {
                _func().ThenResolvePromise(_promise);
            }

            public override void Dispose()
            {
                _func = null;
                _promise = null;
                ObjectPool.Push(this);
            }

            public static FuncResolution Create(Func<IPromise> func, Promise promise)
            {
                var r = ObjectPool.Pop<FuncResolution>();
                r._func = func;
                r._promise = promise;
                return r;
            }
        }
        
        private class GenericFuncResolution<T> : PromiseResolution
        {
            private Func<T, IPromise> _func;
            private Promise _promise;
            
            public override void Resolve(object o)
            {
                _func(o is T ? (T) o : default(T)).ThenResolvePromise(_promise);
            }

            public override void Dispose()
            {
                _func = null;
                _promise = null;
                ObjectPool.Push(this);
            }

            public static GenericFuncResolution<T> Create(Func<T, IPromise> func, Promise promise)
            {
                var r = ObjectPool.Pop<GenericFuncResolution<T>>();
                r._func = func;
                r._promise = promise;
                return r;
            }
        }
        
        private class FuncToEnumerableResolution : PromiseResolution
        {
            private Func<IEnumerable<IPromise>> _func;
            private Promise _promise;
            
            public override void Resolve(object o)
            {
                All(_func()).ThenResolvePromise(_promise);
            }

            public override void Dispose()
            {
                _func = null;
                _promise = null;
                ObjectPool.Push(this);
            }

            public static FuncToEnumerableResolution Create(Func<IEnumerable<IPromise>> func, Promise promise)
            {
                var r = ObjectPool.Pop<FuncToEnumerableResolution>();
                r._func = func;
                r._promise = promise;
                return r;
            }
        }
    }
}