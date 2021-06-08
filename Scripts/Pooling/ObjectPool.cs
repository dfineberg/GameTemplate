﻿using System.Collections.Generic;

namespace GameTemplate
{
    public static class ObjectPool
    {
        private static class GenericObjectPool<T> where T : new()
        {
            private static readonly Stack<T> Pool = new Stack<T>();

            public static T Pop()
            {
                while (Pool.Count > 0)
                {
                    var o = Pool.Pop();
                    if (o != null) return o;
                }

                return new T();
            }

            public static void Push(T obj)
            {
                if (obj == null || Pool.Contains(obj)) return;
                Pool.Push(obj);
            }
        }

        public static void Push<T>(T obj) where T : new()
        {
            GenericObjectPool<T>.Push(obj);
        }

        public static T Pop<T>() where T : new()
        {
            return GenericObjectPool<T>.Pop();
        }
    }
}