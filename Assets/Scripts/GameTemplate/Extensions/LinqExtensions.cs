using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Linq {
	public static class LinqExtensions {

		public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, System.Action<T,int> action)
		{
			int i = 0;

			foreach(var e in enumerable){
				action(e, i);
				i++;
			}

			return enumerable;
		}

		public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, System.Action<T> action)
		{
			return enumerable.Each((t, i) => action(t));
		}

		public static IEnumerable<U> SelectEach<T,U>(this IEnumerable<T> enumerable, System.Func<T,int,U> func)
		{
			List<U> list = new List<U>();

			enumerable.Each((t,i) => list.Add(func(t,i)));

			return list;
		}

		public static IEnumerable<U> SelectEach<T,U>(this IEnumerable<T> enumerable, System.Func<T,U> func)
		{
			return enumerable.SelectEach((t, i) => func(t));
		}
	}
}
