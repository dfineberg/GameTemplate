using System.Collections.Generic;

namespace GameTemplate
{
    public static class ObjectPool
    {
        private static class GenericObjectPool<T> where T : new()
        {
            private static Stack<T> pool;
            private static Stack<T> Pool
            {
                get
                {
                    if (pool == null) Init();
                    return pool;
                }
            }

            public static void Init(int capacity = -1)
            {
                Stack<T> existingPool = pool;
                
                if (capacity > 0)
                {
                    pool = new Stack<T>(capacity);
                }
                else
                {
                    pool = new Stack<T>();
                }

                if (existingPool != null)
                {
                    foreach (T item in existingPool)
                    {
                        pool.Push(item);
                    }
                }
            }

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

        public static void Init<T>(int capacity) where T : new()
        {
            GenericObjectPool<T>.Init(capacity);
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