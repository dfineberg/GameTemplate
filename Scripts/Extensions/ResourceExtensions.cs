using System;
using System.Collections.Generic;
using System.Linq;
using Promises;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ResourceExtensions
{
    public static IPromise LoadAsync<T>(string path, Action<T> loadHandler) where T : Object
    {
        var resourceRequest = Resources.LoadAsync<T>(path);

        return CoroutineExtensions.WaitUntil(resourceRequest)
            .ThenLog("Loaded resource at: " + path)
            .ThenDo(() => loadHandler(resourceRequest.asset as T));
    }

    public static IPromise LoadAsync(string path, Action<Object> loadHandler)
    {
        return LoadAsync<Object>(path, loadHandler);
    }

    public static IPromise LoadAllAsync<T>(IEnumerable<string> paths, Action<T[]> loadHandler) where T : Object
    {
        var pathsArray = paths as string[] ?? paths.ToArray();
        var loadedObjects = new T[pathsArray.Length];

        var promises = pathsArray.SelectEach((path, i) => LoadAsync<T>(path, o => loadedObjects[i] = o));

        return Promise.All(promises)
            .ThenDo(() => loadHandler(loadedObjects));
    }

    public static IPromise LoadAllAsync(IEnumerable<string> paths, Action<Object[]> loadHandler)
    {
        return LoadAllAsync<Object>(paths, loadHandler);
    }
}