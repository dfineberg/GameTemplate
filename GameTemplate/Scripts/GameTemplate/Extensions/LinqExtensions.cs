using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var i = 0;

            var each = enumerable as T[] ?? enumerable.ToArray();

            foreach (var e in each)
            {
                action(e, i);
                i++;
            }

            return each;
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            return enumerable.Each((t, i) => action(t));
        }

        public static IEnumerable<TU> SelectEach<T, TU>(this IEnumerable<T> enumerable, Func<T, int, TU> func)
        {
            var list = new List<TU>();

            enumerable.Each((t, i) => list.Add(func(t, i)));

            return list;
        }

        public static IEnumerable<TU> SelectEach<T, TU>(this IEnumerable<T> enumerable, Func<T, TU> func)
        {
            return enumerable.SelectEach((t, i) => func(t));
        }
    }
}