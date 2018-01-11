using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTemplate
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var each = enumerable as T[] ?? enumerable.ToArray();

            for (var i = 0; i < each.Length; i++)
            {
                action(each[i], i);
            }
            
            return each;
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var each = enumerable as T[] ?? enumerable.ToArray();
            
            foreach (var e in each)
            {
                action(e);
            }

            return each;
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

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable.ToList();

            var returnList = new List<T>();

            while (list.Count > 0)
            {
                var element = list[UnityEngine.Random.Range(0, list.Count)];
                list.Remove(element);
                returnList.Add(element);
            }

            return returnList;
        }
    }
}