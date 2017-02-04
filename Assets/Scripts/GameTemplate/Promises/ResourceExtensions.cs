using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Promises;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ResourceExtensions {

	public static IPromise LoadAsync<T>(string path, Action<T> loadHandler) where T : Object
	{
		return CoroutineExtensions.WaitForCoroutine(LoadRoutine(path, loadHandler));
	}

	public static IPromise LoadAllAsync<T>(string[] paths, Action<T[]> loadHandler) where T : Object
	{
		T[] loadedObjects = new T[paths.Length];

		var promises = paths.SelectEach((path, i) => LoadAsync<T>(path, o => loadedObjects[i] = o));
		
		return Promise.All(promises)
		.ThenDo(() => loadHandler(loadedObjects));
	}

	public static IPromise LoadAllAsync(string[] paths, Action<Object[]> loadHandler)
	{
		return LoadAllAsync(paths, loadHandler);
	}

	static IEnumerator LoadRoutine<T>(string path, Action<T> loadHandler) where T : UnityEngine.Object
	{
		ResourceRequest request = Resources.LoadAsync<T>(path);
		yield return new WaitUntil(() => request.isDone);
		Debug.Log("Loaded resource at: " + path);
		loadHandler(request.asset as T);
	}
}
