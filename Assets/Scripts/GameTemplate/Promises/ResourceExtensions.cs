using System.Collections;
using System.Collections.Generic;
using System;
using Promises;
using UnityEngine;

public static class ResourceExtensions {

	public static IPromise LoadAsync<T>(string path, Action<T> loadHandler) where T : UnityEngine.Object
	{
		return CoroutineExtensions.WaitForCoroutine(LoadRoutine(path, loadHandler));
	}

	static IEnumerator LoadRoutine<T>(string path, Action<T> loadHandler) where T : UnityEngine.Object
	{
		ResourceRequest request = Resources.LoadAsync<T>(path);
		yield return new WaitUntil(() => request.isDone);
		loadHandler(request.asset as T);
	}
}
